using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    AudioSource BGM;
    AudioSource SFX;

    [SerializeField]
    Slider BGMVol_Slider;
    [SerializeField]
    Slider SFXVol_Slider;

    // Use this for initialization
    //New audio manager to take care of bgm and sfx
    void Start()
    {
        BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();

        if (BGMVol_Slider == null)
            BGMVol_Slider = GameObject.FindGameObjectWithTag("BGM_SLIDER").GetComponent<Slider>();

        if (SFXVol_Slider == null)
            SFXVol_Slider = GameObject.FindGameObjectWithTag("SFX_SLIDER").GetComponent<Slider>();

        if (BGMVol_Slider != null)
            BGMVol_Slider.value = PlayerPrefs.GetFloat("BGM_Volume");
        if (SFXVol_Slider != null)
            SFXVol_Slider.value = PlayerPrefs.GetFloat("SFX_Volume");
        if (PlayerPrefs.HasKey("BGM_Volume"))
        {
            BGM.volume = PlayerPrefs.GetFloat("BGM_Volume");
        }
        else
        {
            BGM.volume = 1;
            PlayerPrefs.SetFloat("BGM_Volume", BGM.volume);
        }
        if (PlayerPrefs.HasKey("SFX_Volume"))
        {
            SFX.volume = PlayerPrefs.GetFloat("SFX_Volume");
        }
        else
        {
            SFX.volume = 1;
            PlayerPrefs.SetFloat("SFX_Volume", BGM.volume);
        }
        BGMVol_Slider.value = PlayerPrefs.GetFloat("BGM_Volume");
        SFXVol_Slider.value = PlayerPrefs.GetFloat("SFX_Volume");
    }

    public void PlaySFX(AudioClip audio)
    {
        SFX.PlayOneShot(audio);
    }
    public void UpdateBGMVolume()
    {
        PlayerPrefs.SetFloat("BGM_Volume",BGMVol_Slider.value);
        BGMVol_Slider.value = PlayerPrefs.GetFloat("BGM_Volume");
        BGM.volume = PlayerPrefs.GetFloat("BGM_Volume");
    }
    public void UpdateSFXVolume()
    {
        PlayerPrefs.SetFloat("SFX_Volume", SFXVol_Slider.value);
        SFX.volume = PlayerPrefs.GetFloat("SFX_Volume");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
