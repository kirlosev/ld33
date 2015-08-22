using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {
    public Vector3 size;
    Vector3 velocity;
    RaycastHit2D hit;

    void Awake() {
        size = gameObject.GetComponent<Collider2D>().bounds.extents;
    }

    void Update() {
        if (checkGround()) {
            var reflectedDir = 0.38f * (velocity - 2 * Vector3.Dot(velocity, hit.normal) * (Vector3)hit.normal);
            if (reflectedDir.magnitude < 0.1f) {
                velocity = Vector3.zero;
            }
            else velocity = reflectedDir;
        }
        transform.position += velocity * Time.deltaTime;
        velocity.y += Game.instance.gravity * Time.deltaTime;
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.groundLayer);
        return hit;
    }

    public void init(Vector3 dir, float force) {
        velocity = dir * force;
    }

    public void destroy() {
        gameObject.SetActive(false);
    }
}
