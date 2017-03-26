using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	void Start () {
        gameStart(); // This should be removed once the menu is added.
    }

    // Called when play button in menu is clicked
    public void gameStart() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().enable();

        GameObject bubbleSpawner = GameObject.FindGameObjectWithTag("BubbleSpawner");
        bubbleSpawner.GetComponent<BubbleSpawner>().enable();
    }

    // Called when player hit by bubble
    public void gameEnd() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().disable();

        GameObject bubbleSpawner = GameObject.FindGameObjectWithTag("BubbleSpawner");
        bubbleSpawner.GetComponent<BubbleSpawner>().disable();

        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
        for (int i = 0; i < bubbles.Length; i++) {
            Destroy(bubbles[i]);
        }
    }
}
