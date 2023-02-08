// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public enum dotHskDoorConsoleTypes { powerSwitch, blockSwitch };

    [ExecuteInEditMode]
    public class DotHskDoorConsoleCollider : MonoBehaviour
    {

        public DotHskDoorConsole doorConsole;
        public DotAnimatedTexture AnimatedTextureScript;
        public dotHskDoorConsoleTypes consoleType = dotHskDoorConsoleTypes.powerSwitch;
        public Texture banner;
        private bool _operate = false;

        private DotControlCenter ccInstance = null;
        private KeyCode interactShortcut = KeyCode.E;

        void Start()
        {
            if (doorConsole != null)
            {
                UpdateAnimationScreen();
                if (DotControlCenter.instance != null)
                {
                    if (DotControlCenter.instance.trackChangesSettings) { ccInstance = DotControlCenter.instance; };
                    UpdateConfig(DotControlCenter.instance);
                }
            }
        }

        void Update()
        {
            if (doorConsole != null)
            {
                int _mode = (int)doorConsole.GetMode();
                if ((!doorConsole.IsDoorsAttached()) ||
                     (consoleType != dotHskDoorConsoleTypes.powerSwitch) && ((_mode & (int)dotHskDoorStats.off) > 0) ||
                     (consoleType == dotHskDoorConsoleTypes.powerSwitch) && ((_mode & (int)dotHskDoorStats.broken) > 0))
                {
                    _operate = false;
                }
                if (_operate)
                {
                    if (ccInstance != null) { UpdateConfig(ccInstance); }
                    if (Input.GetKeyDown(interactShortcut) && (AnimatedTextureScript != null))
                    {
                        if (consoleType == dotHskDoorConsoleTypes.powerSwitch)
                        {
                            doorConsole.SetPowerMode((_mode & (int)dotHskDoorStats.off) > 0);
                        }
                        else
                        {
                            if ((_mode & (int)dotHskDoorStats.blocked) > 0)
                            {
                                doorConsole.ChangeMode(dotHskDoorMode.active);
                            }
                            else
                            {
                                doorConsole.ChangeMode(dotHskDoorMode.blocked);
                            }
                        }
                    }
                }
                UpdateAnimationScreen();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (Common.CollideWithPlayer(other)) { _operate = true; }
        }

        void OnTriggerExit(Collider other)
        {
            if (Common.CollideWithPlayer(other)) { _operate = false; }
        }

        void OnGUI()
        {
            if (_operate && (banner != null))
            {
                float _tw = banner.width;
                float _th = banner.height;
                GUI.DrawTexture(new Rect((Screen.width - _tw) / 2, Screen.height - 36 - _th, _tw, _th), banner, ScaleMode.ScaleToFit, true);
            }
        }

        public void UpdateAnimationScreen()
        {
            if ((doorConsole != null) && (AnimatedTextureScript != null))
            {
                int _mode = (int)doorConsole.GetMode();
                if (consoleType == dotHskDoorConsoleTypes.powerSwitch)
                {
                    if ((_mode & (int)dotHskDoorStats.broken) > 0)
                    {
                        AnimatedTextureScript.activeSequence = 2;
                    }
                    else
                    {
                        AnimatedTextureScript.activeSequence = ((_mode & (int)dotHskDoorStats.off) > 0) ? 0 : 1;
                    }
                }
                else
                {
                    if ((_mode & (int)dotHskDoorStats.broken) + (_mode & (int)dotHskDoorStats.off) > 0)
                    {
                        AnimatedTextureScript.activeSequence = 2;
                    }
                    else
                    {
                        AnimatedTextureScript.activeSequence = ((_mode & (int)dotHskDoorStats.blocked) > 0) ? 0 : 1;
                    }
                }
                if (!Application.isPlaying) { AnimatedTextureScript.ForceUpdate(); }
            }
        }

        void UpdateConfig(DotControlCenter c)
        {
            interactShortcut = c.interactShortcut;
        }

    }

}