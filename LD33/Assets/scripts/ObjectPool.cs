using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance;

    [Header("Effects")]
    public Explosion explosionInstance;
    public ExplosionManager explosionManagerInstance;
    public Smoke smokeInstance;
    public Blood bloodInstance;

    [Header("People")]
    public Human[] humanInstance;
    public Car[] carInstance;

    [Header("Enemies")]
    public Helicopter[] helicopterInstance;
    public Tank[] tankInstance;
    public Bullet bulletInstance;
    public Rocket rocketInstance;

    List<Explosion> explosionContainer;
    List<ExplosionManager> explosionManagerContainer;
    List<Smoke> smokeContainer;
    List<Human> humanContainer;
    List<Car> carContainer;
    List<Helicopter> helicopterContainer;
    List<Tank> tankContainer;
    List<Blood> bloodContainer;
    List<Bullet> bulletContainer;
    List<Rocket> rocketContainer;

    void Awake() {
        instance = this;

        explosionContainer = new List<Explosion>();
        explosionManagerContainer = new List<ExplosionManager>();
        smokeContainer = new List<Smoke>();
        humanContainer = new List<Human>();
        carContainer = new List<Car>();
        helicopterContainer = new List<Helicopter>();
        tankContainer = new List<Tank>();
        bloodContainer = new List<Blood>();
        bulletContainer = new List<Bullet>();
        rocketContainer = new List<Rocket>();
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

    public ExplosionManager getExplosionManager() {
        for (var i = 0; i < explosionManagerContainer.Count; ++i) {
            if (!explosionManagerContainer[i].gameObject.activeInHierarchy) {
                return explosionManagerContainer[i];
            }
        }
        var obj = Instantiate(explosionManagerInstance, transform.position, Quaternion.identity) as ExplosionManager;
        obj.gameObject.SetActive(false);
        explosionManagerContainer.Add(obj);
        return obj;
    }

    public Human getHuman() {
        for (var i = 0; i < humanContainer.Count; ++i) {
            if (!humanContainer[i].gameObject.activeInHierarchy) {
                return humanContainer[i];
            }
        }
        var ind = (int)Random.Range(0f, humanInstance.Length);
        var obj = Instantiate(humanInstance[ind], transform.position, Quaternion.identity) as Human;
        obj.gameObject.SetActive(false);
        humanContainer.Add(obj);
        return obj;
    }

    public Car getCar() {
        for (var i = 0; i < carContainer.Count; ++i) {
            if (!carContainer[i].gameObject.activeInHierarchy) {
                return carContainer[i];
            }
        }
        var ind = (int)Random.Range(0f, carInstance.Length);
        var obj = Instantiate(carInstance[ind], transform.position, Quaternion.identity) as Car;
        obj.gameObject.SetActive(false);
        carContainer.Add(obj);
        return obj;
    }

    public Helicopter getHelicopter() {
        for (var i = 0; i < helicopterContainer.Count; ++i) {
            if (!helicopterContainer[i].gameObject.activeInHierarchy) {
                return helicopterContainer[i];
            }
        }
        var ind = (int)Random.Range(0f, helicopterInstance.Length);
        var obj = Instantiate(helicopterInstance[ind], transform.position, Quaternion.identity) as Helicopter;
        obj.gameObject.SetActive(false);
        helicopterContainer.Add(obj);
        return obj;
    }

    public Tank getTank() {
        for (var i = 0; i < tankContainer.Count; ++i) {
            if (!tankContainer[i].gameObject.activeInHierarchy) {
                return tankContainer[i];
            }
        }
        var ind = (int)Random.Range(0f, tankInstance.Length);
        var obj = Instantiate(tankInstance[ind], transform.position, Quaternion.identity) as Tank;
        obj.gameObject.SetActive(false);
        tankContainer.Add(obj);
        return obj;
    }

    public Blood getBlood() {
        for (var i = 0; i < bloodContainer.Count; ++i) {
            if (!bloodContainer[i].gameObject.activeInHierarchy) {
                return bloodContainer[i];
            }
        }
        var obj = Instantiate(bloodInstance, transform.position, Quaternion.identity) as Blood;
        obj.gameObject.SetActive(false);
        bloodContainer.Add(obj);
        return obj;
    }

    public Bullet getBullet() {
        for (var i = 0; i < bulletContainer.Count; ++i) {
            if (!bulletContainer[i].gameObject.activeInHierarchy) {
                return bulletContainer[i];
            }
        }
        var obj = Instantiate(bulletInstance, transform.position, Quaternion.identity) as Bullet;
        obj.gameObject.SetActive(false);
        bulletContainer.Add(obj);
        return obj;
    }
    public Rocket getRocket() {
        for (var i = 0; i < rocketContainer.Count; ++i) {
            if (!rocketContainer[i].gameObject.activeInHierarchy) {
                return rocketContainer[i];
            }
        }
        var obj = Instantiate(rocketInstance, transform.position, Quaternion.identity) as Rocket;
        obj.gameObject.SetActive(false);
        rocketContainer.Add(obj);
        return obj;
    }

}
