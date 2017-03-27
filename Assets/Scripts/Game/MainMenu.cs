using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	void Start () {
        gameStart(); // This should be removed once the menu is added.
    }

    // Called when play button in menu is clicked
    public void gameStart() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enable();

        GameObject.FindGameObjectWithTag("BubbleSpawner").GetComponent<BubbleSpawner>().enable();

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>().enable();

        // disable menu here
    }

    // Called when player hit by bubble
    public void gameEnd() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().disable();

        GameObject.FindGameObjectWithTag("BubbleSpawner").GetComponent<BubbleSpawner>().disable();

        ScoreManager scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
        scoreManager.disable();
        saveHighscore(scoreManager.score());

        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
        for (int i = 0; i < bubbles.Length; i++) {
            Destroy(bubbles[i]);
        }

        Debug.Log(getHighscore());

        // enable menu here
    }

    void saveHighscore(float score) {
        float highscore = PlayerPrefs.GetFloat("HighScore");

        if (score > highscore) {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }

    float getHighscore() {
        return PlayerPrefs.GetFloat("HighScore");
    }
}
