using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite[] animSprite;
    public float animSpeed = 12;
    public float moveSpeed = 0.5f;

    Vector3 velocity;

    void Start() {
        StartCoroutine(animate());
    }

    void Update() {
        var targetPos = Game.instance.monster.transform.position;
        velocity = (targetPos - transform.position).normalized;
        transform.position += velocity *moveSpeed* Time.deltaTime;
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle); 
    }

    IEnumerator animate() {
        var i = 0;
        while (true) {
            sr.sprite = animSprite[i++%animSprite.Length];
            yield return new WaitForSeconds(1f/animSpeed);
        }
    }

    public void damage(float value) {
        gameObject.SetActive(false);
    }
}
