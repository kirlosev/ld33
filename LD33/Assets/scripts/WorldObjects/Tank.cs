using UnityEngine;
using System.Collections;

public class Tank : WorldObject {
    public SpriteRenderer sr;
    public Sprite[] animSprite;
    public float animSpeed = 12;
    public float moveSpeed = 4f;
    Character target;
    public TankShoot shoot;

    public void Start() {
        base.Start();
        target = Game.instance.monster;
    }

    public override void calcVelocity() {
        if (!isAlive) return;

        var targetDir = target.transform.position - transform.position;
        if (targetDir.magnitude < 2f) {
            velocity.x = -targetDir.normalized.x * moveSpeed;
        }
        else if (targetDir.magnitude > 5f) {
            velocity.x = targetDir.normalized.x * moveSpeed;
        }
        else {
            velocity = Vector3.right * Mathf.Sin(Time.time) * moveSpeed;
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

    public override void init(Vector3 pos) {
        base.init(pos);
        moveSpeed += Random.Range(-0.5f, 0.5f);
        shoot.init();
        StartCoroutine(animate());
    }
}
