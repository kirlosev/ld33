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
    public Sounder sounder;
    public MonsterUI ui;

    public AudioClip
        gameOverSound,
        hitSound,
        jumpSound,
        landingSound,
        takeBloodSound,
        throwSound;

    public override void damage(float value) {
        sounder.playSound(hitSound);
        base.damage(value);
    }

    public override void destroy() {
        sounder.playSound(gameOverSound);
        var currentTime = Game.instance.getTimerValue();
        PlayerPrefs.SetFloat("currentTime", currentTime);
        if (currentTime > PlayerPrefs.GetFloat("bestTime")) {
            PlayerPrefs.SetFloat("bestTime", currentTime);
        }
        base.destroy();
        Application.LoadLevel("gameover");
    }
}
