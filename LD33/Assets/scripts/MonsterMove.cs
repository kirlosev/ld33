using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {
    public Monster monster;

    public bool onGround = false;
    Vector3 velocity;
    RaycastHit2D hit;

    WorldObject sittingOnObject;

    void Update() {
        if (monster.input.jump && onGround) {
            var dir = (monster.input.clickPos - transform.position).normalized;
            if (Vector3.Dot(dir, hit.normal) > 0)
                velocity = dir * monster.jumpForce;
        }

        if (checkGround()) {
            if (!onGround) {
                velocity = Vector3.zero;
                onGround = true;
                //transform.position = hit.point + hit.normal * monster.size.y; // TODO : need better
                var angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                sittingOnObject = hit.collider.GetComponent<WorldObject>();
            }
        }
        else onGround = false;

        if (onGround && sittingOnObject != null) {
            if (sittingOnObject.canThrow && monster.input.throwObject) {
                var dir = (monster.input.clickPos - transform.position).normalized;
                velocity = -dir * monster.jumpForce;
                sittingOnObject.throwObject(dir, monster.throwForce);
            }
        }

        transform.position += velocity * Time.deltaTime;
        if (!onGround)
            velocity.y += Game.instance.gravity * Time.deltaTime;

        if (velocity.magnitude > 0 && !onGround) {
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, monster.size.x, Game.instance.groundLayer);
        return hit;
    }
}
