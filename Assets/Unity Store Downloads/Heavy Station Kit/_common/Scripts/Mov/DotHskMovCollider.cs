// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{
    public class DotHskMovCollider : DotHskMovEventsClass
    {
        public DotHskMov movScript;
        public Texture bannerOpen;
        public Texture bannerClose;
        public Texture bannerBlocked;

        public AudioSource audioSource;
        public AudioClip openSound;
        public AudioClip closeSound;
        public AudioClip blockedSound;

        private bool _accessible = false; // true - collider trigger activated
        private bool _call = false; // true - "execution" key pressed

        private DotControlCenter ccInstance = null;
        private KeyCode interactShortcut = KeyCode.E;

        void Update()
        {
            if (movScript != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) { movScript.movEvents = this; }
#endif
                if (_accessible)
                {
                    if (_call)
                    {
                        if (movScript.getIsStopped())
                        {
                            movScript.operate(movScript.getIsFullyClosed());
                            _call = false;
                        }

                    }
                    else
                    {
                        if ((int)movScript.getMode() <= 1)
                        {
                            if (ccInstance != null) { UpdateConfig(ccInstance); }
                            _call = movScript.canOperate() && Input.GetKeyDown(interactShortcut); 
                        }
                    }
                }
            }
        }
        void OnGUI()
        {
            if (_accessible)
            {
                Texture banner = null;
                if ( (movScript.getMode() & (int)dotHskDoorStats.blocked) != 0 )
                {
                    banner = bannerBlocked;
                }
                else
                {
                    if (!_call && movScript.canOperate())
                    {
                        banner = movScript.getIsFullyClosed() ? bannerOpen : bannerClose;
                    }
                }
                if (banner != null)
                {
                    float _tw = banner.width;
                    float _th = banner.height;
                    GUI.DrawTexture(new Rect((Screen.width - _tw) / 2, Screen.height - 36 - _th, _tw, _th), banner, ScaleMode.ScaleToFit, true);
                }
            }
        }
        void Start()
        {
            if (movScript != null) {
                movScript.movEvents = this;
                if (DotControlCenter.instance != null) {
                    if (DotControlCenter.instance.trackChangesSettings) { ccInstance = DotControlCenter.instance; };
                    UpdateConfig(DotControlCenter.instance);
                }
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (Common.CollideWithPlayer(other))
            {
                if (_accessible = (movScript != null)) { if (movScript.getIsBlocked()) { playMovSound(blockedSound); } }
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (Common.CollideWithPlayer(other)) { _accessible = false; }
        }
        void UpdateConfig(DotControlCenter c)
        {
            interactShortcut = c.interactShortcut;
        }
        public override void OnChangeMotion(float targetPos, int dir)
        {
            if (dir != 0) { playMovSound(dir > 0 ? openSound : closeSound); }
        }
        public override void OnChangeMode(bool isClosed, bool isOff, bool isBlocked) {}
        public void playMovSound(AudioClip _clip)
        {
            if ((audioSource != null) && (_clip != null))
            {
                audioSource.clip = _clip;
                audioSource.Play();
            }
        }
    }
}