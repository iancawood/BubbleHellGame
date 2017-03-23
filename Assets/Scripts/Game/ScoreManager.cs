using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    // A manager class that handles all score related functionality as well as triggering level changes.

    public Text highScoreText;

    int nextLevelUp;
    int currentLevel = 1;

    int LEVEL_UP = 10; // Amount of seconds between each level change

    void Start() {
        reset();
    }

    void Update() {
        highScoreText.text = "Score: " + scoreAsString();

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
        nextLevelUp = LEVEL_UP;
        currentLevel = 1;
    }

    void levelUp() {
        Debug.Log("Level up!");

        nextLevelUp += LEVEL_UP;
        currentLevel++;

        GameObject.FindGameObjectWithTag("PipeBuilder").SendMessage("changeTheme");
        GameObject.FindGameObjectWithTag("BubbleSpawner").SendMessage("increaseDifficulty", currentLevel);
    }
}
