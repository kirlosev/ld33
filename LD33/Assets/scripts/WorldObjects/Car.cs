using UnityEngine;
using System.Collections;

public class Car : WorldObject {
    public SpriteRenderer sr;
    public Sprite[] animSprite;
    public float animSpeed = 12;
    public float moveSpeed = 4f;
    Character target;

    public void Start() {
        base.Start();
        target = Game.instance.monster;
        moveSpeed += Random.Range(-0.5f, 0.5f);
        velocity = Vector3.right * moveSpeed * Mathf.Sign(Random.Range(-1f, 1f));
        StartCoroutine(animate());
    }

    public override void calcVelocity() {
        if (!isAlive) return;

        var targetDir = target.transform.position - transform.position;
        if (targetDir.magnitude < 2f) {
            velocity.x = -targetDir.normalized.x;
        }

        if (Physics2D.Raycast(transform.position, -Vector3.up, size.y, Game.instance.groundLayer)) {
            velocity.y = 0f;
        }
    }

    IEnumerator animate() {
        var i = 0;
        while (isAlive) {
            sr.sprite = animSprite[i++ % animSprite.Length];
            yield return new WaitForSeconds(1f / animSpeed);
        }
    }
}
