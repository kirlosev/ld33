using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {
    public Image docs;
    public Sprite[] docsPages;
    public GameObject prevBtn, nextBtn;
    public GameObject startBtn, quitBtn;
    public Text timeText;
    int currentPage = 0;
    public bool gameOverMenu = false;

    void Start() {
        if (!gameOverMenu) {
            if (PlayerPrefs.HasKey("bestTime")) {
                timeText.text = PlayerPrefs.GetFloat("bestTime").ToString("0:00.00");
            } else {
                timeText.text = "NO INFO";
            }
            checkBtns();
        }
        else {
            timeText.text = PlayerPrefs.GetFloat("currentTime").ToString("0:00.00");
        }
    }

    public void nextPage() {
        docs.sprite = docsPages[Mathf.Clamp(++currentPage, 0, docsPages.Length)];
        checkBtns();
    }

    public void prevPage() {
        docs.sprite = docsPages[Mathf.Clamp(--currentPage, 0, docsPages.Length)];
        checkBtns();
    }

    void checkBtns() {
        if (currentPage == 0) {
            nextBtn.SetActive(true);
            prevBtn.SetActive(false);

            startBtn.SetActive(true);
            quitBtn.SetActive(true);
        }
        else if (currentPage == docsPages.Length - 1) {
            nextBtn.SetActive(false);
            prevBtn.SetActive(true);

            startBtn.SetActive(false);
            quitBtn.SetActive(false);
        }
        else {
            nextBtn.SetActive(true);
            prevBtn.SetActive(true);

            startBtn.SetActive(false);
            quitBtn.SetActive(false);
        }
        timeText.gameObject.SetActive(currentPage == 3);
    }

    public void start() {
        Application.LoadLevel("gamescene");
    }

    public void quit() {
        Application.Quit();
    }

    public void mainMenu() {
        Application.LoadLevel("menu");
    }
}
