using UnityEngine;
using System.Collections;

public class MonsterInput : MonoBehaviour {
    public Monster monster;

    public Vector3 mousePos;
    public Vector3 clickPos;
    public bool jumpReleased = false;
    public bool jumpPressed = false;
    public bool throwObject = false;

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        jumpPressed = false;
        if (Input.GetMouseButtonDown(0)) {
            jumpPressed = true;
        }
        jumpReleased = false;
        if (Input.GetMouseButtonUp(0)) {
            clickPos = mousePos;
            jumpReleased = true;
        }

        throwObject = false;
        if (Input.GetMouseButtonUp(1)) {
            clickPos = mousePos;
            throwObject = true;
        }
    }
}
