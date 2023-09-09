using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip footstep;

    public void PlayFootStepSound()
    {
        audioSource.PlayOneShot(footstep);
    }

    public AudioSource AudioSource { get { return audioSource; } }
}
