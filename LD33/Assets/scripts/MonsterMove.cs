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

    void Start() {
        vertexCount = (int)Mathf.Ceil(trajectoryTime / timeResolution);
        lr.SetVertexCount(vertexCount);
        Debug.Log("vert count: " + vertexCount);
        StartCoroutine(calcTrajectory());
    }

    void Update() {
        if (monster.input.jumpPressed) {
            startJumpTime = Time.time;
            startJump = true;
            lr.enabled = true;
        }

        if (startJump) {
            var deltaTime = Mathf.Clamp(Time.time - startJumpTime, 0f, monster.maxJumpTime);
            jumpForce = deltaTime * monster.maxJumpForce / monster.maxJumpTime;
            var bloodThisJump = (monster.move.jumpForce / monster.maxJumpForce) * monster.maxBloodPerJump;
            monster.bloodTaken = monster.bloodManager.reserveBlood(bloodThisJump);
        }

        if (monster.input.jumpReleased && !inAir) {
            var dir = (monster.input.clickPos - transform.position).normalized;
            Debug.Log("jumpForce : " + jumpForce);
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
            inAir = true;
            startJump = false;
            lr.enabled = false;
            Debug.Log("blood before taking : "+monster.blood +", jump force: "+jumpForce);
            var bb = monster.bloodManager.getBlood(monster.bloodTaken);
            monster.bloodTaken = 0f;
            Debug.Log("took blood : "+ bb +", remain : "+ monster.blood);
        }

        if (checkGround()) {
            if (inAir) {
                velocity = Vector3.zero;
                inAir = false;
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
                }
            }
            previousObject = null;
        }

        if (monster.input.throwObject && sittingOnObject != null) {
            if (sittingOnObject.canThrow) {
                var dir = (monster.input.clickPos - transform.position).normalized;
                velocity = -dir * monster.maxJumpForce;
                sittingOnObject.throwObject(dir, monster.throwForce);
                previousObject = sittingOnObject;
                sittingOnObject = null;
                transform.SetParent(null);
                inAir = true;
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
    }

    IEnumerator calcTrajectory() {
        while (true) {
            if (startJump) {
                var point = transform.position;
                var dir = (monster.input.mousePos - point).normalized;
                var pointVelocity = dir * jumpForce;
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
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
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
