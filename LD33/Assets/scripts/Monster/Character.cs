using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    public bool isAlive = true;
    public float health = 1f;
    public float maxHealth = 1f;
    public Vector3 size;

    public void Awake() {
        size = GetComponent<BoxCollider2D>().bounds.extents;
    }

    public virtual void destroy() {
        isAlive = false;
    }

    public virtual void damage(float value) {
        health -= value;
        if (health <= 0) {
            destroy();
        }
    }
}
