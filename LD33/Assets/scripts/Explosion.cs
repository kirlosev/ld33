using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    public SpriteRenderer sr;
    public Color[] animColors;
    public bool oneHitAnim = false;
    public float animDuration = 1f;
    public float animSpeed = 12;

    void Start() {
        StartCoroutine(animate());
    }

    IEnumerator animate() {
        var i = 0;
        var animTimer = 0f;
        while (animTimer <= animDuration) {
            sr.color = animColors[i++%animColors.Length];
            yield return new WaitForSeconds(1f/animSpeed);
            animTimer += 1f / animSpeed;
        }
        gameObject.SetActive(false);
    }
}
