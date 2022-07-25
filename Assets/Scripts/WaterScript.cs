using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public AudioSource[] audioSources;
    int audioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            audioSources[audioIndex].pitch = Random.Range(.7f, 1.3f);
            audioSources[audioIndex].time = 0;
            audioSources[audioIndex].Play();
            audioIndex++;
            audioIndex %= audioSources.Length;
        }
    }
}
