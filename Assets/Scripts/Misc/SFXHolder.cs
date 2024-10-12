using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXHolder : MonoBehaviour
{
    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource audioSource;

    public void PlayAudioClip(int clipIndex){
        audioSource.PlayOneShot(audioClips[clipIndex]);
    }
}
