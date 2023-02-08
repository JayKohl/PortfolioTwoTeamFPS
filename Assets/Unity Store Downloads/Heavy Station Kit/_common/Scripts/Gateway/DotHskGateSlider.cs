// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{

    [System.Serializable]
    public class dotHskGateParts : System.Object
    {
        public Transform part;
        [HideInInspector] public Transform savedPart;
#if UNITY_EDITOR
        [ReadOnlyAttribute]
#endif
        public Vector3 initialPos = new Vector3();
        [HideInInspector] public float w = 0f;
        [HideInInspector] public float lt = 0f;
    }

    public abstract class DotHskGatePartEventsClass : MonoBehaviour
    {
        public DotHskGateSlider part;
        public abstract void OnInit();
        public abstract void OnMotion(int motionEvent);
    }

    [ExecuteInEditMode]
    public class DotHskGateSlider : MonoBehaviour
    {

        public bool setupMode = false;
        [Range(0f, 1f)] [Tooltip("Current state - 0.0 (closed) .. 1.0 (open)")] public float currentState = 0.0f;
        [Space]
        public float motionTime = 1f;
#if UNITY_EDITOR
        [ReadOnlyAttribute]
#endif
        public float motionDistance;
        [Space]
        public Vector3 tuckOffset = new Vector3();
        public float brokenOpen = 0.0f;
        public float brokenClosed = 0.0f;
        [Space]
        public List<dotHskGateParts> gateParts;

        [HideInInspector] public DotHskGate gate;
        [HideInInspector] public int gateID;
        [HideInInspector] public DotHskGatePartEventsClass partEvents;
        [HideInInspector] [SerializeField] private float tuck_offset;
        [HideInInspector] [SerializeField] private Vector3 SavedTuckOffset = new Vector3();

        private int motionState; // 1 - general motion, 2 - tuck motion
        [HideInInspector] public int motionDir;  // +1 - forward, -1 - backward, 0 - stopped
        [HideInInspector] public float targetState = 0.0f;
        private bool prevSetupMode = false;
        private float delta = 0.001f;
        private int tuckedParts = 0;
        private float lastPartTuck = 0f;
        private float lastPartMove = 0f;

        // General public method to move gate flap parts to appropriate position
        public void MoveTo(bool isClosed, bool isBroken, bool isInit)
        {
            float to_state = isBroken ? (isClosed ? brokenClosed : brokenOpen) : (isClosed ? 0f : 1f);
            if (!isInit && Application.isPlaying)
            {
                targetState = to_state;
            }
            else
            {
                PlaceToState(currentState = targetState = to_state, false, false);
            }
        }

        // Private class methods
        void Start()
        {
            if (setupMode)
            {
                SavePartsPlacement();
                prevSetupMode = setupMode = false;
            }
            Init();
        }

        void Init()
        {
            targetState = currentState;
            motionState = motionDir = 0;
            if (partEvents == null)
            {
                partEvents = GetComponent<DotHskGatePartEventsClass>();
                if (partEvents != null)
                {
                    partEvents.part = this;
                }
            }
            if (partEvents != null)
            {
                partEvents.OnInit();
            }
        }

        void Update()
        {
            if (gateParts == null) { return; }
            if (Application.isPlaying)
            {
                int new_motionDir = (targetState != currentState) ? ((targetState > currentState) ? 1 : -1) : 0;
                if ((new_motionDir != 0) && (motionDir == 0))
                {
                    if (partEvents != null) { partEvents.OnMotion(-1); }
                    if (gate != null) { gate.onGateSliderEvent(gateID, -1); }
                }
                if ((motionDir = new_motionDir) != 0)
                {
                    float newState = currentState + motionDir * ((motionTime == 0) ? 1f : 1f / motionTime) * Time.deltaTime;
                    if (motionDir * (targetState - newState) < delta)
                    {
                        newState = targetState;
                        motionDir = 0;
                    }
                    if ((newState != currentState) || (motionDir == 0))
                    {
                        PlaceToState(currentState = newState, motionDir == 0, true);
                    }
                }
            }
            else
            {
                if (setupMode || prevSetupMode)
                {
                    SavePartsPlacement();
                }
                else
                {
                    RestorePartsPlacement();
                }
                prevSetupMode = setupMode;
            }
        }

        private void SavePartsPlacement()
        {
            currentState = 0.0f;
            tuck_offset = tuckOffset.magnitude;
            SavedTuckOffset = tuckOffset;
            int j = -1;
            for (int i = 0; i < gateParts.Count; i++)
            {
                gateParts[i].savedPart = gateParts[i].part;
                if (gateParts[i].part == null) { continue; }
                if (j > -1)
                {
                    gateParts[i].lt = gateParts[j].lt + gateParts[j].w + tuck_offset;
                    gateParts[i].w = Math.Abs(Vector3.Distance(gateParts[i].initialPos, gateParts[j].initialPos));
                }
                else
                {
                    gateParts[i].lt = gateParts[i].w = 0f;
                }
                gateParts[i].initialPos = gateParts[i].part.localPosition;
                j = i;
            }
            motionDistance = (j > -1) ? gateParts[j].lt + gateParts[j].w : 0;
            motionState = motionDir = 0;
        }

        private void RestorePartsPlacement()
        {
            tuckOffset = SavedTuckOffset;
            for (int i = 0; i < gateParts.Count; i++)
            {
                gateParts[i].part = gateParts[i].savedPart;
            }
            PlaceToState(currentState, false, false);
        }

        private void PlaceToState(float toState, bool _isStopped, bool callEvents)
        {
            float to_state = toState * motionDistance;
            tuckedParts = -1;
            lastPartTuck = 0f;
            lastPartMove = 0f;
            float ll;
            int _motionState = 0;
            // 1st step - enumerate tuked parts
            for (int i = 1; i < gateParts.Count; i++)
            {
                tuckedParts++;
                lastPartTuck = 1f;
                ll = gateParts[i].lt;
                if (to_state < ll + 0.5 * delta)
                {
                    // Part trak phase
                    if (ll - to_state > delta)
                    {
                        lastPartTuck -= (ll - to_state) / tuck_offset;
                    }
                    _motionState = 2;
                    break;
                }
                else
                {
                    ll += gateParts[i].w;
                    if (to_state < ll + 0.5 * delta)
                    {
                        // Part moving phase
                        lastPartMove = 1f;
                        if (ll - to_state > delta)
                        {
                            lastPartMove -= (ll - to_state) / gateParts[i].w;
                        }
                        _motionState = 1;
                        break;
                    }
                };
            }
            if (_isStopped)
            {
                _motionState = -2;
            }
            if (callEvents && ((_motionState != motionState) || _isStopped))
            {
                if (partEvents != null) { partEvents.OnMotion(_motionState); }
                if (gate != null) { gate.onGateSliderEvent(gateID, _motionState); }
            }
            motionState = _motionState;

            // 2nd step - move parts
            for (int i = 0; i < gateParts.Count; i++)
            {
                if (gateParts[i].part == null)
                {
                    continue;
                }
                if (i <= tuckedParts)
                {
                    gateParts[i].part.localPosition = gateParts[0].initialPos + tuckOffset * (tuckedParts - i + lastPartTuck);
                }
                else
                {
                    gateParts[i].part.localPosition = Vector3.Lerp(gateParts[i - tuckedParts].initialPos, gateParts[i - tuckedParts - 1].initialPos, lastPartMove);
                }
            }
        }

    }

}