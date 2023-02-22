// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotHskElevator2ConCollider : MonoBehaviour
    {

        private DotHskElevator2Console consoleScript = null;

        void Start()
        {
            consoleScript = GetComponentInParent<DotHskElevator2Console>();
        }

        void OnTriggerEnter(Collider other)
        {
            if ((consoleScript != null) && Common.CollideWithPlayer(other))
            {
                consoleScript.OnConsoleEnter(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if ((consoleScript != null) && Common.CollideWithPlayer(other))
            {
                consoleScript.OnConsoleExit(other);
            }
        }

    }

}