using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    // A manager class that handles all score related functionality as well as triggering level changes.

    public Text scoreText;
    public Text levelText;

    float nextLevelUp;
    int currentLevel = 1;
    float startTime = 0;

    public int levelDuration = 10;

    void Start() {
        reset();
    }

    void Update() {
        scoreText.text = "Score: " + scoreAsString();

        if (score() > nextLevelUp) {
            levelUp();
        }
    }

    public float score() {
        return Time.time - startTime;
    }

    string scoreAsString() {
        return score().ToString("0.00");
    }

    void reset() {
        startTime = Time.time;
        nextLevelUp = levelDuration;
        currentLevel = 1;
        levelText.text = "Level: " + currentLevel.ToString();
    }

    public void disable() {
        scoreText.enabled = false;
        levelText.enabled = false;
    }

    public void enable() {
        scoreText.enabled = true;
        levelText.enabled = true;
        reset();
    }

    void levelUp() {
        nextLevelUp += levelDuration;
        currentLevel++;

        levelText.text = "Level: " + currentLevel.ToString();
        
        GameObject.FindGameObjectWithTag("BubbleSpawner").SendMessage("increaseDifficulty", currentLevel);
    }
}
