using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour {
    public WorldObject worldObject;
    public float moveSpeed = 4f;
    Character target;
    Vector3 velocity;

    void Start() {
        target = Game.instance.monster;
    }

    void Update() {
        if (!worldObject.isAlive) {
            return;
        }
        
        var targetDir = target.transform.position - transform.position;
        if (targetDir.magnitude < 2f) {
            velocity = -targetDir.normalized;
        }
        else if (targetDir.magnitude > 5f) {
            velocity = targetDir.normalized;
        }
        else {
            velocity = Vector3.up * Mathf.Sin(Time.time);
        }

        if (Physics2D.Raycast(transform.position, -Vector3.up, worldObject.size.y, Game.instance.groundLayer)) {
            velocity = Vector3.up;
        }

        transform.position += velocity * moveSpeed * Time.deltaTime;
    }
}
