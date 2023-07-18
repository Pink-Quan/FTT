using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sounds", fileName = "SceneName")]
public class SoundInfo : ScriptableObject
{
    [SerializeField] private List<GameSound> soundsList;

    [Serializable]
    public struct GameSound
    {
        public string name;
        public AudioClip sound;
    }

    public Dictionary<string, AudioClip> sounds;

    private void OnEnable()
    {
        sounds = new Dictionary<string, AudioClip>();
        foreach (var sound in soundsList)
            sounds.Add(sound.name, sound.sound);
    }
}
