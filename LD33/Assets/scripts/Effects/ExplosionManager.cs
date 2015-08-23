using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {
    public AudioClip sound;
    public float explosionRadius = 1f;
    public float damageValue = 1f;

    public void init(Vector3 pos) {
        transform.position = pos;
        Game.instance.playSfx(sound);
        /*
        var cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius, Game.instance.worldObjectLayer);
        for (var i = 0; i < cols.Length; ++i) {
            cols[i].GetComponent<WorldObject>().damage(damageValue);
        }
        cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius, Game.instance.playerLayer);
        for (var i = 0; i < cols.Length; ++i) {
            cols[i].GetComponent<Character>().damage(damageValue);
        }
        */
        StartCoroutine(explode());
    }

    IEnumerator explode() {
        Game.instance.gameplayCam.shake(0.4f, 0.1f);
        for (var i = 0; i < 5; ++i) {
            var exp = ObjectPool.instance.getExplosion();
            exp.gameObject.SetActive(true);
            exp.init(transform.position + (Vector3)Random.insideUnitCircle * explosionRadius);
            yield return new WaitForFixedUpdate();
        }
        for (var i = 0; i < 10; ++i) {
            var smo = ObjectPool.instance.getSmoke();
            smo.gameObject.SetActive(true);
            smo.init(transform.position + (Vector3)Random.insideUnitCircle * explosionRadius);
            yield return new WaitForFixedUpdate();
        }
    }
}
