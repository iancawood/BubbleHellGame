using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject bubble;
    public Transform bubbleJar;

    float spawnerZPos;

    float spawnRate = 0.3f; // delay between bubbles spawns
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

	void Start () {
        Collider collider = this.GetComponent<Collider>();

        spawnerZPos = collider.bounds.center.z;
        spawnerXRange = new Range(collider.bounds.min.x, collider.bounds.max.x);
        spawnerYRange = new Range(collider.bounds.min.y, collider.bounds.max.y);

        speedRange = new Range(5, 25);
        typeRange = new Range(1, 6);

        pipeRange = new Range(0, PipeBuilder.pipeRadius);
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
        int type = 1;

        float rand1 = Random.Range(typeRange.min, typeRange.max);
        float rand2 = Random.Range(typeRange.min, typeRange.max);

        float randSum = rand1 + rand2;

        // need some sort of mapping from randSum to different things

        return type;
    }

    float selectSpeed() {
        return Random.Range(speedRange.min, speedRange.max);
    }

    void increaseDifficulty(int level) {
        // increase speed as a function of the level

        // also, reallange the configuration of the type map

        // also, increase spawn rate
    }
}
