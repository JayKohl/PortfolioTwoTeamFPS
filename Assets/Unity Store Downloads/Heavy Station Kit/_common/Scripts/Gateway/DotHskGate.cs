// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{

    public enum dotHskGateStats { none = 0, closed = 1, broken = 2, off = 4, blocked = 8 };

    public enum dotHskGateMode
    {
        active = dotHskGateStats.closed,
        activeOpen = dotHskGateStats.none,
        blocked = dotHskGateStats.closed + dotHskGateStats.blocked,
        brokenOpen = dotHskGateStats.broken + dotHskGateStats.off,
        brokenClosed = dotHskGateStats.closed + dotHskGateStats.broken + dotHskGateStats.off,
        inactiveOpen = dotHskGateStats.off,
        inactiveClosed = dotHskGateStats.closed + dotHskGateStats.off
    };

    public abstract class DotHskGateEventsClass : MonoBehaviour
    {
        [HideInInspector] public DotHskGate gate;
        public abstract void OnInit();
        public abstract void OnChangeMode(bool isOff, bool isBlocked, bool isBroken);
        public abstract void OnMotion(int gateID, int motionEvent);
    }

    [ExecuteInEditMode]
    public class DotHskGate : MonoBehaviour
    {

        // Public setting
        public dotHskGateMode mode = dotHskGateMode.active;
        [HideInInspector] public bool isFullyOpen;
        [HideInInspector] public bool isFullyClosed;
        [HideInInspector] public bool isStopped;
        public List<DotHskGateSlider> gateFlaps;

        // Private setting
        private dotHskGateMode prevMode = dotHskGateMode.active;
        private bool first = true;
        [HideInInspector] public DotHskGateEventsClass gateEvents;
        private bool isOff;
        private bool isBlocked;

        void Update()
        {
            if (first && !Application.isPlaying)
            {
                Init();
                updateGateState();
            }
            if ((prevMode != mode) || first)
            {
                setMode(mode, first);
            }
            prevMode = mode;
            first = false;
        }

        void Start()
        {
            Init();
        }

        public bool setMode(dotHskGateMode mode, bool isInit)
        {
            int _mode = (int)mode;
            isOff = (_mode & (int)dotHskGateStats.off) != 0;
            isBlocked = (_mode & (int)dotHskGateStats.blocked) != 0;
            bool isClosed = (_mode & (int)dotHskGateStats.closed) != 0;
            bool isBroken = (_mode & (int)dotHskGateStats.broken) != 0;
            if (gateEvents != null)
            {
                gateEvents.OnChangeMode(isOff, isBlocked, isBroken);
            }
            setState(isClosed, isBroken, isInit);
            if (!Application.isPlaying)
            {
                updateGateState();
            }
            return true;
        }

        public void setState(bool isClosed, bool isBroken, bool isInit)
        {
            for (int i = 0; i < gateFlaps.Count; i++)
            {
                if (gateFlaps[i] != null)
                {
                    gateFlaps[i].MoveTo(isClosed, isBroken, isInit);
                }
            }
        }

        void Init()
        {
            if (gateEvents == null)
            {
                gateEvents = GetComponent<DotHskGateEventsClass>();
                if (gateEvents != null)
                {
                    gateEvents.gate = this;
                }
            }
            if (gateEvents != null)
            {
                gateEvents.OnInit();
            }
            if (gateFlaps == null) { return; }
            for (int i = 0; i < gateFlaps.Count; i++)
            {
                if (gateFlaps[i] != null)
                {
                    gateFlaps[i].gate = this;
                    gateFlaps[i].gateID = i;
                }
            }
        }

        // Motion Event: -1 - start motion, -2 - stop motion, 1 - start general motion, 2 - start tuck motion
        public void onGateSliderEvent(int gateID, int motionEvent)
        {
            // Update "Fully Open/Close" and "Stopped" status flags
            if (motionEvent == -1)
            {
                clearGateState();
            }
            else
            {
                if (motionEvent == -2) { updateGateState(); }
            }
            if (gateEvents != null)
            {
                gateEvents.OnMotion(gateID, motionEvent);
            }
        }

        // Check common gate "Fully Open/Close" and "Stopped" states
        private void updateGateState()
        {
            bool _open = true;
            bool _closed = true;
            bool _stopped = true;
            float _state;
            for (int i = 0; i < gateFlaps.Count; i++)
            {
                _state = gateFlaps[i].currentState;
                _open = _open && (_state == 1f);
                _closed = _closed && (_state == 0f);
                _stopped = _stopped && (gateFlaps[i].motionDir == 0);
            }
            isFullyOpen = _open;
            isFullyClosed = _closed;
            isStopped = _stopped;
        }

        // Clear common gate "Fully Open/Close" and "Stopped" states
        private void clearGateState()
        {
            isStopped = isFullyOpen = isFullyClosed = false;
        }

    }

}