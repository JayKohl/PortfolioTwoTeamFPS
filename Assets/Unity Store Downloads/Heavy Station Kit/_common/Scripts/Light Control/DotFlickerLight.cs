// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotFlickerLight : MonoBehaviour
    {

        [Range(0f, 10f)] public float frequency = 0f;
        [Range(0f, 1f)] public float irregularity = 0f;
        [Range(0f, 1f)] public float glowDuration = 0.5f;

        private Light light_src;

        private float threshold = 0f;
        private bool prev_state = false;
        private int prev_cicle = 0;
        private float intencity = 1f;

        void Start()
        {
            light_src = GetComponent<Light>();
            intencity = light_src.intensity;
        }

        void Update()
        {
            if (frequency < 0.1f) { return; }
            int cicle = (int)(Time.time * frequency);
            if (prev_cicle != cicle)
            {
                if (irregularity < 0.1f)
                {
                    threshold = glowDuration;
                }
                else
                {
                    if (glowDuration < 0.5f)
                    {
                        threshold = glowDuration * Random.Range(0f, irregularity);
                    }
                    else
                    {
                        threshold = 1f - (1f - glowDuration) * Random.Range(0f, irregularity);
                    }
                }
                prev_cicle = cicle;
            }
            bool new_state = (Time.time % (1f / frequency)) * frequency < threshold;
            if (prev_state != new_state)
            {
                light_src.intensity = (prev_state = new_state) ? intencity : 0f;
            }
        }

    }

}