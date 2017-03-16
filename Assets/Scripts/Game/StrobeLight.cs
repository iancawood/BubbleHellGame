using UnityEngine;
using System.Collections;

public class StrobeLight : MonoBehaviour {
    // A simple point light that moves through the tunnel to make a ring effect.

    float SPEED = 50f;
    float RESET_AT = -50f;
    float RESET_TO = 250f;

    void Update() {
        transform.position += Vector3.back * Time.deltaTime * SPEED;

        if (transform.position.z < RESET_AT) {
            transform.position = new Vector3(0, 0, RESET_TO);
        }
    }
}
