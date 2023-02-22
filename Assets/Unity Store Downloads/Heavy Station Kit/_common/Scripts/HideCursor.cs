// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{


    public class HideCursor : MonoBehaviour
    {
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}