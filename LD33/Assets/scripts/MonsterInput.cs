using UnityEngine;
using System.Collections;

public class MonsterInput : MonoBehaviour {
    public Monster monster;

    public Vector3 clickPos;
    public bool jump = false;
    public bool throwObject = false;

    void Update() {
        jump = false;
        if (Input.GetMouseButtonUp(0)) {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            clickPos = mousePos;
            jump = true;
        }

        throwObject = false;
        if (Input.GetMouseButtonUp(1)) {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            clickPos = mousePos;
            throwObject = true;
        }
    }
}
