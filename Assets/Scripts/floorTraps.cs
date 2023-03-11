using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class floorTraps : ScriptableObject
{
    public MeshRenderer brokenBarrel;
    public ParticleSystem effect;
    public AudioSource soundEffect;
    public float activationTime;
    public int health;
    public bool wasHit;
}
