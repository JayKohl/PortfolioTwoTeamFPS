// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;
using System.Collections;

namespace DotTeam.HSK
{
    [RequireComponent(typeof(Light))]
    public class FPC_Light : MonoBehaviour
    {

        private Light lightSrc = null;
        public FPC FPCSrcipt = null;
        public Transform charCamera = null;
        private float lightHeightRatio = 0f;

        private DotControlCenter ccInstance = null;
        private KeyCode flashlightShortcut = KeyCode.L;

        void Start()
        {
            // Get Light Source
            lightSrc = GetComponent<Light>();
            // Calculate height of Light relative to height of Camera
            if (charCamera != null ){
                lightHeightRatio = (charCamera.localPosition.y != 0 ) ? transform.localPosition.y / charCamera.localPosition.y : 0f;
            }
            // Update cofiguration 
            if (DotControlCenter.instance != null)
            {
                if (DotControlCenter.instance.trackChangesSettings) { ccInstance = DotControlCenter.instance; };
                UpdateConfig(DotControlCenter.instance);
            }
        }

        void Update()
        {
            if (lightSrc != null)
            {
                bool justEnabled = false;
                // Update configuration
                if (ccInstance != null) { UpdateConfig(ccInstance); }
                // Turn Light source on / off 
                if (Input.GetKeyDown(flashlightShortcut))
                {
                    justEnabled = lightSrc.enabled = !lightSrc.enabled;
                }
                // Update y-coordinate of light sourse 
                if ( (justEnabled || (lightSrc.enabled && (FPCSrcipt!= null) && (FPCSrcipt.isHeightChanged))) && (charCamera != null)  ) {
                    Vector3 localPos = transform.localPosition;
                    localPos.y = charCamera.localPosition.y * lightHeightRatio;
                    transform.localPosition = localPos;
                }
            }
        }

        void UpdateConfig(DotControlCenter c)
        {
            flashlightShortcut = c.flashlightShortcut;
        }
    }

}