using UnityEngine;
using System.Collections.Generic;
public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;

    void Awake()
    {
        instance = this;
    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip == null) return;
        effectSource.PlayOneShot(clip);
    }

    public void PlayRandomAudio(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0) return;
        PlayAudio(clips[Random.Range(0, clips.Count)]);
    }
}
