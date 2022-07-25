using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAudioScript : MonoBehaviour
{
    public AudioSource[] RattleSounds;
    public AudioSource OpenSound;
    int _rattleIndex = 0;
    
    public void PlayChestRattle()
    {
        RattleSounds[_rattleIndex].time = 0;
        RattleSounds[_rattleIndex].Play();
        _rattleIndex++;
        _rattleIndex %= RattleSounds.Length;
    }

    public void PlayChestOpen()
    {
        OpenSound.time = 0;
        
        OpenSound.Play();
    }
}
