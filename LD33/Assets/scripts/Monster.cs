using UnityEngine;
using System.Collections;

public class Monster : Character {
    public MonsterMove move;
    public MonsterInput input;
    public float jumpForce = 10f;
    public float throwForce = 10f;
}
