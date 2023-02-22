// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{
    [RequireComponent(typeof(FPC))]
    public class FPC_Elevator : MonoBehaviour
    {

        private Transform OrgParent = null;
        private CharacterController cc = null;
        private float skinWidth = 0f;

        void Start()
        {
            cc = GetComponent<CharacterController>();
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Elevator2")
            {
                OrgParent = gameObject.transform.parent;
                gameObject.transform.parent = collider.gameObject.transform;
                if (cc != null)
                {
                    skinWidth = cc.skinWidth;
                    cc.skinWidth = 0.001f;
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Elevator2")
            {
                gameObject.transform.parent = OrgParent;
                if (cc != null)
                {
                    cc.skinWidth = skinWidth;
                }
            }
        }

    }

}