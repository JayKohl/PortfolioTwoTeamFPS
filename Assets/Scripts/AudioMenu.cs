//using System;
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
    [SerializeField] Toggle mute;
    [SerializeField] List<AudioClip> sfxClips;
    [SerializeField] List<AudioClip> musicClips;
    [SerializeField] AudioSource musicTest;
    [SerializeField] AudioSource sfxTest;
    [SerializeField] Button musicbutton;
    [SerializeField] Button sfxbutton;
    [SerializeField] float volumeHold; 

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(HandleMusicSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(HandleSFXSliderValueChanged);
        masterSlider.onValueChanged.AddListener(HandleMasterSliderValueChanged);
        mute.onValueChanged.AddListener(HandleMuteChange);
        musicbutton.onClick.AddListener(TestMusic);
        sfxbutton.onClick.AddListener(TestSFX);
    }

    private void TestSFX()
    {
        sfxTest.PlayOneShot(sfxClips[Random.Range(0, sfxClips.Count)]);
    }

    private void TestMusic()
    {
        if (!musicTest.isPlaying)
        {
            StartCoroutine(PlayStop());
        }
    }

    private void HandleMuteChange(bool soundOn)
    {
        //if (DisableToggle)
        //{
        //    return;
        //}
        if (masterSlider.value > masterSlider.minValue)
        {
            volumeHold = masterSlider.value;
        }
        

        if (soundOn)
        {
            masterSlider.value = masterSlider.minValue;
        }
        else
        {
            masterSlider.value = volumeHold;
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(masterVolume, masterSlider.value);
        PlayerPrefs.SetFloat(musicVolume, musicSlider.value);
        PlayerPrefs.SetFloat(sfxVolume, sfxSlider.value);
       
    }

    private void HandleMusicSliderValueChanged(float value)
    {
        if(musicSlider.value != 0)
        {
            _mixer.SetFloat(musicVolume, value: Mathf.Log10(value) * multiplier);
        }
        else
        {
            _mixer.SetFloat(musicVolume, -80);
        }
    }

    private void HandleSFXSliderValueChanged(float value)
    {
        if (sfxSlider.value != 0)
        {
            _mixer.SetFloat(sfxVolume, value: Mathf.Log10(value) * multiplier);
        }
        else
        {
            _mixer.SetFloat(sfxVolume, -80);
        }
        
    }

    private void HandleMasterSliderValueChanged(float value)
    {
        

        if (masterSlider.value != 0)
        {
            _mixer.SetFloat(masterVolume, value: Mathf.Log10(value) * multiplier);
            mute.isOn = false;
            
        }
        else
        {
            mute.isOn = masterSlider.value == masterSlider.minValue;
            _mixer.SetFloat(masterVolume, -80);
        }
    }


    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(masterVolume, masterSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat(musicVolume, musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolume, sfxSlider.value);
    }

    IEnumerator PlayStop()
    {
        musicTest.PlayOneShot(musicClips[Random.Range(0, musicClips.Count)]);
        yield return new WaitForSecondsRealtime(2);
        musicTest.Stop();

    }


}
