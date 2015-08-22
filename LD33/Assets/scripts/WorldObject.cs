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
        if (GetComponent<BoxCollider2D>() != null) 
            size = GetComponent<BoxCollider2D>().bounds.extents;
        else
            size = GetComponent<CircleCollider2D>().bounds.extents;
    }

    void Update() {
        if (!isThrown) {
            calcVelocity();
        }

        transform.position += velocity * Time.deltaTime;
        if (isThrown)
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
