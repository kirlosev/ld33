using UnityEngine;
using System.Collections;

public class HelicopterShoot : MonoBehaviour {
    public Helicopter helicopter;
    Character target;
    public float shootPeriod = 1f;
    public int bulletsPerShoot = 4;
    public float delayBetweenBullets = 0.1f;
    Vector3 targetDir;
    
    public void init() {
        target = Game.instance.monster;
        StartCoroutine(shoot());
    }

    void Update() {
        targetDir = (target.transform.position - transform.position).normalized;
        var targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 180f;
    }

    IEnumerator shoot() {
        while (helicopter.isAlive) {
            for (var i = 0; i < bulletsPerShoot; ++i) {
                var b = ObjectPool.instance.getRocket();
                b.gameObject.SetActive(true);
                b.init(transform.position - transform.right * helicopter.size.x * 2f, transform.right);
                yield return new WaitForSeconds(delayBetweenBullets);
            }
            yield return new WaitForSeconds(shootPeriod);
        }
    }
}
