using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    // A manager class that handles all score related functionality as well as triggering level changes.

    public Text scoreText;
    public Text levelText;

    float nextLevelUp;
    int currentLevel = 1;

    int LEVEL_UP = 10; // Amount of seconds between each level change

    void Start() {
        reset();
    }

    void Update() {
        scoreText.text = "Score: " + scoreAsString();

        if (score() > nextLevelUp) {
            levelUp();
        }
    }

    float score() {
        return Time.time;
    }

    string scoreAsString() {
        return Time.time.ToString("0.00");
    }

    void reset() {
        nextLevelUp = Time.time + LEVEL_UP;
        currentLevel = 1;
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
        nextLevelUp += LEVEL_UP;
        currentLevel++;

        levelText.text = "Level: " + currentLevel.ToString();

        GameObject.FindGameObjectWithTag("PipeBuilder").SendMessage("changeTheme");
        GameObject.FindGameObjectWithTag("BubbleSpawner").SendMessage("increaseDifficulty", currentLevel);
    }
}
