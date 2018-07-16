using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SoundManager : Singleton<SoundManager> {

    public AudioSource BGMSource;
    private AudioSource FXSource;
    public SoundSetting SoundSet;
    public Settings Set;

	// Use this for initialization
	public void Start ()
    {
        SoundSet = new SoundSetting();
        if (File.Exists(Application.persistentDataPath + "/SoundSettings"))
            Load();

        Set = (Settings)FindObjectOfType(typeof(Settings));

        //If BGM source is set, play it
        if (BGMSource)
            PlayBGM();
        else
            BGMSource = new AudioSource();
	}
	
	// Update is called once per frame
	void Update () {
     //   SetBGMVolume(Set.GetBGMValue());
        //SoundSet.VolumeBGM = Set.GetBGMValue();
        //SetBGMVol(SoundSet.VolumeBGM);
        //gameObject.GetComponent<AudioSource>().volume = Set.GetBGMValue();
	}

    public void PlayBGM()
    {
        if (BGMSource == null)
            return;
        //Range (0f - 1f)
        if (SoundSet == null)
            Debug.Log("We have no Sound Settings ");
        BGMSource.volume = SoundSet.VolumeBGM;
        BGMSource.Play();
    }
    public void PlayFX(AudioSource FXtoPlay)
    {
        FXtoPlay.Play();
    }
    public void Save()
    {
        //convert data to binary for storing
        BinaryFormatter bf = new BinaryFormatter();
        //Open path to file
        FileStream file = File.Create(Application.persistentDataPath + "/SoundSettings");
        bf.Serialize(file, SoundSet);
        file.Close();
    }
    public void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/SoundSettings"))
        {
            //convert binary to data
            BinaryFormatter bf = new BinaryFormatter();
            //Path from file
            FileStream file = File.Open(Application.persistentDataPath + "/SoundSettings", FileMode.Open);
            SoundSet = (SoundSetting)bf.Deserialize(file);
            file.Close();
        }
    }

    //SET BGM
    public void SetBGM(AudioSource BGM_S)
    {
        BGMSource = BGM_S;
    }

    public float GetBGMVol()
    {
        return SoundSet.VolumeBGM;
    }


    //Set Volume
    public void SetBGMVolume(float Volume)
    {
        if (BGMSource == null)
            return;
        BGMSource.volume = Volume;
    }

}

//Save
[System.Serializable]
public class SoundSetting
{
    public float VolumeBGM;
    public float VolumeFX;


}