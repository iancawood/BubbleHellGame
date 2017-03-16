using UnityEngine;
using System.Collections;

public class PipeSegment : MonoBehaviour {
    // This script is meant to be attached to the PipeSegment object.

    float SPEED = 5f;
    float DEATH_ZONE = -5f; // Where the pipes despawn, should be negative
    string PIPE_BUILDER_TAG = "PipeBuilder";

    float pipeLength;

    bool appQuit = false;

    void Start() {
        pipeLength = this.GetComponent<Renderer>().bounds.size.z;
    }

    // Move constant. Despawn if object moves behind camera.
    void Update() {
        this.transform.position += Vector3.back * Time.deltaTime * SPEED;

        if (transform.position.z < DEATH_ZONE - pipeLength / 2f) {
            Destroy(this.gameObject);
        }
    }

    // Called upon game being closed and needed to avoid testing bugs in the editor.
    void OnApplicationQuit() {
        appQuit = true;
    }

    // Called upon game object being destroyed and tells builder to create another pipe.
    void OnDestroy() {
        if (!appQuit) {
            GameObject.FindGameObjectWithTag(PIPE_BUILDER_TAG).SendMessage("replacePipe", DEATH_ZONE);
        }
    }
}
