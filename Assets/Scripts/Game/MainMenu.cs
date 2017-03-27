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
    }

    // Called when player hit by bubble
    public void gameEnd() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().disable();

        GameObject.FindGameObjectWithTag("BubbleSpawner").GetComponent<BubbleSpawner>().disable();

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>().disable();

        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
        for (int i = 0; i < bubbles.Length; i++) {
            Destroy(bubbles[i]);
        }
    }
}
