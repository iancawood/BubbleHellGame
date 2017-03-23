using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    const float DEATH_ZONE = -10f; // Where the bubbles despawn, should be negative

    public const int SIMPLE_BUBBLE = 0;
    public const int SINE_BUBBLE = 1;
    public const int DOUBLE_SINE_BUBBLE = 2;
    public const int HOMING_BUBBLE = 3;

    public const int SIMPLE_CLUSTER = 4;
    public const int HELIX_CLUSTER = 5;
    public const int EXPANDING_CLUSTER = 5;

    public float speed = 15f;
    public int type = SIMPLE_BUBBLE;

    void Start ()
    {
        if (type == SIMPLE_BUBBLE)
        {
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (type == SINE_BUBBLE)
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
        else if (type == DOUBLE_SINE_BUBBLE)
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (type == HOMING_BUBBLE)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void Move ()
    {
        if (type == SIMPLE_BUBBLE) {
            transform.position += Vector3.back * Time.deltaTime * speed;
        }
        else if (type == SINE_BUBBLE) {
            transform.position += (Vector3.back * Time.deltaTime * speed);
            transform.position += (Vector3.up * Mathf.Sin (Time.time * 8.0f) * 0.25f);
        }
        else if (type == DOUBLE_SINE_BUBBLE) {
            transform.position += (Vector3.back * Time.deltaTime * speed);
            transform.position += (Vector3.up * Mathf.Cos(Time.time * 4.0f) * 0.15f);
            transform.position += (Vector3.right * Mathf.Sin(Time.time * 4.0f) * 0.15f);
        }
        else if (type == HOMING_BUBBLE)
        {
            Vector3 target = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.x, -50);
            target.x += 2 * Random.value - 1;
            target.y += 2 * Random.value - 1;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    void Update () {

        Move();

        if (transform.position.z < DEATH_ZONE) {
            Destroy(this.gameObject);
        }
    }
}
