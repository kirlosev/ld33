using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
    public Transform cam;
    public Transform[] parallaxObjects;
    float[] parallaxScales;

    void Start() {
        parallaxScales = new float[parallaxObjects.Length];
        for (var i = 0; i < parallaxScales.Length; ++i) {
            parallaxScales[i] = parallaxObjects[i].position.z;
        }

        StartCoroutine(makeParallax());
    }

    IEnumerator makeParallax() {
        var prevPos = cam.position;
        while (true) {
            for (var i = 0; i < parallaxObjects.Length; ++i) {
                var deltaPos = cam.position.x - prevPos.x;
                parallaxObjects[i].transform.position += Vector3.right * parallaxScales[i] * deltaPos * Time.deltaTime;
            }
            prevPos = cam.position;
            yield return null;
        }
    }
}
