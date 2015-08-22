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
    public bool checkRect = true;
    RaycastHit2D hit;
    public float damageValue = 1f;

    void Awake() {
        size = GetComponent<Collider2D>().bounds.extents;
    }

    void Update() {
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
        
        if (checkGround() && isThrown) {
            Debug.DrawLine(transform.position, hit.point, Color.magenta);
            var reflectedDir = 0.38f * (velocity - 2 * Vector3.Dot(velocity, hit.normal) * (Vector3)hit.normal);
            if (reflectedDir.magnitude < 0.1f) {
                velocity = Vector3.zero;
                isThrown = false;
            }
            else velocity = reflectedDir;

            Debug.Log("ground : "+hit.collider.gameObject.name);
        }

        transform.position += velocity * Time.deltaTime;
        if (isThrown)
            velocity.y += Game.instance.gravity * Time.deltaTime;
    }

    public virtual void calcVelocity() {
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.groundLayer);
        return hit;
    }
    
    Collider2D[] checkWorldObject() {
        var cols = new Collider2D[0];
        if (checkRect)
            cols = Physics2D.OverlapAreaAll(transform.position-size, transform.position+size, Game.instance.worldObjectLayer);
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
        isAlive = false;
        // TODO : create blood
        gameObject.SetActive(false);
    }

    public void throwObject(Vector3 dir, float force) {
        isThrown = true;
        velocity = dir.normalized * force;
    }
}
