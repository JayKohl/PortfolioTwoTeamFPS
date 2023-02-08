// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;
using System.Collections;

namespace DotTeam.HSK
{
    [RequireComponent(typeof(FPC))]
    public class FPC_Ladder : MonoBehaviour
    {

        public Texture tipOnLadder = null;
        public Texture tipOffLadder = null;

        private FPC fpc;

        void Start()
        {
            fpc = GetComponent<FPC>();
        }

        void OnGUI()
        {
            if ((fpc.canClimbing) && (tipOnLadder != null) && (tipOffLadder != null))
            {
                Texture texture = (fpc.isClimbing) ? tipOffLadder : tipOnLadder;
                float tw = texture.width;
                float th = texture.height;
                GUI.DrawTexture(new Rect((Screen.width - tw) / 2, Screen.height - 36 - th, tw, th), texture, ScaleMode.ScaleToFit, true);
            }
        }
    }

}