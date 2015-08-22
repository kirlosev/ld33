﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    public static Game instance;
    public LayerMask groundLayer = 1 << 8;
    public LayerMask worldObjectLayer = 1 << 9;
    public LayerMask bloodLayer = 1 << 10;
    public LayerMask skyscrapperLayer = 1 << 11;
    public Monster monster;
    public float gravity = -9f;
    public Blood bloodInstance;
    public AudioSource sfxSound;

    void Awake() {
        instance = this;
    }

    void Update() {
        // TODO : remove from release
        if (Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void playSfx(AudioClip sfxClip) {
        var pitch = 0.9f + Random.Range(-0.5f, 0.5f);
        sfxSound.pitch = pitch;
        sfxSound.PlayOneShot(sfxClip);
    }
}
