using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    private int currTrack;
    private int lastTrack;
    private bool startedPlayingOnce;
    public AudioClip[] songList;
    public AudioSource soundtrackSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(soundtrackSource.enabled)
        {
            if (!soundtrackSource.isPlaying)
            {
                tryAgain:
                currTrack = Random.Range(0, songList.Length);
                if (startedPlayingOnce && currTrack == lastTrack) { goto tryAgain; }
                startedPlayingOnce = true;
                lastTrack = currTrack;
                soundtrackSource.clip = songList[currTrack];
                soundtrackSource.Play();
            }
        }
    }
}
