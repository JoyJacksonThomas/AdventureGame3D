using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private void Start()
    {
        AudioSource explodeSound = GameObject.Find("ExplosionAudio").GetComponent<AudioSource>();
        if (!explodeSound.isPlaying)
            explodeSound.Play();
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
