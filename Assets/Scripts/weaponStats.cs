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
    //public AudioClip weaponAudio;
    [SerializeField] public List<AudioClip> weaponAudio = new List<AudioClip>();
    public Sprite weaponIcon;
    public Vector3 muzzleFlashPosition;
    public Sprite crosshairTexture;
    public float zoomAmount;
    public string weaponName;
}
