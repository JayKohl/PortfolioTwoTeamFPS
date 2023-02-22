// ---------------------------------------------
// Sci-Fi Heavy Station Kit  
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DotTeam.HSK
{
    public class DotHskShuttleCollider : MonoBehaviour
    {
        public DotHskShuttleController ShuttleController;
        public KeyCode Interact = KeyCode.E;
        public Texture2D shuttleControlStartTip;
        public Texture2D shuttleEngineOnTip;
        public Texture2D shuttleFlightModeTip;
        public bool DisplayGUIMenu = true;

        private DotControlCenter ccInstance = null;
        private bool isAccessed = false;

        void Update()
        {
            // Begin control
            if( isAccessed && Input.GetKey(Interact) ) {
                ShuttleController.Operate = true;
            }
            // Update "Execute" hotkey from Dot Control Centre
            if (DotControlCenter.instance != null) {
                if (DotControlCenter.instance.trackChangesSettings) { ccInstance = DotControlCenter.instance; };
                UpdateConfig(DotControlCenter.instance);
            }
        }
        void OnGUI()
        {
            if (isAccessed){
                if (ShuttleController.Operate) {
                    showPrompt(ShuttleController.EngineAct ? shuttleFlightModeTip : shuttleEngineOnTip );
                } else { 
                    showPrompt(shuttleControlStartTip, true);
                }
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (Common.CollideWithPlayer(other)) { isAccessed = false; }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (Common.CollideWithPlayer(other)) { isAccessed = (ShuttleController != null) && !ShuttleController.Operate; }
        }
        private void showPrompt(Texture2D bm, bool forceShow = false) {
            if ((forceShow || DisplayGUIMenu) && (bm != null)) {
                int _tw = bm.width;
                int _th = bm.height;
                GUI.DrawTexture(new Rect((Screen.width - _tw) / 2, Screen.height - 36 - _th, _tw, _th), bm, ScaleMode.ScaleToFit, true);
            }
        }
        private void UpdateConfig(DotControlCenter c)
        {
            Interact = c.interactShortcut;
        }
    }
}