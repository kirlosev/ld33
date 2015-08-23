using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterUI : MonoBehaviour {
    public Monster monster;
    public Image healthBar;
    public Image bloodBar;
    public Image jumpBar;
    public Text timerText;
    float oldWidth;
    float maxBarWidth;
    Vector3 healthBarOriginPos;
    Vector3 bloodBarOriginPos;
    Vector3 jumpBarOriginPos;
    
    void Awake() {
        maxBarWidth = healthBar.rectTransform.sizeDelta.x;
        healthBarOriginPos = healthBar.rectTransform.localPosition;
        bloodBarOriginPos = bloodBar.rectTransform.localPosition;
        jumpBarOriginPos = jumpBar.rectTransform.localPosition;
    }
    
    void Update() {
        var res = healthBar.rectTransform.sizeDelta;
        res.x = (monster.health/monster.maxHealth) * maxBarWidth;
        healthBar.rectTransform.sizeDelta = res;
        healthBar.rectTransform.localPosition = healthBarOriginPos - Vector3.right * ((monster.health / monster.maxHealth) * maxBarWidth - maxBarWidth) / 2f;
        
        res = bloodBar.rectTransform.sizeDelta;
        res.x = (monster.blood / monster.maxBlood) * maxBarWidth;
        bloodBar.rectTransform.sizeDelta = res;
        bloodBar.rectTransform.localPosition = bloodBarOriginPos + Vector3.right * ((monster.blood / monster.maxBlood) * maxBarWidth - maxBarWidth) / 2f;

        res = jumpBar.rectTransform.sizeDelta;
        res.x = (monster.bloodTaken / monster.maxBlood) * maxBarWidth;
        jumpBar.rectTransform.sizeDelta = res;
        jumpBar.rectTransform.localPosition = jumpBarOriginPos + Vector3.right * ((monster.bloodTaken / monster.maxBlood) * maxBarWidth - maxBarWidth) / 2f;

        timerText.text = Game.instance.getTimerValue().ToString("0:00.00");
    }
}
