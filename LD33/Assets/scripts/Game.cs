using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    public static Game instance;
    public LayerMask groundLayer = 1 << 8;
    public Monster monster;
    public float gravity = -9f;

    void Awake() {
        instance = this;
    }
}
