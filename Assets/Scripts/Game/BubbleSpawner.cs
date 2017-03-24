using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject bubble;
    public Transform bubbleJar;
    public int initialMinSpeed = 5;
    public int initialMaxSpeed = 25;
    public float speedIncrement = 0.5f;
    public float spawnRate = 3f; // delay between bubbles spawns
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

    // Reset bubble spawner to initial state
    void reset() {
        spawnableTypes = new List<int>();
        chooseSpawnableTypes(1);
        populateMapping();
    }

    // Chooses the properties of a bubble or bubble cluster and then spawns it
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

    // Instantiates a bubble game object
    GameObject instantiateBubble(Vector3 position, float speed, int type, Quaternion rotation) {
        GameObject b = (GameObject)Instantiate(bubble, position, rotation);
        b.transform.parent = bubbleJar;

        Bubble bubbleScript = b.GetComponent<Bubble>();
        bubbleScript.speed = speed;
        bubbleScript.type = type;

        return b;
    }

    // Randomly determines the position of a bubble
    // Must be within the bounds of the pipe
    Vector3 selectPosition() {
        float x = 0;
        float y = 0;

        do {
            x = Random.Range(-pipeRange.max + PIPE_THICKNESS, pipeRange.max - PIPE_THICKNESS);
            y = Random.Range(-pipeRange.max + PIPE_THICKNESS, pipeRange.max - PIPE_THICKNESS);
        } while ((Mathf.Pow(x, 2) + Mathf.Pow(y, 2)) >= Mathf.Pow(pipeRange.max, 2));

        return new Vector3(x, y, spawnerZPos);
    }

    // Selects the type of the bubble by rolling a set of dice and looking up the outcome
    int selectBubbleType() {
        int outcome = 0;

        for (int i = 0; i < numDice; i++) {
            outcome += Random.Range((int)typeRange.min, (int)typeRange.max);
        }

        return typeMapping[outcome];
    }

    // Randomly determine speed of bubble
    float selectSpeed() {
        return Random.Range(speedRange.min, speedRange.max);
    }

    // Called upon the level increasing and increase the difficulty of the bubbles spawned
    void increaseDifficulty(int level) {
        speedRange.min += speedIncrement;
        speedRange.max += speedIncrement;

        if ((spawnRate - spawnRateIncrement) >= maxSpawnRate) {
            spawnRate -= spawnRateIncrement;
        }

        chooseSpawnableTypes(level);

        shuffleTypeMapping();
    }

    // Fills the type mapping but starting in the middle and moving outward
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

    // Adds an entry into the type mapping
    void addTypeMapping(int index) {
        if (currentType >= spawnableTypes.Count) {
            currentType = 0;
        }

        typeMapping.Add(index, spawnableTypes[currentType++]);
    }

    // Determines what bubbles will exist at each level.
    // Change the switch case in the function to configure what level different bubbles types are added
    void chooseSpawnableTypes(int level) {
        int oldSize = spawnableTypes.Count;

        switch(level) {
            case 1:
                spawnableTypes.Add(Bubble.SIMPLE_BUBBLE);
                break;
            case 3:
                spawnableTypes.Add(Bubble.SINE_BUBBLE);
                break;
            case 6:
                spawnableTypes.Add(Bubble.SIMPLE_CLUSTER);
                spawnableTypes.Add(Bubble.DOUBLE_SINE_BUBBLE);
                break;
            case 11:
                spawnableTypes.Add(Bubble.HOMING_BUBBLE);
                spawnableTypes.Add(Bubble.EXPANDING_CLUSTER);
                break;
            case 13:
                spawnableTypes.Add(Bubble.HELIX_CLUSTER);
                break;
            default:
                break;
        }

        if (spawnableTypes.Count > oldSize) {
            populateMapping();
        }
    }

    // Shuffles the indices in the type mapping to increase chance of harder bubbles spawning
    void shuffleTypeMapping() {

    }

    // Spawns 4 simple bubbles in a diamond shaped cluster
    void spawnSimpleCluster(Vector3 center, float speed) {
        instantiateBubble(center + new Vector3(-1, 0, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
        instantiateBubble(center + new Vector3(1, 0, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
        instantiateBubble(center + new Vector3(0, -1, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
        instantiateBubble(center + new Vector3(0, 1, 0), speed, Bubble.SIMPLE_BUBBLE, Quaternion.identity);
    }

    // Spawns double sine bubbles in a helix formation
    void spawnHelixCluster(Vector3 center, float speed) {
        GameObject one = instantiateBubble(center, speed, Bubble.DOUBLE_SINE_BUBBLE, Quaternion.identity);
        one.GetComponent<Bubble>().sineOffset = 0;
        one.GetComponent<Bubble>().amplitude = 0.05f;
        GameObject two = instantiateBubble(center, speed, Bubble.DOUBLE_SINE_BUBBLE, Quaternion.identity);
        two.GetComponent<Bubble>().sineOffset = 2 * Mathf.PI / 3.0f;
        two.GetComponent<Bubble>().amplitude = 0.05f;
        GameObject three = instantiateBubble(center, speed, Bubble.DOUBLE_SINE_BUBBLE, Quaternion.identity);
        three.GetComponent<Bubble>().sineOffset = 4 * Mathf.PI / 3.0f;
        three.GetComponent<Bubble>().amplitude = 0.05f;
    }

    // Spawns sine bubbles at different rotations so they expand and contract on the same point
    void spawnExpandingCluster(Vector3 center, float speed) {
        instantiateBubble(center, speed, Bubble.SINE_BUBBLE, Quaternion.identity);
        instantiateBubble(center, speed, Bubble.SINE_BUBBLE, Quaternion.Euler(0, 0, 120));
        instantiateBubble(center, speed, Bubble.SINE_BUBBLE, Quaternion.Euler(0, 0, 240));
    }
}