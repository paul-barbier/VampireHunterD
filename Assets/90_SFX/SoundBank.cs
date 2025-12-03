using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundBank : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip[] Sounds;
    }
    [Serializable]
    public class SoundGround
    {
        public string Name;
        public AudioClip[] Sounds;
    }

    [SerializeField] public List<Sound> soundList;
    [SerializeField] public List<SoundGround> soundGroundList;

    public AudioClip GetSound(string name)
    {
        Sound sound = soundList.Find(s => s.Name == name);
        if (sound == null)
            return null;

        int index = UnityEngine.Random.Range(0, sound.Sounds.Length);

        return sound.Sounds[index];
    }
    public AudioClip GetSoundGround(string name)
    {
        SoundGround sound = soundGroundList.Find(s => s.Name == name);
        if (sound == null)
            return null;

        int index = UnityEngine.Random.Range(0, sound.Sounds.Length);

        return sound.Sounds[index];
    }

}