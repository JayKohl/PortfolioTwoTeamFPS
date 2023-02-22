// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotHskDoorColony : DotHskDoorsEventsClass
    {

        [Header("Sounds")]
        public AudioSource audioSource;
        public AudioClip closeSound;
        public AudioClip openSound;
        public AudioClip blockedSound;
        [Header("Lights")]
        public Renderer doorRenderer;
        public Color blockedColor = new Color(1f, 0, 0);
        public Color inactiveColor = new Color(0, 0, 0);
        public Color activeColor = new Color(1f, 1f, 1f);
	public float intencity = 2.3f;

        private float[] soundLen = new float[3] { 0, 0, 0 };

        public void Start()
        {
            if (closeSound != null) { soundLen[0] = closeSound.length; }
            if (openSound != null) { soundLen[2] = openSound.length; }
            if (doorRenderer != null) { doorRenderer.sharedMaterial.EnableKeyword("_EMISSION"); }
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
            if (doorRenderer != null)
            {
                Color color = isOff ? inactiveColor : (isBlocked ? blockedColor : activeColor);
                if (Application.isPlaying)
                {
                    doorRenderer.material.SetColor("_EmissionColor", color * intencity);
                }
                else
                {
#if UNITY_EDITOR
                    Material rendererMaterial = Material.Instantiate(doorRenderer.sharedMaterial);
                    rendererMaterial.name = doorRenderer.sharedMaterial.name;
                    rendererMaterial.SetColor("_EmissionColor", color);
                    doorRenderer.material = rendererMaterial;
#endif
                }
            }

        }

        public override void OnInit() { }
        public override void OnStop() { }

    }

}