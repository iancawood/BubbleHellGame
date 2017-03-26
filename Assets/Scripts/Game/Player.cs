﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public float speed = 5f;
    float COLLISION_BIAS = 1f;
	float radius;

	public List<Vector3> points;

    void Start() {
		points.Add (new Vector3(0, 0, 10)); 
		points.Add (new Vector3(0, 0, 10));
    }

    void Update() {
		// move in the direction of last two points
		if(points.Count >=2){
			Vector3 move = points[points.Count-1] - points[points.Count-2];
            if (!pipeCollision(move)) {
                transform.position += move * speed * Time.deltaTime;
            }
        }
    }

    // Anticipate a collision with the pipe before it happens. Doesnt work though. Need to lerp transform.position + move.
    bool pipeCollision(Vector3 move) {
        return (Mathf.Pow(transform.position.x + move.x, 2) + Mathf.Pow(transform.position.y + move.y, 2)) >= Mathf.Pow(PipeBuilder.pipeRadius - COLLISION_BIAS, 2);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Bubble") {
            Debug.Log("Ded");
        }
    }

	//TOUCH COMMANDS
	public void OnTouchDown(Vector3 point){
		points.Clear ();
		points.Add (point);
	}

	public void OnTouchUp(Vector3 point){
		points.Add (point);
		//only ever want 2 points in the list for efficiency
		if (points.Count > 2) { points.RemoveAt (0); } 
	}

	public void OnTouchHold(Vector3 point){
		points.Add (point);
		//only ever want 2 points in the list for efficiency
		if (points.Count > 2) { points.RemoveAt (0); }
	}
}
