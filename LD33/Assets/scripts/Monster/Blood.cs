using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {
    public Vector3 size;
    Vector3 velocity;
    RaycastHit2D hit;
    public bool isMoving = true;
    public Transform bloodDecal;

    void Awake() {
        size = gameObject.GetComponent<Collider2D>().bounds.extents;
    }

    void Update() {
        if (!isMoving) return;
        if (checkGround()) {
            var angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
            if (angle % 45 == 0) {
                Instantiate(bloodDecal, hit.point, Quaternion.AngleAxis(angle, Vector3.forward));
            }
            var reflectedDir = 0.38f * (velocity - 2 * Vector3.Dot(velocity, hit.normal) * (Vector3)hit.normal);
            if (reflectedDir.magnitude < 0.1f) {
                velocity = Vector3.zero;
                isMoving = false;
                GetComponent<Blood>().enabled = false;
            }
            else velocity = reflectedDir;
        }
        transform.position += velocity * Time.deltaTime;
        velocity.y += Game.instance.gravity * Time.deltaTime;
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.groundLayer | Game.instance.skyscraperLayer);
        return hit;
    }

    public void init(Vector3 pos, Vector3 dir, float force) {
        transform.position = pos;
        velocity = dir * force;
        isMoving = true;
    }

    public void destroy() {
        gameObject.SetActive(false);
    }
}
