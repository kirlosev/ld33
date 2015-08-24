using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    public static Game instance;
    public LayerMask groundLayer = 1 << 8;
    public LayerMask worldObjectLayer = 1 << 9;
    public LayerMask bloodLayer = 1 << 10;
    public LayerMask skyscraperLayer = 1 << 11;
    public LayerMask projectilesLayer = 1 << 12;
    public LayerMask playerLayer = 1 << 13;
    public Monster monster;
    public float gravity = -9f;
    public AudioSource sfxSound;
    public CameraMove gameplayCam;
    public Transform leftBottomCorner, rightTopCorner;
    public float startGameTime;
    
    void Awake() {
        instance = this;
    }

    void Start() {
        startGameTime = Time.time;
    }

    public float getTimerValue() {
        return Time.time - startGameTime;
    }
    
    public void playSfx(AudioClip sfxClip) {
        var pitch = 0.9f + Random.Range(-0.5f, 0.5f);
        sfxSound.pitch = pitch;
        sfxSound.PlayOneShot(sfxClip);
    }
}
