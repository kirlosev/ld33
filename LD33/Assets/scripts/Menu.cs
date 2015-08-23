using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {
    public Image docs;
    public Sprite[] docsPages;
    public GameObject prevBtn, nextBtn;
    public GameObject startBtn, quitBtn;
    int currentPage = 0;

    void Start() {
        checkBtns();
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
    }

    public void start() {
        Application.LoadLevel("gamescene");
    }

    public void quit() {
        Application.Quit();
    }
}
