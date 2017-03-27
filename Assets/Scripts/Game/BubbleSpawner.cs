using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject bubble;
    public Transform bubbleJar;

    [System.Serializable]
    public class BubbleType {
        public string name;
        public int type;
        public int level;
        public int spawnChance;
        public int growthRate;

        public BubbleType(BubbleType copy) {
            name = copy.name;
            type = copy.type;
            level = copy.level;
            spawnChance = copy.spawnChance;
            growthRate = copy.growthRate;
        }
    }
    public List<BubbleType> bubbleTypes;

    [Header("Speed")]
    public int initialMinSpeed = 5;
    public int initialMaxSpeed = 25;
    public float speedIncrement = 0.5f;
    [Header("Spawn Rate")]
    public float initialSpawnRate = 3f; // delay between bubbles spawns
    public float maxSpawnRate = 0.05f;
    public float spawnIncrement = 0.05f;

    float spawnerZPos;

    float nextSpawn = 0;
    bool spawning = false;

    const float PIPE_THICKNESS = 2f;
    const int HUNDRED_PERCENT = 100;

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
    Range spawnerXRange;
    Range spawnerYRange;
    Range pipeRange;

    List<BubbleType> spawnableTypes = new List<BubbleType>();
    float spawnRate;

    void Start () {
        Collider collider = this.GetComponent<Collider>();

        spawnerZPos = collider.bounds.center.z;
        spawnerXRange = new Range(collider.bounds.min.x, collider.bounds.max.x);
        spawnerYRange = new Range(collider.bounds.min.y, collider.bounds.max.y);

        speedRange = new Range(initialMinSpeed, initialMaxSpeed);
        pipeRange = new Range(0, PipeBuilder.pipeRadius);

        spawnRate = initialSpawnRate;

        reset();
    }

	void Update () {
        if (spawning) {
            nextSpawn -= Time.deltaTime;

            if (nextSpawn <= 0) {
                nextSpawn = spawnRate;
                spawnBubble();
            }
        }
    }

    // Reset bubble spawner to initial state
    void reset() {
        spawnableTypes = new List<BubbleType>();
        chooseSpawnableTypes(1);
        nextSpawn = 0;
        speedRange = new Range(initialMinSpeed, initialMaxSpeed);
        spawnRate = initialSpawnRate;
    }

    public void disable() {
        spawning = false;
    }

    public void enable() {
        spawning = true;
        reset();
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
            spawnSimpleCluster(position, speed, type);
        }
        else if (type == Bubble.HELIX_CLUSTER) {
            spawnHelixCluster(position, speed, type);
        } 
        else if (type == Bubble.EXPANDING_CLUSTER) {
            spawnExpandingCluster(position, speed, type);
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
        int outcome = Random.Range(0, 100);
        int type = Bubble.SIMPLE_BUBBLE;

        foreach (BubbleType bubbleType in spawnableTypes) {
            if (outcome <= bubbleType.spawnChance) {
                type = bubbleType.type;
                break;
            }
        }

        return type;
    }

    // Randomly determine speed of bubble
    float selectSpeed() {
        return Random.Range(speedRange.min, speedRange.max);
    }

    // Called upon the level increasing and increase the difficulty of the bubbles spawned
    void increaseDifficulty(int level) {
        speedRange.min += speedIncrement;
        speedRange.max += speedIncrement;

        if ((spawnRate - spawnIncrement) >= maxSpawnRate) {
            spawnRate -= spawnIncrement;
        }

        bool typeAdded = chooseSpawnableTypes(level);

        if (!typeAdded) {
            increaseSpawnChance();
        }
    }

    // Adds a bubble type to the front list of the list and pushes other spawn chances to a higher value
    // This guarantees that the list is sorted in ascending order based on spawn chance
    void addToSpawnableTypes(BubbleType BubbleType) {
        BubbleType copy = new BubbleType(BubbleType);

        foreach (BubbleType bubbleType in spawnableTypes) {
            bubbleType.spawnChance += copy.spawnChance;
        }

        spawnableTypes.Insert(0, copy);
    }

    // Determines what bubbles will exist at each level.
    bool chooseSpawnableTypes(int level) {
        bool typeAdded = false;
        foreach (BubbleType bubbleType in bubbleTypes) {
            if (bubbleType.level == level) {
                addToSpawnableTypes(bubbleType);
                typeAdded = true;
            }
        }
        return typeAdded;
    }

    void increaseSpawnChance() {
        int totalIncrease = 0;

        foreach (BubbleType bubbleType in spawnableTypes) {
            totalIncrease += bubbleType.growthRate;
            bubbleType.spawnChance += totalIncrease;            
        }
    }

    // Spawns 4 simple bubbles in a diamond shaped cluster
    void spawnSimpleCluster(Vector3 center, float speed, int type) {
        instantiateBubble(center + new Vector3(-1, 0, 0), speed, type, Quaternion.identity);
        instantiateBubble(center + new Vector3(1, 0, 0), speed, type, Quaternion.identity);
        instantiateBubble(center + new Vector3(0, -1, 0), speed, type, Quaternion.identity);
        instantiateBubble(center + new Vector3(0, 1, 0), speed, type, Quaternion.identity);
    }

    // Spawns double sine bubbles in a helix formation
    void spawnHelixCluster(Vector3 center, float speed, int type) {
        GameObject one = instantiateBubble(center, speed, type, Quaternion.identity);
        one.GetComponent<Bubble>().sineOffset = 0;
        one.GetComponent<Bubble>().amplitude = 0.05f;
        GameObject two = instantiateBubble(center, speed, type, Quaternion.identity);
        two.GetComponent<Bubble>().sineOffset = 2 * Mathf.PI / 3.0f;
        two.GetComponent<Bubble>().amplitude = 0.05f;
        GameObject three = instantiateBubble(center, speed, type, Quaternion.identity);
        three.GetComponent<Bubble>().sineOffset = 4 * Mathf.PI / 3.0f;
        three.GetComponent<Bubble>().amplitude = 0.05f;
    }

    // Spawns sine bubbles at different rotations so they expand and contract on the same point
    void spawnExpandingCluster(Vector3 center, float speed, int type) {
        instantiateBubble(center, speed, type, Quaternion.identity);
        instantiateBubble(center, speed, type, Quaternion.Euler(0, 0, 120));
        instantiateBubble(center, speed, type, Quaternion.Euler(0, 0, 240));
    }
}