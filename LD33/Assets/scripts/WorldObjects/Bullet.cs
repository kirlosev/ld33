using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
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
            hit.collider.GetComponent<Monster>().damage(10f);
            damage();
        }

        transform.position += velocity * moveSpeed * Time.deltaTime;
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator animate() {
        var i = 0;
        while (true) {
            sr.sprite = animSprite[i++ % animSprite.Length];
            yield return new WaitForSeconds(1f / animSpeed);
        }
    }

    bool checkGround() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.groundLayer);
        return hit;
    }

    bool checkPlayer() {
        var dir = velocity.magnitude > 0 ? velocity.normalized : -1 * (Vector3)hit.normal;
        hit = Physics2D.Raycast(transform.position, dir, size.y, Game.instance.playerLayer);
        return hit;
    }

    public void init(Vector3 pos, Vector3 dir) {
        transform.position = pos;
        velocity = dir;
        StartCoroutine(animate());
    }

    public void damage(float value = 1) {
        gameObject.SetActive(false);
    }
}
