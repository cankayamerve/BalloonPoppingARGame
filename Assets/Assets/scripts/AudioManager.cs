using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Provides the sound of the arrow colliding with objects
public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  
    public AudioClip shootSound;   
    public AudioClip errorSound;   
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayBalloonShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    public void PlayErrorSound()
    {
        if (audioSource != null && errorSound != null)
        {
            audioSource.PlayOneShot(errorSound);
        }
    }
}
