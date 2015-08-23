﻿using UnityEngine;
using System.Collections;

public class TankShoot : MonoBehaviour {
    public WorldObject parent;
    public Transform gun;
    Character target;
    public float shootPeriod = 1f;
    public int bulletsPerShoot = 4;
    public float delayBetweenBullets = 0.1f;
    Vector3 targetDir;
    Vector3 gunSize;

    void Start() {
        gunSize = gun.GetComponent<SpriteRenderer>().bounds.extents;
        // TODO : debug
        init();
    }

    public void init() {
        target = Game.instance.monster;
        StartCoroutine(shoot());
    }

    void Update() {
        targetDir = (target.transform.position - transform.position).normalized;
        var targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 180f;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        if (parent.transform.localScale.x < 0)
            gun.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            gun.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    IEnumerator shoot() {
        while (parent.isAlive) {
            for (var i = 0; i < bulletsPerShoot; ++i) {
                var b = ObjectPool.instance.getBullet();
                b.gameObject.SetActive(true);
                b.init(gun.transform.position - gun.transform.right * gunSize.x * 2f, targetDir);
                yield return new WaitForSeconds(delayBetweenBullets);
            }
            yield return new WaitForSeconds(shootPeriod);
        }
    }
}
