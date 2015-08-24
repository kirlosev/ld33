using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {
    public bool isAlive = true;
    public bool canThrow = false;
    public float health = 1f;
    public float maxHealth = 1f;
    public Vector3 size;
    public Vector3 velocity;
    bool isThrown = false;
    public bool checkRect = true;
    RaycastHit2D hit;
    public float damageValue = 1f;
    public bool canBlood = false;
    public bool disableOnDestroy = false;
    public bool inAir = false;
    public bool explosive = false;

    void Awake() {
        size = GetComponent<Collider2D>().bounds.extents;
    }

    public void Start() {
    }

    void Update() {
        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(velocity.x) && velocity.magnitude > 0.1f)
            flipScale();
        if (checkGround()) {
            inAir = false;
            if (!isAlive) {
                velocity = Vector3.zero;
            }
            if (isThrown) {
                var reflectedDir = 0.62f * (velocity - 2 * Vector3.Dot(velocity, hit.normal) * (Vector3)hit.normal);
                if (reflectedDir.magnitude < 0.1f || (Vector3)hit.normal == Vector3.up) {
                    velocity = Vector3.zero;
                    isThrown = false;
                    damage(5f);
                }
                else {
                    velocity = reflectedDir;
                }
                if (hit.collider.GetComponent<Rocket>() != null) {
                    hit.collider.GetComponent<Rocket>().damage(1f);
                }
            }
        }
        else {
            if (canThrow)
                inAir = true;
        }

        if (!isThrown) {
            calcVelocity();
        }
        else {
            var cols = checkWorldObject();
            for (var i = 0; i < cols.Length; ++i) {
                if (gameObject == cols[i].gameObject)
                    continue;
                cols[i].gameObject.GetComponent<WorldObject>().damage(damageValue);
            }
        }

        transform.position += velocity * Time.deltaTime;
        if (inAir) {
            velocity.y += Game.instance.gravity * Time.deltaTime;
        }

        if (transform.position.x > Game.instance.rightTopCorner.position.x) {
            var repos = transform.position;
            repos.x = Game.instance.leftBottomCorner.position.x;
            transform.position = repos;
        }
        else if (transform.position.x < Game.instance.leftBottomCorner.position.x) {
            var repos = transform.position;
            repos.x = Game.instance.rightTopCorner.position.x;
            transform.position = repos;
        }
    }

    public virtual void calcVelocity() {
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.groundLayer | Game.instance.projectilesLayer);
        return hit;
    }

    Collider2D[] checkWorldObject() {
        var cols = new Collider2D[0];
        if (checkRect)
            cols = Physics2D.OverlapAreaAll(transform.position - size, transform.position + size, Game.instance.worldObjectLayer);
        else
            cols = Physics2D.OverlapCircleAll(transform.position, size.y, Game.instance.worldObjectLayer);
        return cols;
    }

    public void damage(float value) {
        health -= value;
        if (health <= 0) {
            destroy();
        }
    }

    public void destroy() {
        var amount = (int)Random.Range(7f, 14f);
        if (isAlive) {
            if (explosive) {
                var expMan = ObjectPool.instance.getExplosionManager();
                expMan.gameObject.SetActive(true);
                expMan.init(transform.position);
            }
            for (var i = 0; i < amount; ++i) {
                var b = ObjectPool.instance.getBlood();
                b.gameObject.SetActive(true);
                b.enabled = true;
                b.init(transform.position,
                        (Vector3)hit.normal
                        + Vector3.right * Random.Range(-0.5f, 0.5f) * Mathf.Sign(hit.normal.x)
                        + Vector3.up * Random.Range(-0.5f, 0.5f) * Mathf.Sign(hit.normal.y),
                        (amount * 4 - i));
            }
        }
        isAlive = false;
        if (disableOnDestroy) {
            gameObject.SetActive(false);
        }
    }

    public void throwObject(Vector3 dir, float force) {
        isThrown = true;
        velocity = dir.normalized * force;
    }

    public void flipScale() {
        var tmp = transform.localScale;
        tmp.x *= -1f;
        transform.localScale = tmp;
    }

    public virtual void init(Vector3 pos) {
        transform.position = pos;
        isAlive = true;
        health = maxHealth;
        isThrown = false;
    }
}
