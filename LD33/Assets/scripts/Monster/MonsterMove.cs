using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {
    public Monster monster;

    public bool inAir = true;
    Vector3 velocity;
    Vector3 targetPoint;
    Skyscraper targetSkyscraper;
    Skyscraper currentSkyscraper;
    RaycastHit2D hit;
    float minTargetDistance;
    WorldObject sittingOnObject, previousObject;
    float startJumpTime;
    bool startJump = false;
    public float jumpForce = 0f;
    public float trajectoryTime = 1f;
    public float timeResolution = 0.1f;
    public LineRenderer lr;
    int vertexCount;
    bool throwable = false;
    public Color jumpTrailColorStart, jumpTrailColorEnd,
                 throwTrailColorStart, throwTrailColorEnd;

    void Start() {
        vertexCount = (int)Mathf.Ceil(trajectoryTime / timeResolution);
        lr.SetVertexCount(vertexCount);
        StartCoroutine(calcTrajectory());
    }

    void Update() {
        if (monster.input.jumpPressed && !inAir) {
            startJumpTime = Time.time;
            startJump = true;
            lr.enabled = true;
            lr.SetColors(jumpTrailColorStart, jumpTrailColorEnd);
        }

        if (startJump) {
            var deltaTime = Mathf.Clamp(Time.time - startJumpTime, 0f, monster.maxJumpTime);
            jumpForce = deltaTime * monster.maxJumpForce / monster.maxJumpTime;
            var targetBloodTake = (monster.move.jumpForce / monster.maxJumpForce) * monster.maxBloodPerJump;
            monster.bloodTaken = monster.bloodManager.reserveBlood(targetBloodTake);
        }

        
        // 

        if (checkGround()) {
            if (inAir) {
                velocity = Vector3.zero;
                inAir = false;
                monster.sounder.playSound(monster.landingSound);
                transform.position = hit.point + hit.normal * monster.size.y;
                var angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            previousObject = null;
        }
        else if (checkWorldObject()) {
            if (hit.collider.GetComponent<WorldObject>() != previousObject) {
                velocity = Vector3.zero;
                inAir = false;
                monster.sounder.playSound(monster.landingSound);
                transform.position = (Vector3)hit.point
                                     + Vector3.up * hit.normal.y * monster.size.y
                                     + Vector3.right * hit.normal.x * monster.size.x;
                var angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                sittingOnObject = hit.collider.GetComponent<WorldObject>();
                if (!sittingOnObject.canThrow && sittingOnObject.canBlood) {
                    sittingOnObject.damage(monster.damageValue);
                    sittingOnObject = null;
                }
                else {
                    transform.SetParent(sittingOnObject.transform);
                    transform.localScale = Vector3.one;
                }
            }
        }
        else if (checkSkyscraper()) {
            if (targetSkyscraper == null) {
                if (hit.collider.GetComponent<Skyscraper>() != currentSkyscraper) {
                    velocity = Vector3.zero;
                    currentSkyscraper = hit.collider.GetComponent<Skyscraper>();
                    targetSkyscraper = null;
                    inAir = false;
                    monster.sounder.playSound(monster.landingSound);
                    var angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
            else {
                var targetDistance = (transform.position - targetPoint).magnitude;
                if (targetDistance < minTargetDistance) {
                    minTargetDistance = targetDistance;
                }
                else {
                    velocity = Vector3.zero;
                    currentSkyscraper = targetSkyscraper;
                    targetSkyscraper = null;
                    inAir = false;
                    monster.sounder.playSound(monster.landingSound);
                }
            }
            previousObject = null;
        }

        if (monster.input.jumpReleased && !inAir) {
            var dir = (monster.input.clickPos - transform.position).normalized;
            monster.bloodManager.getBlood(monster.bloodTaken);
            jumpForce = monster.bloodTaken / monster.maxBloodPerJump * monster.maxJumpForce;
            velocity = dir * jumpForce;
            targetPoint = monster.input.clickPos;
            minTargetDistance = Mathf.Infinity;
            var col = Physics2D.OverlapCircle(monster.input.clickPos, 0.1f, Game.instance.skyscraperLayer);
            if (col != null) {
                targetSkyscraper = col.GetComponent<Skyscraper>();
            }
            else
                targetSkyscraper = null;
            sittingOnObject = null;
            transform.SetParent(null);
            transform.localScale = Vector3.one;
            inAir = true;
            startJump = false;
            lr.enabled = false;
            monster.bloodTaken = 0f;
            monster.sounder.playSound(monster.jumpSound);
        }

        if (monster.input.throwObject && sittingOnObject != null) {
            if (sittingOnObject.canThrow) {
                var dir = (monster.input.clickPos - transform.position).normalized;
                velocity = -dir * monster.maxJumpForce;
                sittingOnObject.throwObject(dir, monster.throwForce);
                previousObject = sittingOnObject;
                sittingOnObject = null;
                transform.SetParent(null);
                transform.localScale = Vector3.one;
                inAir = true;
                lr.enabled = false;
                throwable = false;
                monster.sounder.playSound(monster.throwSound);
            }
        }
        else if (sittingOnObject != null) {
            if (sittingOnObject.canThrow) {
                throwable = true;
                lr.enabled = true;
                lr.SetColors(throwTrailColorStart, throwTrailColorEnd);
            }
        }

        if (sittingOnObject == null) {
            transform.position += velocity * Time.deltaTime;
            if (inAir) {
                velocity.y += Game.instance.gravity * Time.deltaTime;
            }
            if (velocity.magnitude > 0) {
                var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        if (transform.position.x > Game.instance.rightTopCorner.position.x) {
            var repos = transform.position;
            repos.x = Game.instance.leftBottomCorner.position.x;
            transform.position = repos;
        } else if (transform.position.x < Game.instance.leftBottomCorner.position.x) {
            var repos = transform.position;
            repos.x = Game.instance.rightTopCorner.position.x;
            transform.position = repos;
        }
    }

    IEnumerator calcTrajectory() {
        while (true) {
            if (startJump || throwable) {
                var point = transform.position;
                if (throwable && sittingOnObject != null) {
                    point = sittingOnObject.transform.position;
                }
                var dir = (monster.input.mousePos - point).normalized;
                var force = 0f;
                if (startJump) 
                    force = monster.bloodTaken / monster.maxBloodPerJump * monster.maxJumpForce;
                else
                    force = monster.throwForce;
                var pointVelocity = dir * force;
                lr.SetPosition(0, point);
                var index = 1;
                for (var i = 0f; index < vertexCount; i += timeResolution, index++) {
                    point += pointVelocity * timeResolution;
                    pointVelocity.y += Game.instance.gravity * timeResolution;
                    lr.SetPosition(index, point);
                }
                yield return null;
            }
            yield return null;
        }
    }

    bool checkGround() {
        var dir = -Vector3.up;
        hit = Physics2D.Raycast(transform.position, dir, monster.size.x, Game.instance.groundLayer);
        return hit;
    }

    bool checkWorldObject() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, monster.size.x, Game.instance.worldObjectLayer);
        return hit;
    }

    bool checkSkyscraper() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, monster.size.x, Game.instance.skyscraperLayer);
        return hit;
    }
}
