using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;

    private SoundInfo commonSounds;

    private void Awake()
    {
        commonSounds = Resources.Load<SoundInfo>("Sounds/Common");
    }

    public void PlayCommondSound(string soundName)
    {
        sfx.PlayOneShot(commonSounds.sounds[soundName]);
    }

    private SceneSoundInfo currentSceneSound;
    public void PlaySound(string soundName)
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if(activeSceneName != currentSceneSound.sceneName)
        {
            currentSceneSound.sceneName = activeSceneName;
            currentSceneSound.sounds = Resources.Load<SoundInfo>("Sounds/"+ activeSceneName);
        }
        sfx.PlayOneShot(currentSceneSound.sounds.sounds[soundName]);
    }
    public void PlayMusic(string musicName)
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName != currentSceneSound.sceneName)
        {
            currentSceneSound.sceneName = activeSceneName;
            currentSceneSound.sounds = Resources.Load<SoundInfo>("Sounds/" + activeSceneName);
        }
        music.clip = currentSceneSound.sounds.sounds[musicName];
        music.Play();
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void PlaySound(AudioClip sound)
    {
        sfx.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        this.music.clip = music;
        this.music.Play();
    }

    private struct SceneSoundInfo
    {
        public string sceneName;
        public SoundInfo sounds;
    }

    public SoundInfo Sounds=>currentSceneSound.sounds;
    public SoundInfo CommonSounds => commonSounds;
}
