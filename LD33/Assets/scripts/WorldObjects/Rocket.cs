using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite[] animSprite;
    public float animSpeed = 12;
    public float moveSpeed = 0.5f;

    Vector3 velocity;
    RaycastHit2D hit;
    Vector3 size;

    void Start() {
        size = GetComponent<Collider2D>().bounds.extents;
    }

    void Update() {
        if (checkGround()) {
            damage();
        }

        if (checkPlayer()) {
            hit.collider.GetComponent<Monster>().damage(50f);
            damage();
        }

        transform.position += velocity * moveSpeed * Time.deltaTime;

        var targetPos = Game.instance.monster.transform.position;
        velocity += (targetPos - transform.position).normalized * Time.deltaTime;
        if (velocity.magnitude > moveSpeed) {
            velocity = velocity.normalized * moveSpeed;
        }

        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle); 
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.groundLayer | Game.instance.skyscraperLayer | Game.instance.playerLayer);
        return hit;
    }

    bool checkPlayer() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.playerLayer);
        return hit;
    }

    IEnumerator animate() {
        var i = 0;
        while (true) {
            sr.sprite = animSprite[i++%animSprite.Length];
            yield return new WaitForSeconds(1f/animSpeed);
        }
    }

    public void init(Vector3 pos, Vector3 dir) {
        transform.position = pos;
        velocity = dir;
        StartCoroutine(animate());
    }

    public void damage(float value = 1) {
        var expMan = ObjectPool.instance.getExplosionManager();
        expMan.gameObject.SetActive(true);
        expMan.init(transform.position);

        gameObject.SetActive(false);
    }
}
