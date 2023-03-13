using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] string masterVolume = "MasterVolume";
    [SerializeField] string musicVolume = "MusicVolume";
    [SerializeField] string sfxVolume = "SFXVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
