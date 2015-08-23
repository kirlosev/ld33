using UnityEngine;
using System.Collections;

public class Monster : Character {
    public MonsterMove move;
    public MonsterInput input;
    public MonsterBlood bloodManager;
    public float maxJumpForce = 10f;
    public float maxJumpTime = 1f;
    public float bloodTaken = 0f;
    public float maxBloodPerJump = 20f;
    public float throwForce = 10f;
    public float damageValue = 5f;
    public float blood = 0f;
    public float maxBlood = 100f;
    public MonsterUI ui;
}
