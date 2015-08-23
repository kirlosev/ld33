using UnityEngine;
using System.Collections;

public class MonsterBlood : MonoBehaviour {
    public Monster monster;

    void FixedUpdate() {
        var bloodParts = checkBlood();
        for (var i = 0; i < bloodParts.Length; ++i) {
            // TODO : increase specs
            takeBlood();
            bloodParts[i].GetComponent<Blood>().destroy();
        }
    }

    Collider2D[] checkBlood() {
        return Physics2D.OverlapAreaAll(transform.position - monster.size, transform.position + monster.size, Game.instance.bloodLayer);
    }

    public void takeBlood() {
        monster.blood = Mathf.Clamp(monster.blood + 1, 0, monster.maxBlood);
    }

    public float reserveBlood(float amount) {
        return Mathf.Abs(monster.blood - Mathf.Clamp(monster.blood - amount, 0, monster.maxBlood));
    }

    public float getBlood(float amount) {
        var saveBloodAmount = monster.blood;
        monster.blood = Mathf.Clamp(monster.blood - amount, 0, monster.maxBlood);
        return Mathf.Abs(saveBloodAmount - monster.blood);
    }
}
