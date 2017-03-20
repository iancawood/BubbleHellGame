using UnityEngine;
using System.Collections.Generic;

public class PipeBuilder : MonoBehaviour {
    // This script is not meant to be attached to any specific object, but should be attached to a global persistent object.

    public GameObject pipeSegment;
    public Transform pipe;

    int NUM_PIPES = 10; // The number of pipes to exist on screeen at a given time.

    public static float pipeLength;
    public static float pipeRadius;

    void Start() {
        Renderer renderer = pipeSegment.GetComponent<Renderer>();
        pipeLength = renderer.bounds.size.z;
        pipeRadius = renderer.bounds.extents.x;

        for (int i = 0; i < NUM_PIPES; i++) {
            createPipe(new Vector3(0, 0, pipeLength * (i + 0.5f)));
        }
    }

    void createPipe(Vector3 position) {
        GameObject p = Instantiate(pipeSegment, position, Quaternion.identity) as GameObject;
        p.transform.parent = pipe;
    }

    // Triggered by a destroyed pipe. Creates a new pipe to replace it.
    void replacePipe(float offset) {
        createPipe(new Vector3(0, 0, pipeLength * (NUM_PIPES - 0.5f) + offset));
    }

    void changeTheme() {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeSegment");

        foreach (GameObject pipe in pipes) {
            //pipe.GetComponent<Material>().DoSomething()
        }
    }
}
