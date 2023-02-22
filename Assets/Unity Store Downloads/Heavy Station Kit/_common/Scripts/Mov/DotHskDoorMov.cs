// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    [ExecuteInEditMode]
    public class DotHskDoorMov : DotHskDoor
    {

        // Public setting
        public DotHskMov movScript;

        // Private setting
        private int _prevMode = -1;

        void Update()
        {
            if ((movScript != null) && (_prevMode != (int)mode) && movScript.setMode(mode, _prevMode == -1)) { _prevMode = (int)mode; }
        }

    }

}