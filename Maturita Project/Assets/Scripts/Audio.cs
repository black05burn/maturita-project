using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour {

    public static Audio instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos);
    }

}
