using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject bubble;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float zPos;

    float spawnRate = 0.5f; // delay between bubbles spawns
    float nextSpawn = 0;

	void Start () {
        Collider collider = this.GetComponent<Collider>();
        xMin = collider.bounds.min.x;
        xMax = collider.bounds.max.x;
        yMin = collider.bounds.min.y;
        yMax = collider.bounds.max.y;
        zPos = collider.bounds.center.z;
    }

	void Update () {
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0) {
            nextSpawn = spawnRate;
            spawnBubble();
        }
    }

    void spawnBubble() {
        Vector3 position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), zPos);

        Instantiate(bubble, position, Quaternion.identity);
    }
}
