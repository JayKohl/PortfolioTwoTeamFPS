// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotHskDoorBeep : MonoBehaviour
    {

        public AudioSource audioSource;
        public AudioClip beepSound;

        void OnTriggerEnter(Collider other)
        {
            if (Common.CollideWithPlayer(other))
            {
                if ((audioSource != null) && (beepSound != null))
                {
                    audioSource.clip = beepSound;
                    audioSource.time = 0;
                    audioSource.Play();
                }
            }
        }

    }

}