using UnityEngine;
using System.Collections;

public class Sounder : MonoBehaviour {
    public AudioSource sfxSound;

    public void playSound(AudioClip clip) {
        var pitch = 0.9f + Random.Range(-0.5f, 0.5f);
        sfxSound.pitch = pitch;
        sfxSound.PlayOneShot(clip);
    }
}
