// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public enum dotHskDoorStats { none = 0, closed = 1, broken = 2, off = 4, blocked = 8 };

    public enum dotHskDoorMode
    {
        active = dotHskDoorStats.closed,
        activeOpen = dotHskDoorStats.none,
        blocked = dotHskDoorStats.closed + dotHskDoorStats.blocked,
        brokenOpen = dotHskDoorStats.broken + dotHskDoorStats.off,
        brokenClosed = dotHskDoorStats.closed + dotHskDoorStats.broken + dotHskDoorStats.off,
        inactiveOpen = dotHskDoorStats.off,
        inactiveClosed = dotHskDoorStats.closed + dotHskDoorStats.off
    };

    [ExecuteInEditMode]
    public class DotHskDoor : MonoBehaviour
    {

        // Public setting
        public dotHskDoorMode mode = dotHskDoorMode.active;
        public DotHskDoorSlider doorScript;

        // Private setting
        private dotHskDoorMode prevMode = dotHskDoorMode.active;
        private bool first = true;

        void Update()
        {
            if ((doorScript != null) && ((prevMode != mode) || first))
            {
                if (doorScript.setMode(mode, first)) { prevMode = mode; } else { mode = prevMode; }
                first = false;
            }
        }

    }

}