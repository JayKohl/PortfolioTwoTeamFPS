// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{
    public class DotHskShuttleFollowCamera : MonoBehaviour
    {
        public Transform Target;
        public float PositionFolowForce = 5f;
        public float RotationFolowForce = 5f;

        void FixedUpdate()
        {
            if (Target != null) {
                var dir = Target.rotation * Vector3.forward;
                dir.y = 0f;
                transform.position = Vector3.Lerp(transform.position, Target.position, PositionFolowForce * Time.fixedDeltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((dir.magnitude > 0f) ? dir / dir.magnitude : Vector3.forward), RotationFolowForce * Time.fixedDeltaTime);
            }
        }
    }
}