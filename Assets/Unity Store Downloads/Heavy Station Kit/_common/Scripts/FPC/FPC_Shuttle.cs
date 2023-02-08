// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{
    [RequireComponent(typeof(FPC))]
    public class FPC_Shuttle : MonoBehaviour
    {

        private Transform OrgParent = null;

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Shuttle2") {
                OrgParent = gameObject.transform.parent;
                gameObject.transform.parent = collider.gameObject.transform;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Shuttle2") {
                gameObject.transform.parent = OrgParent;

            }
        }

    }

}