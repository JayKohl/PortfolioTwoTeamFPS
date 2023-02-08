// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotControlCenter : MonoBehaviour
    {
        public static DotControlCenter instance = null;

        [Header("Shortcuts")]
        [Tooltip("General Shortcut")]
        public KeyCode interactShortcut = KeyCode.E;

        [Tooltip("DotFPC only Shortcut")]
        public KeyCode crouchShortcut = KeyCode.C;
        [Tooltip("DotFPC only Shortcut")]
        public KeyCode flashlightShortcut = KeyCode.L;

        [Header("Shortcut modifiers")]
        [Tooltip("Applied to the Colony's Elevator Console\n(included in C_EL_Platform Prefab)")]
        public KeyCode basementFloorsModifierKey1 = KeyCode.LeftShift;
        [Tooltip("Applied to the Colony's Elevator Console\n(included in C_EL_Platform Prefab)")]
        public KeyCode basementFloorsModifierKey2 = KeyCode.RightShift;

        [Header("Settings")]
        [Tooltip("If the check mark is set, the DotControlCenter object will not be destroyed when a new scene is loaded")]
        public bool useInOtherScenes = false;
        [Tooltip("If the check mark is set, the setting's changes will be tracked and applied for each update application cycle")]
        public bool trackChangesSettings = false;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            if (useInOtherScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

    }

}