using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    float SPEED = 15f;
    float DEATH_ZONE = -10f; // Where the bubbles despawn, should be negative

    void Update () {
        transform.position += Vector3.back * Time.deltaTime * SPEED;

        if (transform.position.z < DEATH_ZONE) {
            Destroy(this.gameObject);
        }
    }
}
