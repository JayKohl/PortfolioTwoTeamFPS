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
    [System.Serializable]
    public class DotHskShuttleCamera : System.Object
    {
        public Camera camera = null;
        public AudioListener listener = null;
        public KeyCode hotKey;
    }

    public class DotHskShuttleSupports : MonoBehaviour
    {
        public DotHskShuttleController ShuttleController;
        public GameObject PersonController;
        public KeyCode ToggleChasis = KeyCode.C;
        [Space]
        public List<DotHskShuttleTurbine> Turbines;
        public List<DotHskMov> Chasis;
        public List<DotHskShuttleCamera> Cameras;
        
        private struct colliderItem { public Collider mCollider; public bool isEnable; }
        private List<colliderItem> cItems = null;
        private List<Collider> fItems = null;
        private Camera _mainCamera = null;
        private int _currentCamera = -1;
        private Rigidbody _shuttleModel = null;

        void Start()
        {
            if (init()) {
                collectColliders();
                disableShuttleCameras();
            }
        }
        void Update()
        {
            if (_shuttleModel != null) {
                switchCameras(ShuttleController.Operate);
                if (Input.GetKeyDown(ToggleChasis) && ShuttleController.Operate && ShuttleController.EngineAct) {
                    toggleChasis();
                }
            }
        }
        private bool init()
        {
            // Main camera
            _mainCamera = Camera.main;
            if (ShuttleController != null){
                // Shuttle rigidBody
                _shuttleModel = ShuttleController.Model;
                // Set initial state of Shuttle (No control)
                toggleControl(HSKShuttleStatus.stStop);
                ShuttleController.changeStatus = StatusChanged;
            }
            return _shuttleModel != null;
        }
        private void StatusChanged(Rigidbody rb, HSKShuttleStatus param, Vector4 control)
        {
            if (_shuttleModel != null){
                switch (param){
                    case HSKShuttleStatus.stBegin: // Begin shuttle control 
                    case HSKShuttleStatus.stEnd: // End shuttle control 
                        toggleControl(param);
                        break;
                    case HSKShuttleStatus.stStart: // Start shuttle engine
                    case HSKShuttleStatus.stStop: // Stop shhutle engine
                    case HSKShuttleStatus.stControl: // Control action
                        turbineControl(param, control);
                        break;
                }
            }
        }
        private void turbineControl(HSKShuttleStatus param, Vector4 control) {
            for (int i = 0; i < Turbines.Count; i++) {
                if (Turbines[i] != null) { Turbines[i].Control(param, control); }
            }
        }
        private void toggleControl(HSKShuttleStatus param)
        {
            bool begin = (param == HSKShuttleStatus.stBegin);
            updateColliders(begin);
            if (PersonController != null) { PersonController.SetActive(!begin); }
            _shuttleModel.isKinematic = _shuttleModel.useGravity = !begin;
            switchCameras(begin);
            turbineControl(param, Vector4.zero);
        }
        private void disableShuttleCameras()
        {
            for (int i = 0; i < Cameras.Count; i++) {
                if ((i != _currentCamera) && (Cameras[i] != null)) {
                    if (Cameras[i].camera != null) { Cameras[i].camera.enabled = false; }
                    if (Cameras[i].listener != null) { Cameras[i].listener.enabled = false; }
                }
            }
        }
        private void switchCameras(bool _isControlled)
        {
            if (_isControlled) {
                // Check if the hotkey to activate camera is pressed 
                int switchTo = -1;
                for (int i = 0; i < Cameras.Count; i++){
                    if ((Cameras[i] != null) && Input.GetKey(Cameras[i].hotKey)) { switchTo = i; break; }
                }
                if ((_currentCamera < 0) && (switchTo < 0)) { switchTo = 0; }
                // Switch to selected camera
                if (switchTo > -1) {
                    if (_currentCamera > -1) {
                        Cameras[_currentCamera].camera.enabled = false;
                        if (Cameras[_currentCamera].listener != null) { Cameras[_currentCamera].listener.enabled = false; }
                    }
                    Cameras[_currentCamera = switchTo].camera.enabled = true;
                    if (Cameras[_currentCamera].listener != null) { Cameras[_currentCamera].listener.enabled = true; }
                }
                // Disable main scene camera
                if ((_mainCamera != null) && _mainCamera.enabled) { _mainCamera.enabled = false; }
            } else {
                // Shuttle is inactive - switch to main scene camera
                if (_currentCamera > -1){
                    Cameras[_currentCamera].camera.enabled = false;
                    if (Cameras[_currentCamera].listener != null) { Cameras[_currentCamera].listener.enabled = false; }
                    _currentCamera = -1;
                }
                if (_mainCamera != null) { _mainCamera.enabled = true; }
            }

        }
        private void collectColliders()
        {
            if (_shuttleModel == null) { return; }
            cItems = new List<colliderItem>();
            fItems = new List<Collider>();
            Collider[] mcolliders = _shuttleModel.gameObject.GetComponentsInChildren<Collider>(true);
            if (mcolliders != null){
                foreach (Collider mc in mcolliders){
                    if ((mc.tag != "FlightCollider") || (mc is MeshCollider)) {
                        cItems.Add(new colliderItem { mCollider = mc, isEnable = mc.enabled });
                    } else {
                        mc.enabled = false;
                        fItems.Add(mc);
                    }
                }
            }
        }
        private void updateColliders(bool start)
        {
            if ((cItems != null) && (fItems != null)){
                // Inactive mode colliders
                for (int i = 0; i < cItems.Count; i++) {
                    if (start && cItems[i].mCollider.enabled != cItems[i].isEnable) {
                        cItems[i] = new colliderItem { mCollider = cItems[i].mCollider, isEnable = cItems[i].isEnable };
                    }
                    cItems[i].mCollider.enabled = (start) ? false : cItems[i].isEnable;
                }
                // Flight colliders
                for (int i = 0; i < fItems.Count; i++) {
                    fItems[i].enabled = start;
                }
            }
        }
        private void toggleChasis()
        {
            updateChasis(Chasis[0].mode == dotHskDoorMode.inactiveClosed);
        }
        private void updateChasis(bool open)
        {
            for (int i = 0; i < Chasis.Count; i++){
                if (Chasis[i] != null){
                    Chasis[i].mode = open ? dotHskDoorMode.inactiveOpen : dotHskDoorMode.inactiveClosed;
                }
            }
        }
    }
}