using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    public SpriteRenderer sr;
    public Color[] animColors;
    public bool oneHitAnim = false;
    public float animDuration = 1f;
    public float animSpeed = 12;
    public AudioClip sound;

    IEnumerator animate() {
        var i = 0;
        var animTimer = 0f;
        Game.instance.playSfx(sound);
        while (animTimer <= animDuration) {
            sr.color = animColors[i++%animColors.Length];
            yield return new WaitForSeconds(1f/animSpeed);
            animTimer += 1f / animSpeed;
        }
        gameObject.SetActive(false);
    }

    public void init(Vector3 pos) {
        transform.position = pos;
        StartCoroutine(animate());
    }
}
