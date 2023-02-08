// ---------------------------------------------
// Sci-Fi Heavy Station Kit 
// Copyright (c) DotTeam. All Rights Reserved.
// https://dotteam.xyz, info@dotteam.xyz
// ---------------------------------------------

using UnityEngine;

namespace DotTeam.HSK
{

    public class DotHskGateHangarsParts : DotHskGatePartEventsClass
    {

        public AudioSource moveAudio;
        public AudioSource tickAudio;
        [Space]
        public AudioClip startSound;
        public AudioClip moveSound;
        public AudioClip stopSound;

        private bool playStart = false;
        private bool playMove = false;
        private bool playStop = false;
        private bool playTick = false;
        private float stopSoundStart = -1f;
        private float stopSoundOverlap = 0.4f;
        private int phase = 0; // 0 - silent, 1 - start, 2 - move, 3 - stop

        public override void OnInit()
        {
            if (moveAudio != null)
            {
                playStart = (startSound != null);
                playMove = (moveSound != null);
                playStop = (stopSound != null);
            }
            playTick = (tickAudio != null) && (tickAudio.clip != null);
        }

        // Motion Event: -1 - start motion, -2 - stop motion, 1 - start general motion, 2 - start tuck motion
        public override void OnMotion(int motionEvent)
        {
            if (motionEvent < 0)
            {
                // Start/Stop
                if (playMove)
                {
                    if (motionEvent == -1)
                    { // Start motion
                        if (playStart)
                        {
                            // Play "Start sound"
                            playSnd(startSound, false);
                            // Calculate starting time of "Stop Sound"
                            if (playStop)
                            {
                                stopSoundStart = Time.time + part.motionTime * Mathf.Abs(part.targetState - part.currentState) - (stopSound.length - stopSoundOverlap);
                            }
                            else
                            {
                                stopSoundStart = -1f;
                            }
                            phase = 1;
                        }
                        else
                        {
                            // Repeatedly play "Move sound"
                            playSnd(moveSound, true);
                            phase = 2;
                        }
                    }
                    else
                    { // Stop motion
                      // Break "Move sound", if "Stop sound" not exist
                        if (phase == 2)
                        {
                            moveAudio.Stop();
                            phase = 0;
                        }
                    }
                }
            }
            else
            {
                // Play "Tick Sound" during motion
                if (playTick)
                {
                    tickAudio.time = 0f;
                    tickAudio.Play();
                }
            }
        }

        void Update()
        {
            if (moveAudio.isPlaying)
            {
                // Play "Stop sound"
                if ((phase == 2) && playStop && (stopSoundStart > 0) && (Time.time >= stopSoundStart))
                {
                    moveAudio.Stop();
                    playSnd(stopSound, false);
                    phase = 3;
                }
            }
            else
            {
                // Start play "Move sound" after "Start sound"
                if (phase == 1)
                {
                    playSnd(moveSound, true);
                    phase = 2;
                }
                // End of plaing "Stop sound"
                if (phase == 3)
                {
                    phase = 0;
                }
            };
        }

        private void playSnd(AudioClip snd, bool loop)
        {
            moveAudio.clip = snd;
            moveAudio.time = 0f;
            moveAudio.loop = loop;
            moveAudio.Play();
        }

    }

}