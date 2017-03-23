using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject bubble;
    public Transform bubbleJar;

    float spawnerZPos;

    float spawnRate = 0.7f; // delay between bubbles spawns
    float nextSpawn = 0;

    const float PIPE_THICKNESS = 2f;

    const float SPEED_MULTIPLIER = 0.5f;
    const float SPAWN_MULTIPLIER = 0.05f;
    const float MAX_SPAWN_RATE = 0.05f;

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
    int numDice = 3;
    int currentType = 0;
    int[] spawnableTypes = {
        Bubble.SIMPLE_BUBBLE,
        Bubble.SINE_BUBBLE,
        Bubble.DOUBLE_SINE_BUBBLE
    };

    void Start () {
        Collider collider = this.GetComponent<Collider>();

        spawnerZPos = collider.bounds.center.z;
        spawnerXRange = new Range(collider.bounds.min.x, collider.bounds.max.x);
        spawnerYRange = new Range(collider.bounds.min.y, collider.bounds.max.y);

        speedRange = new Range(5, 25);
        typeRange = new Range(1, 6);
        pipeRange = new Range(0, PipeBuilder.pipeRadius);

        populateMapping();

        Debug.Log(typeMapping);
    }

	void Update () {
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0) {
            nextSpawn = spawnRate;
            spawnBubble();
        }
    }

    void spawnBubble() {
        Vector3 position = selectPosition();

        GameObject b = (GameObject)Instantiate(bubble, position, Quaternion.identity);
        b.transform.parent = bubbleJar;

        Bubble bubbleScript = b.GetComponent<Bubble>();
        bubbleScript.speed = selectSpeed();
        bubbleScript.type = selectBubbleType();
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
        speedRange.min += SPEED_MULTIPLIER;
        speedRange.max += SPEED_MULTIPLIER;

        if ((spawnRate - SPAWN_MULTIPLIER) >= MAX_SPAWN_RATE) {
            spawnRate -= SPAWN_MULTIPLIER;
        }

        // also, rearrange the configuration of the type map
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
        if (currentType >= spawnableTypes.Length) {
            currentType = 0;
        }

        typeMapping.Add(index, spawnableTypes[currentType++]);
    }
}
