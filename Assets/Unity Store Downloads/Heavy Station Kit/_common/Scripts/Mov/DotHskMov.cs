// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{
    public abstract class DotHskMovEventsClass : MonoBehaviour
    {
        public abstract void OnChangeMotion(float targetPos, int dir);
        public abstract void OnChangeMode(bool isClosed, bool isOff, bool isBlocked);
    }
    [System.Serializable]
    public class dotHskMovFlap : System.Object
    {
        public Vector3 openPosition; 
        public Vector3 openRotation; 
        public Vector3 closedPosition;
        public Vector3 closedRotation;
        public Transform flap;
        public int turnStep = 0;
        private Quaternion[] rotations = null; 
        private int phases = 1;
        public Quaternion rotation( float newPos ) {
            if (newPos < 1f)
            {
                float newNPos = newPos * phases;
                int i = (int)Mathf.Floor(newNPos);
                return Quaternion.Lerp(rotations[i], rotations[i + 1], newNPos - i);
            }
            return rotations[phases];
        }
        public void setRotations(int _phases) {
            phases = (_phases < 1) ? 1 : _phases;
            rotations = new Quaternion[phases + 1];
            rotations[0].eulerAngles = closedRotation;
            if (phases > 1)
            {
                for (int i = 1; i < phases; i++) {
                    Vector3 v3 = new Vector3();
                    for (int j = 0; j < 3; j++) {
                        v3[j] = ((openRotation[j] - closedRotation[j]) / _phases) * i + closedRotation[j];
                    }
                    rotations[i] = Quaternion.Euler(v3);
                }
            }
            rotations[phases].eulerAngles = openRotation;
        }
        public int countPhases() {
            int maxAngle = 0;
            int rotAxis = 0;
            for (int i = 0; i < 3; i++)
            {
                int a = Mathf.Abs((int)openRotation[i] - (int)closedRotation[i]);
                if (a > 0)
                {
                    if (a > maxAngle) { maxAngle = a; }
                    rotAxis++;
                }
            }
            if (rotAxis + maxAngle == 0) { return 1; }
            int _tAngle = (turnStep > 10) ? turnStep : (maxAngle > 180 ? 90 : 180) / rotAxis / rotAxis;
            return maxAngle / _tAngle + ( (maxAngle % _tAngle) > 0 ? 1 : 0 );
        }
    }

    [ExecuteInEditMode]
    public class DotHskMov : MonoBehaviour
    {
        public dotHskDoorMode mode = dotHskDoorMode.active;
        [Tooltip("If 0 - movements are automatically repeated in an endless loop, if 1 or more - the movements are initiated manually and repeated the specified number of times")]
        public int repeatMotion = 1;
        [Tooltip("Perform movement in the opposite direction in every odd cycle")]
        public bool reverseOddCycles = false;
        [Tooltip("Pause between movement cycles")]
        public float delay = 0f;
        [Tooltip("Duration of one cycle of movement (without pause duration)")]
        public float motionTime = 1f;

        public List<dotHskMovFlap> movFlaps;

        [Tooltip("Flaps position in \"Broken open\" state (0 - fully closed, 1 - fully open)")]
        [Range(0f, 1f)] public float almostOpenPosition = 0f;
        [Tooltip("Flaps position in \"Broken closed\" state (0 - fully closed, 1 - fully open)")]
        [Range(0f, 1f)] public float almostClosedPosition = 0f;

        [HideInInspector] public bool wait = false;
        [HideInInspector] public DotHskMovEventsClass movEvents;

        private int dir = 0; // -1 - closing, 0 - idle, 1 - opening
        private float curPos = 0f; // normalized current position of flap [0..1]
        private float targetPos = 0f; // normalized target position

        private bool isOff = false;
        private bool isBlocked = false;

        private int _revCicles = 0; // internal loop counter -1 - permanent loop, 0 - stop, 1..N - number of cycles 
        private int _prevMode = -1; // previous mode
        private float _delay = 0; // delay duration value in progres

        public bool getIsFullyOpen() { return curPos == 1f; }
        public bool getIsFullyClosed() { return curPos == 0f; }
        public bool getIsStopped() { return dir == 0; }
        public int getDir() { return dir; }
        public int getMode() { return (int)mode; }
        public bool getIsBlocked() { return isBlocked; }

        void Start()
        {
            updatePlacement();
        }

        void Update()
        {
            if (wait)
            {
                _delay -= Time.deltaTime;
                if (_delay <= 0f)
                {
                    _delay = 0; wait = false;
                    if (movEvents != null) { movEvents.OnChangeMotion(targetPos, dir); }
                }
                else
                {
                    return;
                }
            }
            if ((_prevMode != (int)mode) && setMode(mode, _prevMode == -1))
            {
                _prevMode = (int)mode;
            }
            else
            {
                if (Application.isPlaying)
                {
                    if (dir == 0)
                    {
                        if ((repeatMotion == 0) && ((int)mode <= 1))
                        {
                            if ((curPos > 0.5f) ^ reverseOddCycles)
                            {
                                dir = 1; targetPos = 1f;
                            }
                            else
                            {
                                dir = -1; targetPos = 0f;
                            }
                            curPos = 1f - targetPos;
                            doDelay();
                        }
                    }
                    else
                    {
                        float newPos = curPos + dir * ((motionTime == 0) ? 1f : 1f / motionTime) * Time.deltaTime;
                        if (newPos * dir >= targetPos * dir)
                        {
                            newPos = targetPos;
                            if ((_revCicles == 0) || ((int)mode > 1))
                            {
                                dir = 0;
                                if (movEvents != null) { movEvents.OnChangeMotion(targetPos, dir); }
                            }
                            else
                            {
                                if (reverseOddCycles)
                                {
                                    dir *= -1;
                                    targetPos = 1f - targetPos;
                                }
                                else
                                {
                                    newPos = dir < 0 ? 1f : 0f;
                                }
                                if (_revCicles > 0) { _revCicles--; }
                                doDelay();
                            }
                        }
                        moveTo(newPos);
                    }
                }
                else
                {
                    if (repeatMotion < 0) { repeatMotion = 0; }
                    updatePlacement();
                }
            }
        }

        private void doDelay()
        {
            if (delay > 0)
            {
                _delay = delay; wait = true;
            }
            else
            {
                if (movEvents!=null) { movEvents.OnChangeMotion(targetPos, dir); }
            }
        }

        public bool setMode(dotHskDoorMode new_mode, bool isInit)
        {
            bool modeChanged = (mode != new_mode);
            mode = new_mode;
            bool isClosed = ((int)mode & (int)dotHskDoorStats.closed) != 0;
            isOff = ((int)mode & (int)dotHskDoorStats.off) != 0;
            isBlocked = ((int)mode & (int)dotHskDoorStats.blocked) != 0;
            if (modeChanged && (movEvents != null))
            {
                movEvents.OnChangeMode(isClosed, isOff, isBlocked);
            }
            if (((int)mode & (int)dotHskDoorStats.broken) != 0)
            {
                moveTo(isClosed ? almostClosedPosition : almostOpenPosition);
                dir = 0;
            }
            else
            {
                float newPos = isClosed ? 0f : 1f;
                targetPos = newPos;
                if (newPos != curPos)
                {
                    if (Application.isPlaying && !isInit)
                    {
                        dir = (newPos > curPos) ? 1 : -1;
                        if(movEvents != null){ movEvents.OnChangeMotion(targetPos, dir); }
                    }
                    else
                    {
                        dir = 0;
                        moveTo(newPos);
                    }
                }
            }
            return true;
        }

        public bool canOperate()
        {
            return ((int)mode <= 1) && (repeatMotion > 0) && (dir == 0);
        }

        public void operate(bool open)
        {
            if ((int)mode <= 1)
            {
                if (isOff || isBlocked) { return; }
                float _tp = (open ? 1f : 0f);
                if (targetPos == _tp) { return; }
                dir = (curPos < (targetPos = _tp)) ? 1 : -1;
                if (movEvents != null) { movEvents.OnChangeMotion(targetPos, dir); }
                _revCicles = repeatMotion - 1;
            }
        }

        private void moveTo(float newPos)
        {
            if (newPos != curPos)
            {
                curPos = newPos;
                place();
            }
        }
        private void place() {
            if (movFlaps != null)
            {
                for (int n = 0; n < movFlaps.Count; n++)
                {
                    if (movFlaps[n].flap != null)
                    {
                        movFlaps[n].flap.localPosition = Vector3.Lerp(movFlaps[n].closedPosition, movFlaps[n].openPosition, curPos);
                        movFlaps[n].flap.localRotation = movFlaps[n].rotation(curPos);
                    }
                }
            }
        }
        public void updatePlacement()
        {
            if (movFlaps != null)
            {
                int maxPhases = 0;
                for (int n = 0; n < movFlaps.Count; n++)
                {
                    int ph = movFlaps[n].countPhases();
                    if (ph > maxPhases) { maxPhases = ph; }
                }
                for (int n = 0; n < movFlaps.Count; n++)
                {
                    movFlaps[n].setRotations(maxPhases);
                }
                if (!Application.isPlaying) {
                    place();
                }
            }
        }

    }

}