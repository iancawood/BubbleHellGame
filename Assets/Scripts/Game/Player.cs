using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    float SPEED = 5f;

    void Update() {
#if UNITY_EDITOR
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * SPEED * Time.deltaTime;
#endif
    }
}
