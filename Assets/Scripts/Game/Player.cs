using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    float SPEED = 5f;
    float COLLISION_BIAS = 2f;

    bool canMove = false;

    bool vibrate;

    void Start() {

    }

    void Update() {
        if (canMove) {
#if UNITY_EDITOR
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
            transform.position += move * SPEED * Time.deltaTime;
        }
#endif
        }
    }

    // Anticipate a collision with the pipe before it happens. Doesnt work though. Need to lerp transform.position + move.
    bool pipeCollision(Vector3 move) {
        return (Mathf.Pow(transform.position.x + move.x, 2) + Mathf.Pow(transform.position.y + move.y, 2)) >= Mathf.Pow(PipeBuilder.pipeRadius - COLLISION_BIAS, 2);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Bubble") {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<MainMenu>().gameEnd();
            if(vibrate) {
                Handheld.Vibrate();
            }
        }
    }

    void reset() {
        //transform.position = Vector3.zero;
    } 

    public void disable() {
        canMove = false;
        reset();
    }

    public void enable() {
        canMove = true;
    }

    //toggle vibrate
    public void toggleVibrate()
    {
        vibrate = !vibrate;
    }
}
