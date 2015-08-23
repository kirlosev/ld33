using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    [Header("People")]
    public int amountHumans = 20;
    public int amountCars = 10;

    [Header("Enemies")]
    public int amountHelicopters = 5;
    public int amountTanks = 10;

    List<Human> humansOnScene;
    List<Car> carsOnScene;
    List<Helicopter> helicoptersOnScene;
    List<Tank> tanksOnScene;

    void Awake() {
        humansOnScene = new List<Human>();
        carsOnScene = new List<Car>();
        helicoptersOnScene = new List<Helicopter>();
        tanksOnScene = new List<Tank>();
    }

    void Start() {
        for (var i = 0; i < amountHumans; ++i) {
            var h = ObjectPool.instance.getHuman();
            var pos = new Vector3(Random.Range(Game.instance.leftBottomCorner.position.x, Game.instance.rightTopCorner.position.x), 1.226f);
            h.gameObject.SetActive(true);
            h.init(pos);
            humansOnScene.Add(h);
        }

        for (var i = 0; i < amountCars; ++i) {
            var h = ObjectPool.instance.getCar();
            var pos = new Vector3(Random.Range(Game.instance.leftBottomCorner.position.x, Game.instance.rightTopCorner.position.x), 1.5f);
            h.gameObject.SetActive(true);
            h.init(pos);
            carsOnScene.Add(h);
        }

        StartCoroutine(checkObjectsOnScene());
    }

    IEnumerator checkObjectsOnScene() {
        while (true) {
            for (var i = 0; i < humansOnScene.Count; ++i) {
                if (!humansOnScene[i].gameObject.activeInHierarchy) {
                    humansOnScene[i].gameObject.SetActive(true);
                    var pos = new Vector3(Random.Range(Game.instance.leftBottomCorner.position.x, Game.instance.rightTopCorner.position.x), 1.226f);
                    if (Mathf.Abs(Game.instance.leftBottomCorner.position.x - Game.instance.monster.transform.position.x)
                        > Mathf.Abs(Game.instance.rightTopCorner.position.x - Game.instance.monster.transform.position.x)) {
                        pos.x = Game.instance.rightTopCorner.position.x;
                    }
                    else {
                        pos.x = Game.instance.leftBottomCorner.position.x;
                    }
                    humansOnScene[i].init(pos);
                }
                yield return new WaitForFixedUpdate();
            }

            for (var i = 0; i < carsOnScene.Count; ++i) {
                if (!carsOnScene[i].isAlive) {
                    carsOnScene.RemoveAt(i);
                    var pos = new Vector3(Random.Range(Game.instance.leftBottomCorner.position.x, Game.instance.rightTopCorner.position.x), 1.5f);
                    /*
                    if (Mathf.Abs(Game.instance.leftBottomCorner.position.x - Game.instance.monster.transform.position.x)
                        > Mathf.Abs(Game.instance.rightTopCorner.position.x - Game.instance.monster.transform.position.x)) {
                        pos.x = Game.instance.rightTopCorner.position.x;
                    }
                    else {
                        pos.x = Game.instance.leftBottomCorner.position.x;
                    }
                    */
                    var h = ObjectPool.instance.getCar();
                    h.gameObject.SetActive(true);
                    h.init(pos);
                    carsOnScene.Add(h);
                    Debug.Log("added a new car");
                }
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
