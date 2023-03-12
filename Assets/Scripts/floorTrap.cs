using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class floorTrap : ScriptableObject
{
	public ParticleSystem trapVisual;
	public AudioClip soundEffect;
	public int activeTime;
	public int effectTime;
	public int damage;
	public bool trapActive;
	public GameObject damageArea;

	//Effecrt Type
	// 1 = poison
	// 2 = electrecuted
	// 3 = burning
	// 4 = slow
	public int effectType;
}
