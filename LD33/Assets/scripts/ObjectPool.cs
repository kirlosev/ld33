using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance;

    [Header("Effects")]
    public Explosion explosionInstance;
    public Smoke smokeInstance;

    public List<Explosion> explosionContainer;
    public List<Smoke> smokeContainer;

    void Awake() {
        instance = this;

        explosionContainer = new List<Explosion>();
        smokeContainer = new List<Smoke>();
    }

    public Explosion getExplosion() {
        for (var i = 0; i < explosionContainer.Count; ++i) {
            if (!explosionContainer[i].gameObject.activeInHierarchy) {
                return explosionContainer[i];
            }
        }
        var obj = Instantiate(explosionInstance, transform.position, Quaternion.identity) as Explosion;
        obj.gameObject.SetActive(false);
        explosionContainer.Add(obj);
        return obj;
    }

    public Smoke getSmoke() {
        for (var i = 0; i < smokeContainer.Count; ++i) {
            if (!smokeContainer[i].gameObject.activeInHierarchy) {
                return smokeContainer[i];
            }
        }
        var obj = Instantiate(smokeInstance, transform.position, Quaternion.identity) as Smoke;
        obj.gameObject.SetActive(false);
        smokeContainer.Add(obj);
        return obj;
    }
}
