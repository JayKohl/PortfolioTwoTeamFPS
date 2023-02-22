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
    public enum DotHskTurbinePlace { @static, left, right };
    public enum DotHskTurbineLocation { middle, front, rear };
    public class DotHskShuttleTurbine : MonoBehaviour
    {
        public Transform Item;
        public Renderer NozzleRenderer;
        public AudioSource SoundSource;
        public AudioClip StartSound;
        public AudioClip MoveSound;
        public AudioClip StopSound;
        [Tooltip("Turbine start-up time, used if PlaySounds is set to false or AudioClip StartSound is not specified")]
        public float StartingTime = 5f;
        [Tooltip("Turbine stop time, used if PlaySounds is set to false or AudioClip StopSound is not specified")]
        public float StopingTime = 5f;
        public Light NozzleLight;
        public DotHskTurbinePlace Place;
        public DotHskTurbineLocation Location;
        public bool PlaySounds = true;
        public float BackwardAngle = -150f;
        public float ForwardAngle = -30f;
        public float VerticalAngle = -90f;
        public float IddleAngle = 0f;
        public float Responsivity = 5f;

        private float state = 0f; // Engine state: [0..1], where 0 - Off, 1 - On
        private int dir = 0; // Direction: -1 - engine shutdown, 0 -  state of engine is not changed, 1 - engine starting 
        private float soundLength = 0f;
        private bool turbineHumPlaying = false;
        private bool parkTurbine = false;
        private Quaternion iddleRotation = Quaternion.identity;

        public void Control(HSKShuttleStatus param, Vector4 action)
        {
            switch( param ){
                case HSKShuttleStatus.stStart: // Start engine
                    if (state < 1) {
                        dir = 1;
                        parkTurbine = false;
                        PlayStartStopSound(StartSound, StartingTime);
                    }
                    break;
                case HSKShuttleStatus.stStop: // Stop engine
                    if (state > 0) {
                        dir = -1;
                        parkTurbine = true;
                        PlayStartStopSound(StopSound, StopingTime);
                    }
                    break;
                case HSKShuttleStatus.stControl: // Control
                    if ((Place == DotHskTurbinePlace.@static) || (Item==null)) { return; }
                    float _angle = VerticalAngle;
                    if (action.x != 0) {
                        // Turn
                        _angle = (action.x > 0) ^ (Place == DotHskTurbinePlace.left) ? (((action.y != 0)||(action.w != 0)) ? VerticalAngle : BackwardAngle) : ForwardAngle;
                    } else {
                        // No turn
                        if ( (action.y != 0) || ((action.w != 0) && (Location != DotHskTurbineLocation.middle)) ){
                            // Motion in Forward/Backward direction or turbine is Front/Rear
                            if (action.w == 0) {
                                // No strafe/no turn - direct motion
                                _angle = (action.y > 0) ? ForwardAngle : BackwardAngle;
                            } else {
                                if (action.y == 0) {
                                    _angle = (
                                        (Place == DotHskTurbinePlace.right) && (Location == DotHskTurbineLocation.rear) ||
                                        (Place == DotHskTurbinePlace.left) && (Location == DotHskTurbineLocation.front)
                                    ) ^ (action.w < 0) 
                                    ? ForwardAngle : BackwardAngle;
                                } else {
                                    if (action.y > 0) {
                                        _angle = (Place == DotHskTurbinePlace.right) && (Location == DotHskTurbineLocation.front) && (action.w < 0) ||
                                            (Place == DotHskTurbinePlace.left) && (Location == DotHskTurbineLocation.rear) && (action.w > 0) 
                                        ? BackwardAngle : ForwardAngle;
                                    } else {
                                        _angle = (Place == DotHskTurbinePlace.right) && (Location == DotHskTurbineLocation.rear) && (action.w < 0) ||
                                            (Place == DotHskTurbinePlace.left) && (Location == DotHskTurbineLocation.front) && (action.w > 0)
                                        ? ForwardAngle : BackwardAngle;
                                    }
                                }
                            }

                        }
                    }
                    Item.localRotation = Quaternion.Slerp(Item.localRotation, Quaternion.Euler(_angle, 0, 0), Time.deltaTime * Responsivity / 16 );
                    break;
            }
        }
        private void Start()
        {
            if (NozzleRenderer != null) {
                NozzleRenderer.material.EnableKeyword("_EMISSION");
                NozzleRenderer.material.SetColor("_EmissionColor", Color.black);
            }
            if (NozzleLight != null) { NozzleLight.intensity = 0f; }
            if ((Place != DotHskTurbinePlace.@static) && (Item != null)) {
                iddleRotation = Item.localRotation = Quaternion.Euler(IddleAngle, 0, 0);
            }
        }

        private void Update()
        {
            if (dir == 0) {
                if (MoveSound != null) { PlayTurbineHum(); }
            } else {
                if (soundLength > 0) {
                    state += dir * Time.deltaTime / soundLength;
                    if (state <= 0) {
                        state = 0f; dir = 0;
                    } else {
                        if (state >= 1) {
                            state = 1f; dir = 0;
                        }
                    }
                }
                if (NozzleRenderer != null) { NozzleRenderer.material.SetColor("_EmissionColor", Color.white * (state * 3.5f)); }
                if (NozzleLight != null) { NozzleLight.intensity = 8 * state; }
            }
            if (parkTurbine && (Item != null) && (Place != DotHskTurbinePlace.@static)) {
                Item.localRotation = Quaternion.Slerp(Item.localRotation, iddleRotation, Time.deltaTime);
            }
        }
        private void PlayStartStopSound(AudioClip clip, float duration) {
            turbineHumPlaying = false;
            if ( !PlaySounds || (SoundSource == null) || (clip == null) ) {
                soundLength = duration;
            } else {
                if (SoundSource.isPlaying) { SoundSource.Stop(); }
                SoundSource.clip = clip;
                soundLength = clip.length;
                SoundSource.loop = false;
                SoundSource.Play();
            }
        }
        private void PlayTurbineHum() {
            if (turbineHumPlaying || (state == 0f)) { return; }
            turbineHumPlaying = true;
            if ( PlaySounds && (SoundSource != null) && (MoveSound != null) ) {
                if (SoundSource.isPlaying) { SoundSource.Stop(); }
                SoundSource.clip = MoveSound;
                SoundSource.loop = true;
                SoundSource.Play();
            }
        }
    }
}