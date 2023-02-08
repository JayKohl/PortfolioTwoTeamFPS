// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public static class Common
    {

        static public bool CollideWithPlayer(Collider collider_obj)
        {
            if (collider_obj.tag == "Player") { return true; }
            GameObject gobj = collider_obj.gameObject;
            while (gobj != null)
            {
                if (gobj.tag == "Player") { return true; }
                if (gobj.transform.parent == null) { return false; }
                gobj = gobj.transform.parent.gameObject;
            };
            return false;
        }

    }

}