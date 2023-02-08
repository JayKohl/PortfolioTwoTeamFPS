// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotHskDoorNano : DotHskDoorsEventsClass
    {

        [Header("Sounds")]
        public AudioSource audioSource;
        public AudioClip closeSound;
        public AudioClip openSound;
        public AudioClip blockedSound;

        private float[] soundLen = new float[3] { 0, 0, 0 };

        public void Start()
        {
            if (closeSound != null) { soundLen[0] = closeSound.length; }
            if (openSound != null) { soundLen[2] = openSound.length; }
        }

        public override void OnStartMotion(float fromPos, int dir)
        {
            if ((audioSource != null) && (soundLen[dir + 1] > 0))
            {
                audioSource.clip = (dir == 1) ? openSound : closeSound;
                audioSource.time = (fromPos == 0) ? 0 : ((dir == 1) ? fromPos : 1f - fromPos);
                audioSource.Play();
            }
        }

        public override void OnBlock()
        {
            if ((audioSource != null) && (blockedSound != null))
            {
                audioSource.clip = blockedSound;
                audioSource.time = 0;
                audioSource.Play();
            }
        }

        public override void OnChangeMode(bool isOff, bool isBlocked) { }
        public override void OnInit() { }
        public override void OnStop() { }

    }

}