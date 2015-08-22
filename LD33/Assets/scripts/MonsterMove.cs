using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {
    public Monster monster;

    public bool onGround = false;
    public bool onSkyscraper = false;
    Vector3 velocity;
    Vector3 targetPoint;
    WorldObject targetSkyscraper;
    WorldObject currentSkyscraper;
    RaycastHit2D hit;
    float minTargetDistance;
    WorldObject sittingOnObject;

    void Update() {
        if (monster.input.jump && (onGround || onSkyscraper)) {
            var dir = (monster.input.clickPos - transform.position).normalized;

            velocity = dir * monster.jumpForce;
            targetPoint = monster.input.clickPos;
            minTargetDistance = Mathf.Infinity;
            var col = Physics2D.OverlapCircle(monster.input.clickPos, 0.1f);
            if (col != null) {
                targetSkyscraper = col.GetComponent<WorldObject>();
            }
            onSkyscraper = false;
            /*
            if (Vector3.Dot(dir, hit.normal) > 0) {
                velocity = dir * monster.jumpForce;
                targetPoint = monster.input.clickPos;
                minTargetDistance = Mathf.Infinity;
                var col = Physics2D.OverlapCircle(monster.input.clickPos, 0.1f);
                if (col != null) {
                    targetSkyscraper = col.GetComponent<WorldObject>();
                }
                onSkyscraper = false;
            }
            */
        }
        
        if (checkSkyscraper()) {
            if (targetSkyscraper == null && currentSkyscraper != null) {
                if (hit.collider.gameObject != currentSkyscraper.gameObject) {
                    velocity = Vector3.zero;
                    currentSkyscraper = hit.collider.GetComponent<WorldObject>();
                    targetSkyscraper = null;
                    onSkyscraper = true;
                    var angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
            if (targetSkyscraper != null) {
                var targetDistance = (transform.position - targetPoint).magnitude;
                if (targetDistance < minTargetDistance) {
                    minTargetDistance = targetDistance;
                }
                else {
                    velocity = Vector3.zero;
                    currentSkyscraper = targetSkyscraper;
                    targetSkyscraper = null;
                    onSkyscraper = true;
                }
            }
        }
        /*
        else
            currentSkyscraper = null;
            */

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
                onGround = false;
            }
            else if (!sittingOnObject.canThrow && sittingOnObject.canBlood) {
                sittingOnObject.damage(monster.damageValue);
            }
        }

        

        // TODO : move to a separate class
        var bloodParts = checkBlood();
        for (var i = 0; i < bloodParts.Length; ++i) {
            // TODO : increase specs
            bloodParts[i].GetComponent<Blood>().destroy();
        }

        transform.position += velocity * Time.deltaTime;
        if (!onGround && !onSkyscraper)
            velocity.y += Game.instance.gravity * Time.deltaTime;

        if (velocity.magnitude > 0 && !onGround) {
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, monster.size.x, Game.instance.groundLayer | Game.instance.worldObjectLayer);
        return hit;
    }

    bool checkSkyscraper() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, monster.size.x, Game.instance.groundLayer | Game.instance.skyscrapperLayer);
        return hit;
    } 

    Collider2D[] checkBlood() {
        return Physics2D.OverlapAreaAll(transform.position - monster.size, transform.position + monster.size, Game.instance.bloodLayer);
    }
}
