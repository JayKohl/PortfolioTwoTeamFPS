using System;
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
    [SerializeField] Slider masterSlider;
    [SerializeField] float multiplier = 20.0f;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(HandleMusicSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(HandleSFXSliderValueChanged);
        masterSlider.onValueChanged.AddListener(HandleMasterSliderValueChanged);
    }

    private void HandleMusicSliderValueChanged(float value)
    {
        _mixer.SetFloat(musicVolume, value: Mathf.Log10(value) * multiplier);
    }

    private void HandleSFXSliderValueChanged(float value)
    {
        throw new NotImplementedException();
    }

    private void HandleMasterSliderValueChanged(float value)
    {
        throw new NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
