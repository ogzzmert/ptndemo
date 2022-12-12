using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private AudioSource musicSource;
    [field: SerializeField] private AudioSource soundSource;

    protected override void initialize()
    {
        
    }
    public void generate(WorldType worldType, int worldIndex)
    {
        
    }
    public void playMusic(string musicName)
    {
        
    }
}
