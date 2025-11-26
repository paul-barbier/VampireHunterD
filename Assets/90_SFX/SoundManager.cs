using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum SoundType
{
    JacketSound,
    WoodFootsteps,
    TilesFootsteps,
    CataFootsteps,
    MobFootsteps,
    Attack,
    Land,
    Vampire,
    Ambiance_Clocher,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{

    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audiosource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this; 
    }

    // Update is called once per frame
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audiosource.PlayOneShot(randomclip, volume);
        //instance.audiosource.PlayOneShot(instance.soundList[(int)sound], volume);
        
    }

    public static void LoopSound(SoundType sound, float volume = 1)
    {
       
    }

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].Name = names[i];
        }
#endif
    }
}


    [Serializable]
    public struct SoundList
    {
        public AudioClip[] Sounds { get => sounds; }
        [SerializeField] public string Name;
        [SerializeField] private AudioClip[] sounds; 
    }
