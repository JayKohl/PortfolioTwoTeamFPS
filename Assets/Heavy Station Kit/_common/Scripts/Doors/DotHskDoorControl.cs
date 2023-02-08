// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DotTeam.HSK
{

    public enum dotHskDoorPowerOnStat { blocked, active, previous };

    [ExecuteInEditMode]
    public class DotHskDoorControl : MonoBehaviour
    {

        public dotHskDoorPowerOnStat powerOnStatus = dotHskDoorPowerOnStat.previous;
        [Tooltip("Specify the previous state, useful only for the initially inactive door and PowerOnStatus parameter is set to 'Previous'")]
        public bool blockedByDefault = true;
        [Space]
        public bool openIfPowerOff = false;
#if UNITY_EDITOR
        [ReadOnlyAttribute]
#endif
        public List<DotHskDoorConsole> consoleList;
        [HideInInspector] public DotHskDoor doorScript;
        [HideInInspector] public bool prevBlocked = false;

        public void RegisterConsole(DotHskDoorConsole item)
        {
            if (!consoleList.Contains(item)) { consoleList.Add(item); }
        }

        void Awake()
        {
            consoleList = new List<DotHskDoorConsole>();
        }

        void Start()
        {
            doorScript = GetComponentInParent<DotHskDoor>();
            if (doorScript != null)
            {
                prevBlocked = (((int)doorScript.mode & (int)dotHskDoorStats.off) > 0) ? blockedByDefault : doorScript.mode == dotHskDoorMode.blocked;
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (powerOnStatus == dotHskDoorPowerOnStat.previous)
                {
                    if ((doorScript != null) && (((int)doorScript.mode & (int)dotHskDoorStats.off) == 0))
                    {
                        blockedByDefault = (doorScript.mode == dotHskDoorMode.blocked);
                    }
                }
                else
                {
                    blockedByDefault = (powerOnStatus == dotHskDoorPowerOnStat.blocked);
                }
            }
        }
#endif

        public void SetMode(dotHskDoorMode mode)
        {
            if ((doorScript != null) && (doorScript.mode != mode))
            {
                prevBlocked = ((int)doorScript.mode & (int)dotHskDoorStats.blocked) > 0;
                doorScript.mode = mode;
            }
        }

        public void SetPowerMode(bool isOn)
        {
            if ((doorScript != null) && ((((int)doorScript.mode & (int)dotHskDoorStats.off) == 0) != isOn))
            {
                if (isOn)
                {
                    doorScript.mode = ((powerOnStatus == dotHskDoorPowerOnStat.blocked) || (powerOnStatus == dotHskDoorPowerOnStat.previous) && prevBlocked)
                    ? dotHskDoorMode.blocked : dotHskDoorMode.active;
                }
                else
                {
                    SetMode(openIfPowerOff ? dotHskDoorMode.inactiveOpen : dotHskDoorMode.inactiveClosed);
                }
            }
        }

    }

}