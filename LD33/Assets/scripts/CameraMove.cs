using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
    public Character target;
    Vector3 velocity;
    public float smoothTime = 0.3f;

    public bool isShaking = false;
    Vector3 originPos;
    float shakeForce;

    public Vector3 camSize;

    void Start() {
        camSize.y = GetComponent<Camera>().orthographicSize;
        camSize.x = (float)Screen.width * camSize.y / (float)Screen.height;
    }

    void LateUpdate() {
        var nextPos = target.transform.position;
        nextPos.z = transform.position.z;
        nextPos.x = Mathf.Clamp(nextPos.x, 
                                Game.instance.leftBottomCorner.position.x + camSize.x, 
                                Game.instance.rightTopCorner.position.x - camSize.x);
        nextPos.y = Mathf.Clamp(nextPos.y, 
                                Game.instance.leftBottomCorner.position.y + camSize.y, 
                                Game.instance.rightTopCorner.position.y - camSize.y);
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
