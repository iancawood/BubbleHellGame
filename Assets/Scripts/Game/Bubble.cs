using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    int type = 1;

    float SPEED = 15f;
    float DEATH_ZONE = -10f; // Where the bubbles despawn, should be negative

    void Start ()
    {
        if (type == 0)
        {
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (type == 1)
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
        else if (type == 2)
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (type == 3)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void Move ()
    {
        if (type == 0) {
            transform.position += Vector3.back * Time.deltaTime * SPEED;
        }
        else if (type == 1) {
            transform.position += (Vector3.back * Time.deltaTime * SPEED);
            transform.position += (Vector3.up * Mathf.Sin (Time.time * 8.0f) * 0.25f);
        }
        else if (type == 2) {
            transform.position += (Vector3.back * Time.deltaTime * SPEED);
            transform.position += (Vector3.up * Mathf.Cos(Time.time * 4.0f) * 0.15f);
            transform.position += (Vector3.right * Mathf.Sin(Time.time * 4.0f) * 0.15f);
        }
        else if (type == 3)
        {
            Vector3 target = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.x, -50);
            target.x += 2 * Random.value - 1;
            target.y += 2 * Random.value - 1;
            //transform.position += (Vector3.back * Time.deltaTime * SPEED);
            transform.position = Vector3.MoveTowards(transform.position, target, SPEED * Time.deltaTime);
        }
    }

    void Update () {

        Move();

        if (transform.position.z < DEATH_ZONE) {
            Destroy(this.gameObject);
        }
    }
}
