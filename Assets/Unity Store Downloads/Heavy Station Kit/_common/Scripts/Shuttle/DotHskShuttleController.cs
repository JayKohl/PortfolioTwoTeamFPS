// ---------------------------------------------
// Sci-Fi Heavy Station Kit  
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace DotTeam.HSK
{
    public enum HSKShuttleStatus { stNone, stBegin, stEnd, stStart, stStop, stControl };
    public class DotHskShuttleController : MonoBehaviour
    {
        [Header("General")]
        public Rigidbody Model;
        [Header("Hot Keys")]
        public KeyCode SwitchEngine = KeyCode.Z;
        public KeyCode Quit = KeyCode.X;
        [Space]
        public KeyCode Forward = KeyCode.W;
        public KeyCode TurnLeft = KeyCode.A;
        public KeyCode Backward = KeyCode.S;
        public KeyCode TurnRight = KeyCode.D;
        [Space]
        public KeyCode StrafeLeft = KeyCode.Q;
        public KeyCode StrafeRight = KeyCode.E;
        [Space]
        public KeyCode Upward = KeyCode.Space;
        public KeyCode Downward = KeyCode.LeftControl;
        [Header("Forces")]
        public float TurnForce = 180f;
        public float ForwardForce = 30f;
        public float ForwardTiltForce = 15f;
        public float TurnTiltForce = 10f;
        public float StrafeForce = 30f;
        public float LiftForce = 30f;
        public float FreeFallForce = 60f;
        [Space]
        public float turnTiltForcePercent = 1.5f;
        [Header("Wiggling")]
        public float WiggleAmplitude = 10f;
        [Range(0.1f,10f)] public float WiggleDuration = 3f;
        public int WiggleDelay = 50;
        public float WiggleMinHeight = 1f;
        [Header("Miscellaneous")]
        [Range(0.1f, 100f)] public float FreeFallHeight = 15f;
        public bool Operate {
            get { return _operate; }
            set {
                if (_operate != value) {
                    doWiggling(false);
                    changeStatus(Model, (_operate = value) ? HSKShuttleStatus.stBegin : HSKShuttleStatus.stEnd, Vector4.zero);
                }
            }
        }
        public bool EngineAct { get { return _engineAct; } set { _engineAct = value; } }
        public bool OnGround { get { return _onGround; } }
        // OnChangeStatus:
        //  rb      - shuttle rigidbody
        //  param   - type of event: stNone, stBegin - begin operate, stEnd - end operate, stStart - start engine, stStop - stop engine, stControl - control
        //  control - control info:  {Turn Left/Right, Forward/Backward, Upward/Downward, Strafe Left/Right}
        public delegate void OnChangeStatus(Rigidbody rb, HSKShuttleStatus param, Vector4 control);
        public OnChangeStatus changeStatus;
        
        private Vector4 hMove = Vector4.zero;
        private Vector2 hTilt = Vector2.zero;
        private float hTurn = 0f;
        private int[] idleCycles = { 1, 1, 1, 1 };
        private KeyCode[] hotKeys;
        private bool _operate = false;
        private bool _engineAct = false;
        private bool _onGround = true;
        private bool isWiggling = false;
        private float wigglePos = 0f;
        private Quaternion wigglingAngleTo;
        private Quaternion wigglingAngleFrom;
        private Quaternion wigglingAngle;
        private RigidbodyConstraints constraintsPre;
        private Quaternion rotationPre;

        private void Start()
        {
            hotKeys = new KeyCode[] { TurnLeft, TurnRight, Backward, Forward, Downward, Upward, StrafeLeft, StrafeRight };
            constraintsPre = Model.constraints;
        }
        void FixedUpdate()
        {
            CheckControlStatus();
            ProcessControl();
        }
        private void Update()
        {
            if (Operate && Input.GetKeyDown(SwitchEngine)) {
                changeStatus(Model, (_engineAct = !_engineAct) ? HSKShuttleStatus.stStart : HSKShuttleStatus.stStop, Vector4.zero);
            }
        }
        private void ProcessControl()
        {
            // Get user input
            int _iddleAll = 0;
            Vector4 dir = Vector4.zero;
            for (int i = 0; i < 4; i++) {
                bool _isIddle = true;
                dir[i] = (hMove[i] == 0) ? 0 : ((hMove[i] < 0) ? 1 : -1);
                if (Operate && _engineAct) {
                    if (Input.GetKey(hotKeys[i * 2])) { dir[i] = -1; _isIddle = false; }
                    if (Input.GetKey(hotKeys[i * 2 + 1])) { dir[i] = +1; _isIddle = false; }
                }
                if (_isIddle) { idleCycles[i]++; } else { idleCycles[i] = 1; }
                if (idleCycles[i] > WiggleDelay) {
                    _iddleAll++;
                } else {
                    hMove[i] = Mathf.Clamp(hMove[i] + dir[i] * Time.fixedDeltaTime, -1f, 1f);
                }
            }
            // Check current Altitude
            RaycastHit hit;
            float curAltitude = Physics.Raycast(Model.gameObject.transform.position + new Vector3(0, 1f, 0), -Model.gameObject.transform.up, out hit, FreeFallHeight) ? hit.distance - 1f : float.MaxValue;
            _onGround = curAltitude < 0.1f;
            if (_engineAct) {
                // Move Upward / Downward
                if (hMove.z != 0) { Model.AddRelativeForce(Vector3.up * LiftForce * Model.mass * hMove.z * altitudeKoeff(curAltitude)); }

                // Move Forward / Backward
                if (hMove.y != 0) { Model.AddRelativeForce(Vector3.forward * ForwardForce * Model.mass * hMove.y); }

                // Strafe
                if (hMove.w != 0) { Model.AddRelativeForce(Vector3.right * StrafeForce * Model.mass * hMove.w); }

                // Turn
                hTurn = Mathf.Lerp(
                    hTurn,
                    Mathf.Lerp(hMove.x, hMove.x * (turnTiltForcePercent - Mathf.Abs(hMove.y)), Mathf.Max(0f, hMove.y)) * TurnForce,
                    Time.fixedDeltaTime * TurnForce
                );
                if (hTurn != 0) { Model.AddRelativeTorque(0f, hTurn * Model.mass, 0f); }
                // Tilt (with Wiggling)
                hTilt.x = Mathf.Lerp(hTilt.x, (hMove.x + hMove.w) * TurnTiltForce, Time.fixedDeltaTime);
                hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.fixedDeltaTime);
                Quaternion tilt = Quaternion.Euler(hTilt.y, Model.transform.localEulerAngles.y, -hTilt.x);
                doWiggling((_iddleAll > 3) && (curAltitude >= WiggleMinHeight));
                Model.transform.localRotation = isWiggling ? tilt * wigglingAngle : tilt;
                changeStatus(Model, HSKShuttleStatus.stControl, dir);
            } else {
                if (isWiggling) { doWiggling(false); }
                Model.AddRelativeForce(Vector3.up * Model.mass * -FreeFallForce * altitudeKoeff(curAltitude));
                hTilt = hMove = Vector4.zero;
                idleCycles = new int[] { 1, 1, 1, 1 };
            }
        }
        private bool doWiggling(bool isWigglingNew)
        {
            if (isWiggling != isWigglingNew) {
                if (isWigglingNew) {
                    Model.constraints = constraintsPre | RigidbodyConstraints.FreezePosition;
                    rotationPre = Model.transform.localRotation;
                    wigglingAngleTo = Quaternion.identity;
                    wigglePos = 1.1f;
                    hMove = Vector4.zero;
                } else {
                    Model.transform.localRotation = rotationPre;
                    Model.constraints = constraintsPre;
                }
            }
            if (isWigglingNew) {
                if (wigglePos >= 1f) {
                    wigglePos = 0f;
                    wigglingAngleFrom = wigglingAngleTo;
                    wigglingAngleTo = Quaternion.Euler(randomWiggle(), 0, randomWiggle());
                }
                wigglePos += Time.fixedDeltaTime / WiggleDuration;
                wigglingAngle = Quaternion.Lerp(wigglingAngleFrom, wigglingAngleTo, (wigglePos < 0.5f) ? 2 * wigglePos * wigglePos : 1f - 2 * (wigglePos - 1f) * (wigglePos - 1f));
            }
            return isWiggling = isWigglingNew;
        }
        private float altitudeKoeff(float curAltitude) {
            return ((curAltitude <= FreeFallHeight) && (hMove.z < 0)) ? Mathf.Clamp(curAltitude * curAltitude / (FreeFallHeight * FreeFallHeight), 0.1f, 1f) : 1f;
        }
        private float randomWiggle() {
            float div = Random.Range(-WiggleAmplitude, WiggleAmplitude);
            return 0.05f * ((Mathf.Abs(div) < 0.5f * WiggleAmplitude) ? (div < 0 ? -1 : 0) * 0.5f * WiggleAmplitude : div);
        }
        private void CheckControlStatus()
        {
            if (Operate && Input.GetKey(Quit)) { Operate = false; }
        }
    }
}
