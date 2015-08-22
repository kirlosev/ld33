using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    public static Game instance;
    public LayerMask groundLayer = 1 << 8;
    public LayerMask worldObjectLayer = 1 << 9;
    public Monster monster;
    public float gravity = -9f;

    void Awake() {
        instance = this;
    }

    void Update() {
        // TODO : remove from release
        if (Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
