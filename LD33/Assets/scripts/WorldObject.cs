using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {
    public bool isAlive = true;
    public bool canThrow = false;
    public float health = 1f;
    public float maxHealth = 1f;
    public Vector3 size;
    Vector3 velocity;
    bool isThrown = false;

    void Awake() {
        size = GetComponent<BoxCollider2D>().bounds.extents;
    }

    void Update() {
        if (!isThrown) {
            calcVelocity();
        }

        transform.position += velocity * Time.deltaTime;
        velocity.y += Game.instance.gravity * Time.deltaTime;
    }

    public virtual void calcVelocity() {
    }

    public void damage(float value) {
        health -= value;
        if (health <= 0) {
            destroy();
        }
    }

    public void destroy() {
        isAlive = false;
    }

    public void throwObject(Vector3 dir, float force) {
        isThrown = true;
        velocity = dir.normalized * force;
    }
}
