// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotHskDoorBase : DotHskDoorsEventsClass
    {

        [Header("Sounds")]
        public AudioSource audioSource;
        public AudioClip openSound;
        public AudioClip closeSound;
        public AudioClip blockedSound;
        [Header("Lights")]
        public Color blockedColor = new Color(1f, 0, 0);
        public Color activeColor = new Color(0f, 1f, 12f / 255f);
        public Light[] lights;
        [Header("Glass")]
        public Renderer doorGlass = null;
        public Material glassOn = null;
        public Material glassBlocked = null;
        public Material glassOff = null;

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

        public override void OnChangeMode(bool isOff, bool isBlocked)
        {
            if (doorGlass != null)
            {
                if (isOff)
                {
                    doorGlass.sharedMaterial = glassOff;
                }
                else
                {
                    if (isBlocked)
                    {
                        doorGlass.sharedMaterial = glassBlocked;
                    }
                    else
                    {
                        doorGlass.sharedMaterial = glassOn;
                    }
                }
            }
            if (lights.Length > 0)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].color = isOff ? new Color(0, 0, 0) : (isBlocked ? blockedColor : activeColor);
                    lights[i].enabled = !isOff;
                }
            }

        }

        public override void OnInit() { }
        public override void OnStop() { }

    }

}