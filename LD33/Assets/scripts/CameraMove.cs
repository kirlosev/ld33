using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public Character target;
    Vector3 velocity;
    public float smoothTime = 0.3f;

    void LateUpdate() {
        var nextPos = target.transform.position;
        nextPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, smoothTime); ;
    }
}
