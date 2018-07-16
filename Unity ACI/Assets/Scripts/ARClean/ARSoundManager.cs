/* 
 * Author: Lim Rui An Ryan
 * Filename: ARSoundManager.cs
 * Description: The ARClean game mode's sound system that has a fade in/out effect and plays through the scenes in the gamemode without cutoffs when switching scenes.
 */
using UnityEngine;
using System.Collections.Generic;

public class ARSoundManager : MonoBehaviour
{
    public List<AudioClip> SoundList;
	public float FadeRate = 5f;

    public bool PlaySoundOnAwake = false;
    public int SoundIndexToPlayOnAwake = 0;

	private AudioSource InternalAudio;
	private GameObject OtherMusicPlayer;
	private int SongToPlayNext = -1;
	private bool FadeInRequired = false;
	private bool FadeOutRequired = false;
	private float OriginalVolume;
     
	void Awake()
	{
		// Check if another music player entity exists in the scene
		OtherMusicPlayer = GameObject.Find(ARCleanDataStore.SoundSystemName);
		if (OtherMusicPlayer == null){
            gameObject.name = ARCleanDataStore.SoundSystemName;
            InternalAudio = gameObject.GetComponent<AudioSource>();
			OriginalVolume = InternalAudio.volume;
			OtherMusicPlayer = gameObject;
			DontDestroyOnLoad(gameObject);
            SongToPlayNext = SoundIndexToPlayOnAwake;
            if (PlaySoundOnAwake && SoundList[SoundIndexToPlayOnAwake] != null)
                PlayAudio(SoundIndexToPlayOnAwake);
        }
		else Destroy(gameObject); // Another music player already exists, and is probably playing something. This object is unneeded.
	}

	void Update()
	{
		if (SongToPlayNext != -1 && SoundList[SongToPlayNext] != null){
			if (FadeOutRequired){
				if (InternalAudio.volume > 0f)
					InternalAudio.volume -= Time.deltaTime * FadeRate;
				else{
					FadeOutRequired = false;
					FadeInRequired = true;
					InternalAudio.volume = 0f;
					InternalAudio.Stop();
				}
			}
			else if (FadeInRequired){
				if (!InternalAudio.isPlaying){
				    InternalAudio.clip = SoundList[SongToPlayNext];
					InternalAudio.Play();
				}
				if (InternalAudio.volume < OriginalVolume)
					InternalAudio.volume += Time.deltaTime * FadeRate;
				else{
					InternalAudio.volume = OriginalVolume;
					FadeInRequired = false;
					SongToPlayNext = -1;
				}
			}
		}
		if (!FadeOutRequired && !FadeInRequired)
			InternalAudio.volume = OriginalVolume;
	}

	public void PlayAudio(int Index)
	{
		SongToPlayNext = Index;
		FadeOutRequired = InternalAudio.isPlaying;
		// Cater for fade in
		FadeInRequired = !InternalAudio.isPlaying;
		if (!InternalAudio.isPlaying)
			InternalAudio.volume = 0f;
	}
}