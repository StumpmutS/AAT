using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    private bool _destroyed;

    private void Start()
    {
        StartCoroutine(PlayAudio());
    }

    private IEnumerator PlayAudio()
    {
        int currentIndex = 0;
        
        while (!_destroyed)
        {
            audioSource.clip = audioClips[currentIndex];
            audioSource.Play();
            yield return new WaitForSeconds(audioClips[currentIndex].length);
            currentIndex++;
            if (currentIndex >= audioClips.Count) currentIndex = 0;
        }
    }

    private void OnDestroy()
    {
        _destroyed = true;
    }
}
