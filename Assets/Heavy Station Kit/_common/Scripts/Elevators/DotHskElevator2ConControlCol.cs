// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace DotTeam.HSK
{

    public class DotHskElevator2ConControlCol : MonoBehaviour
    {

        private bool curVisible = true;
        private CursorLockMode curLock = CursorLockMode.None;
        private bool initialized = false;
        public UnityEvent OnDisplayActivated;
        public UnityEvent OnDisplayDeactivated;

        void OnTriggerEnter(Collider other)
        {
            if (Common.CollideWithPlayer(other))
            {
                if (!initialized)
                {
                    curVisible = Cursor.visible;
                    curLock = Cursor.lockState;
                    initialized = true;
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if ( OnDisplayActivated != null )
                {
                    OnDisplayActivated.Invoke();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (Common.CollideWithPlayer(other) && initialized)
            {
                Cursor.lockState = curLock;
                Cursor.visible = curVisible;
                if ( OnDisplayDeactivated != null )
                {
                    OnDisplayDeactivated.Invoke();
                }
            }
        }

    }

}