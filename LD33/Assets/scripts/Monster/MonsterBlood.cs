using UnityEngine;
using System.Collections;

public class MonsterBlood : MonoBehaviour {
    public Monster monster;

    void FixedUpdate() {
        var bloodParts = checkBlood();
        for (var i = 0; i < bloodParts.Length; ++i) {
            takeBlood();
            bloodParts[i].GetComponent<Blood>().destroy();
        }
    }

    Collider2D[] checkBlood() {
        return Physics2D.OverlapAreaAll(transform.position - monster.size, transform.position + monster.size, Game.instance.bloodLayer);
    }

    public void takeBlood() {
        monster.blood = Mathf.Clamp(monster.blood + 2, monster.maxBloodPerJump / 2f, monster.maxBlood);
        monster.health = Mathf.Clamp(monster.health + 1, 0f, monster.maxHealth);
        monster.sounder.playSound(monster.takeBloodSound);
    }

    public float reserveBlood(float amount) {
        return Mathf.Clamp(monster.blood - Mathf.Clamp(monster.blood - amount, monster.maxBloodPerJump / 2f, monster.maxBlood), monster.maxBloodPerJump / 2f, monster.maxBlood);
    }

    public float getBlood(float amount) {
        var saveBloodAmount = monster.blood;
        monster.blood = Mathf.Clamp(monster.blood - amount, monster.maxBloodPerJump/2f, monster.maxBlood);
        return Mathf.Abs(saveBloodAmount - monster.blood);
    }
}
