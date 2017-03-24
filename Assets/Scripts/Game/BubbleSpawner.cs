using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject bubble;
    public Transform bubbleJar;
    public int initialMinSpeed = 5;
    public int initialMaxSpeed = 25;
    public float speedIncrement = 0.5f;
    public float spawnRate = 0.7f; // delay between bubbles spawns
    public float maxSpawnRate = 0.05f;
    public float spawnRateIncrement = 0.05f;
    public int lowDiceValue = 1;
    public int highDiceValue = 6;
    public int numDice = 2;

    float spawnerZPos;


    float nextSpawn = 0;

    const float PIPE_THICKNESS = 2f;

    class Range {
        public float min;
        public float max;

        public Range() {
            min = 0;
            max = 10;
        }

        public Range(float minimum, float maximum) {
            min = minimum;
            max = maximum;
        }
    }

    Range speedRange;
    Range typeRange;
    Range spawnerXRange;
    Range spawnerYRange;
    Range pipeRange;

    Dictionary<int, int> typeMapping = new Dictionary<int, int>();
    int currentType = 0;
    List<int> spawnableTypes = new List<int>();

    void Start () {
        Collider collider = this.GetComponent<Collider>();

        spawnerZPos = collider.bounds.center.z;
        spawnerXRange = new Range(collider.bounds.min.x, collider.bounds.max.x);
        spawnerYRange = new Range(collider.bounds.min.y, collider.bounds.max.y);

        speedRange = new Range(initialMinSpeed, initialMaxSpeed);
        typeRange = new Range(lowDiceValue, highDiceValue);
        pipeRange = new Range(0, PipeBuilder.pipeRadius);

        reset();
    }

	void Update () {
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0) {
            nextSpawn = spawnRate;
            spawnBubble();
        }
    }

    void reset() {
        spawnableTypes = new List<int>();
        chooseSpawnableTypes(1);
        populateMapping();
    }

    void spawnBubble() {
        Vector3 position = selectPosition();
        float speed = selectSpeed();
        int type = selectBubbleType();

        if (type == Bubble.SIMPLE_BUBBLE || type == Bubble.SINE_BUBBLE || type == Bubble.DOUBLE_SINE_BUBBLE || type == Bubble.HOMING_BUBBLE) {
            instantiateBubble(position, speed, type, Quaternion.identity);
        }
        else if (type == Bubble.SIMPLE_CLUSTER) {
            spawnSimpleCluster(position, speed);
        }
        else if (type == Bubble.HELIX_CLUSTER) {
            spawnHelixCluster(position, speed);
        } 
        else if (type == Bubble.EXPANDING_CLUSTER) {
            spawnExpandingCluster(position, speed);
        }
    }

    void instantiateBubble(Vector3 position, float speed, int type, Quaternion rotation) {
        GameObject b = (GameObject)Instantiate(bubble, position, rotation);
        b.transform.parent = bubbleJar;

        Bubble bubbleScript = b.GetComponent<Bubble>();
        bubbleScript.speed = speed;
        bubbleScript.type = type;
    }

    Vector3 selectPosition() {
        float x = 0;
        float y = 0;

        do {
            x = Random.Range(-pipeRange.max + PIPE_THICKNESS, pipeRange.max - PIPE_THICKNESS);
            y = Random.Range(-pipeRange.max + PIPE_THICKNESS, pipeRange.max - PIPE_THICKNESS);
        } while ((Mathf.Pow(x, 2) + Mathf.Pow(y, 2)) >= Mathf.Pow(pipeRange.max, 2));

        return new Vector3(x, y, spawnerZPos);
    }

    int selectBubbleType() {
        int outcome = 0;

        for (int i = 0; i < numDice; i++) {
            outcome += Random.Range((int)typeRange.min, (int)typeRange.max);
        }

        return typeMapping[outcome];
    }

    float selectSpeed() {
        return Random.Range(speedRange.min, speedRange.max);
    }

    void increaseDifficulty(int level) {
        speedRange.min += speedIncrement;
        speedRange.max += speedIncrement;

        if ((spawnRate - spawnRateIncrement) >= maxSpawnRate) {
            spawnRate -= spawnRateIncrement;
        }

        chooseSpawnableTypes(level);

        shuffleTypeMapping();
    }

    void populateMapping() {
        // reset some stuff
        typeMapping = new Dictionary<int, int>();
        currentType = 0;

        // prep useful variables
        int minRoll = numDice * (int)typeRange.min;
        int maxRoll = numDice * (int)typeRange.max;
        int numOutcomes = maxRoll - minRoll + 1;
        int numIterations = (int)Mathf.Ceil(numOutcomes / 2.0f);
        int midPoint = (maxRoll + minRoll) / 2;

        // iterate from the middle of the mapping outward
        for (int i = 0; i <= numIterations; i++) {
            if (i == 0) {
                addTypeMapping(midPoint);
            } else {
                if (midPoint + i <= maxRoll) {
                    addTypeMapping(midPoint + i);
                }
                if (midPoint - i >= minRoll) {
                    addTypeMapping(midPoint - i);
                }
            }
        }
    }

    void addTypeMapping(int index) {
        if (currentType >= spawnableTypes.Count) {
            currentType = 0;
        }

        typeMapping.Add(index, spawnableTypes[currentType++]);
    }

    // Change the switch case in the function to configure what level different bubbles types are added
    void chooseSpawnableTypes(int level) {
        int oldSize = spawnableTypes.Count;

        switch(level) {
            case 1:
                spawnableTypes.Add(Bubble.SIMPLE_BUBBLE);
                spawnableTypes.Add(Bubble.SINE_BUBBLE);
                spawnableTypes.Add(Bubble.SIMPLE_CLUSTER);
                break;
            case 6:
                spawnableTypes.Add(Bubble.DOUBLE_SINE_BUBBLE);
                break;
            case 11:
                spawnableTypes.Add(Bubble.HOMING_BUBBLE);
                break;
            case 16:
                break;
            default:
                break;
        }

        if (spawnableTypes.Count > oldSize) {
            populateMapping();
        }
    }

    void shuffleTypeMapping() {

    }

    void spawnSimpleCluster(Vector3 center, float speed) {
        instantiateBubble(center + new Vector3(-1, 0, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
        instantiateBubble(center + new Vector3(1, 0, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
        instantiateBubble(center + new Vector3(0, -1, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
        instantiateBubble(center + new Vector3(0, 1, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
    }

    void spawnHelixCluster(Vector3 center, float speed) {

    }

    void spawnExpandingCluster(Vector3 center, float speed) {

    }
}
