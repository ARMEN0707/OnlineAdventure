using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSound : MonoBehaviour
{
    public AudioSource[] music;
    public AudioSource[] sound;
    public Slider musicSlider;
    public Slider soundSlider;

    public void ChangedVolumeMusic(Slider slider)
    {
        DataScenes.volumeMusic = slider.value;
        foreach (AudioSource audio in music)
        {
            audio.volume = DataScenes.volumeMusic;
        }
    }
    public void ChangedVolumeSound(Slider slider)
    {
        DataScenes.volumeSound = slider.value;
        foreach (AudioSource audio in sound)
        {
            audio.volume = DataScenes.volumeSound;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(AudioSource audio in music)
        {
            audio.volume = DataScenes.volumeMusic; 
        }
        foreach (AudioSource audio in sound)
        {
            audio.volume = DataScenes.volumeSound;
        }
        musicSlider.value = DataScenes.volumeMusic;
        soundSlider.value = DataScenes.volumeSound;
    }
}
