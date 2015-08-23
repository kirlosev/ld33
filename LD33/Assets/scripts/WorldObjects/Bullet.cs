using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite[] animSprite;
    public float animSpeed = 12;
    public float moveSpeed = 0.5f;

    Vector3 velocity;
    
    void Update() {
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

    public void init(Vector3 pos, Vector3 dir) {
        transform.position = pos;
        velocity = dir;
        StartCoroutine(animate());
    }

    public void damage(float value) {
        gameObject.SetActive(false);
    }
}
