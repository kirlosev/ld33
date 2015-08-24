using UnityEngine;
using System.Collections;

public class Helicopter : WorldObject {
    public SpriteRenderer sr;
    public Sprite[] animSprite;
    public float animSpeed = 12;
    public float moveSpeed = 4f;
    Character target;
    public TankShoot shootBullet;
    public HelicopterShoot shootRocket;

    public void Start() {
        base.Start();
        target = Game.instance.monster;
    }

    public override void calcVelocity() {
        if (!isAlive) return;

        var targetDir = target.transform.position - transform.position;
        if (targetDir.magnitude < 2f) {
            velocity = -targetDir.normalized;
        }
        else if (targetDir.magnitude > 5f) {
            velocity = targetDir.normalized;
        }
        else {
            velocity = Vector3.up * Mathf.Sin(Time.time);
        }

        if (Physics2D.Raycast(transform.position, -Vector3.up, size.y, Game.instance.groundLayer)) {
            velocity = Vector3.up;
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
        if (shootRocket != null) {
            shootRocket.init();
        }
        else if (shootBullet != null) {
            shootBullet.init();
        }

        StartCoroutine(animate());
    }
}
