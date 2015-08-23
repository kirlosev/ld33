using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour {
    public SpriteRenderer sr;
    public AnimationCurve animProgress;
    public bool oneHitAnim = false;
    public float animDuration = 1f;
    public float animSpeed = 12;
    public float moveSpeed = 0.05f;
    
    IEnumerator animate() {
        var i = 0;
        var animTimer = 0f;
        while (animTimer <= animDuration) {
            var nextScale = transform.localScale;
            nextScale.x = animProgress.Evaluate(Mathf.Clamp(animTimer / animDuration, 0f, 1f));
            nextScale.y = animProgress.Evaluate(Mathf.Clamp(animTimer / animDuration, 0f, 1f));
            transform.localScale = nextScale;
            transform.position += Vector3.up * moveSpeed * 1f / animSpeed;
            yield return new WaitForSeconds(1f / animSpeed);
            animTimer += 1f / animSpeed;
        }
        gameObject.SetActive(false);
    }

    public void init(Vector3 pos) {
        transform.position = pos;
        moveSpeed += Random.Range(-0.05f, 0.1f);
        animDuration += Random.Range(-4f, 4f);
        StartCoroutine(animate());
    }
}
