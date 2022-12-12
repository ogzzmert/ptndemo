using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Handler, IHandlerGenerator
{
    [field: SerializeField, Range(0f, 1f)] private float musicVolume;
    [field: SerializeField, Range(0f, 1f)] private float soundVolume;
    [field: SerializeField] private AudioSource musicSource;
    [field: SerializeField] private AudioSource soundSource;

    Dictionary<string, AudioClip> musics = new Dictionary<string, AudioClip>();
    Dictionary<int, AudioClip> sounds = new Dictionary<int, AudioClip>();

    protected override void initialize()
    {
        changeMusicVolume(musicVolume);
        changeSoundVolume(soundVolume);

        foreach(WorldType wt in Enum.GetValues(typeof(WorldType)))
        {
            int i = 0;

            while(true)
            {
                string clipname = wt.ToString() + "_" + i.ToString();
                AudioClip clip = ResourceManager.load<AudioClip>("Audio/Music/" + clipname);
                if (clip != null)
                {
                    musics.Add(clipname, clip);
                    i++;
                }
                else break;
            }
        }

        int e = 0;

        while(true)
        {
            AudioClip clip = ResourceManager.load<AudioClip>("Audio/Sound/" + e.ToString());
            if (clip != null)
            {
                sounds.Add(e, clip);
                e++;
            }
            else break;
        }
    }
    public void generate(WorldType worldType, int worldIndex)
    {
        playMusic(worldType.ToString() + "_" + worldIndex.ToString());
    }
    public void changeMusicVolume(float volume)
    {
        volume = volume > 1f || volume < 0f ? 1f : volume;
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }
    public void changeSoundVolume(float volume)
    {
        volume = volume > 1f || volume < 0f ? 1f : volume;
        soundVolume = volume;
        soundSource.volume = soundVolume;
    }
    public void playMusic(string musicName)
    {
        if (musics.ContainsKey(musicName))
        { 
            musicSource.clip = musics[musicName]; 
            musicSource.Play();
        }
        else musicSource.Stop();
    }
    public void playSound(int soundNo)
    {
        if (sounds.ContainsKey(soundNo))
        { 
            soundSource.clip = sounds[soundNo]; 
            soundSource.Play();
        }
    }
    public void playSoundButtonA() { playSound(0); }
    public void playSoundButtonB() { playSound(1); }
    public void playSoundActionA() { playSound(2); }
    public void playSoundActionB() { playSound(3); }

}
