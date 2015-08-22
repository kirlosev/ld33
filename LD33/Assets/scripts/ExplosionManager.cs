using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {
    IEnumerator Start() {
        for (var i = 0; i < 5; ++i) {
            var exp = ObjectPool.instance.getExplosion();
            exp.gameObject.SetActive(true);
            exp.init(transform.position + (Vector3)Random.insideUnitCircle);
            yield return new WaitForFixedUpdate();
        }
        for (var i = 0; i < 10; ++i) {
            var smo = ObjectPool.instance.getSmoke();
            smo.gameObject.SetActive(true);
            smo.init(transform.position + (Vector3)Random.insideUnitCircle);
            yield return new WaitForFixedUpdate();
        }
    }
}
