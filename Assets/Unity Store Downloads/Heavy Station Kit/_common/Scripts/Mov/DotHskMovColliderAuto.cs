// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{
    public class DotHskMovColliderAuto : MonoBehaviour
    {
        public DotHskMovCollider movColliderScript;
        [Tooltip("Сlose automatically if it was previously closed")]
        public bool holdState = false;

        private DotHskMov movScript;

        private bool _needToClose = true;

        void OnTriggerEnter(Collider other)
        {
            if (Common.CollideWithPlayer(other))
            {
                if (movScript!=null)
                {
                    if (movScript.getIsBlocked()) {
                        movColliderScript.playMovSound(movColliderScript.blockedSound);
                    }
                    else
                    {
                        _needToClose = holdState && !movScript.getIsFullyOpen();
                        movScript.operate(true);
                    }
                }
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (_needToClose && Common.CollideWithPlayer(other) && !movScript.getIsFullyClosed()) {
                _needToClose = false;
                movScript.operate(false);
            }
        }
        private void Start()
        {
            movScript = (movColliderScript != null) ? movColliderScript.movScript : null;
        }

    }
}
