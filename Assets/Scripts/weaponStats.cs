using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class weaponStats : ScriptableObject
{
    public float shootRate;
    public int shootDist;
    public int shootDamage;
    public GameObject weaponModel;
    public AudioClip weaponAudio;
}
