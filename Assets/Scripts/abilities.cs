using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu]

public class abilities : ScriptableObject
{
    public float cooldownTime;
    public Sprite abilityImage;
    public AudioClip abilityAudio;
    public float abilityAudioVol;
    public string abilityName;
    public Sprite abilityInfo;
}
