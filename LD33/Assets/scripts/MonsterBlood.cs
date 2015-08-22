using UnityEngine;
using System.Collections;

public class MonsterBlood : MonoBehaviour {
    public Monster monster;

    void FixedUpdate() {
        var bloodParts = checkBlood();
        for (var i = 0; i < bloodParts.Length; ++i) {
            // TODO : increase specs
            bloodParts[i].GetComponent<Blood>().destroy();
        }
    }

    Collider2D[] checkBlood() {
        return Physics2D.OverlapAreaAll(transform.position - monster.size, transform.position + monster.size, Game.instance.bloodLayer);
    }
}
