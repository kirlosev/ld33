using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public Character target;
    Vector3 velocity;
    public float smoothTime = 0.3f;

    public bool isShaking = false;
    Vector3 originPos;
    float shakeForce;

    void LateUpdate() {
        var nextPos = target.transform.position;
        nextPos.z = transform.position.z;
        if (!isShaking)
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, smoothTime);
        else
            transform.position = nextPos + (Vector3)Random.insideUnitCircle * shakeForce;
    }

    public void shake(float duration, float force) {
        if (!isShaking) {
            originPos = transform.position;
            shakeForce = force;
            StartCoroutine(shakeTimer(duration));
        }
    }

    IEnumerator shakeTimer(float shakeDuration) {
        isShaking = true;
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
    }
}
