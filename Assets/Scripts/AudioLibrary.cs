using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour {

    private static AudioLibrary audioLibrary;
    public static AudioLibrary Instance {
        get {
            if (audioLibrary == null) audioLibrary = GameObject.Find("Audio Library").GetComponent<AudioLibrary>();
            return audioLibrary;
        }
    }

    [SerializeField]
    private AudioClip theme;
    public AudioClip GetTheme() {
        return theme;
    }

    [SerializeField]
    private AudioClip[] contactSounds;
    public AudioClip GetRandomContactSound() {
        int index = Random.Range(0, contactSounds.Length);
        return contactSounds[index];
    }

}
