// This script handles touch events and sends messages to other game objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchManager : MonoBehaviour {

	public static TouchManager Instance { set; get;} //singleton

	private void Start(){
		Instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void Update () {
	// For testing in unity editor
	#if UNITY_EDITOR
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				GameObject recipient = hit.transform.gameObject;

				if (Input.GetMouseButtonDown(0)) {
					recipient.SendMessage ("OnTouchDown", hit.point);
				}
				if (Input.GetMouseButtonUp(0)) {
					recipient.SendMessage ("OnTouchUp", hit.point);
				}
				if (Input.GetMouseButton(0)) {
					recipient.SendMessage ("OnTouchHold", hit.point);
				}
			}
		}
	#endif
		// For mobile
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit;

				// 4 phases of a touch event
				if (Physics.Raycast (ray, out hit)) {
					GameObject recipient = hit.transform.gameObject;

					if (touch.phase == TouchPhase.Began) {
						recipient.SendMessage ("OnTouchDown", hit.point);
					}
					if (touch.phase == TouchPhase.Ended) {
						recipient.SendMessage ("OnTouchUp", hit.point);
					}
					if (touch.phase == TouchPhase.Moved) {
						recipient.SendMessage ("OnTouchHold", hit.point);
					}
					if (touch.phase == TouchPhase.Canceled) {
						recipient.SendMessage ("OnTouchCancelled", SendMessageOptions.DontRequireReceiver);
					}
				}

			}
		}
	}
}
