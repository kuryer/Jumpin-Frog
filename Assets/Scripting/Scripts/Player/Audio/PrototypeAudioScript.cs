using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeAudioScript : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] bool playAudio;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> sounds;

    [Header("Steps")]
    [SerializeField] AudioSource stepStource;
    [SerializeField] List<AudioClip> steps;
    [SerializeField] bool pitchMix;
    [SerializeField] Vector2 PitchRange;

    public void PlaySound(int index)
    {
        if (!playAudio || index < 0 || index > sounds.Count - 1)
            return;

        audioSource.PlayOneShot(sounds[index]);
    }

    public void PlayStep()
    {
        if (!playAudio)
            return;
        int index = Random.Range(0, steps.Count - 1);
        float pitch = Random.Range(PitchRange.x, PitchRange.y);
        if(pitchMix)
            stepStource.pitch = pitch;
        stepStource.PlayOneShot(steps[index]);
    }
}
