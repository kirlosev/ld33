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

    Texture2D redScreenTexture;
    GUIStyle redScreenStyle;
    public Color redScreenColor;
    public float redScreenDuration = 0.1f;

    public AudioClip
        gameOverSound,
        hitSound,
        jumpSound,
        landingSound,
        takeBloodSound,
        throwSound;
    
    public void Start() {
        redScreenTexture = new Texture2D(1, 1);
        redScreenStyle = new GUIStyle();
    }

    public override void damage(float value) {
        sounder.playSound(hitSound);
        StartCoroutine(redScreen());
        Game.instance.gameplayCam.shake(0.1f, 0.4f);
        base.damage(value);
    }

    IEnumerator redScreen() {
        redScreenTexture.SetPixel(0, 0, redScreenColor);
        redScreenTexture.wrapMode = TextureWrapMode.Repeat;
        redScreenTexture.Apply();
        redScreenStyle.normal.background = redScreenTexture;
        yield return new WaitForSeconds(redScreenDuration);
        redScreenTexture.SetPixel(0, 0, Color.clear);
        redScreenTexture.Apply();
    }

    void OnGUI() {
        GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), redScreenTexture, redScreenStyle);
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
