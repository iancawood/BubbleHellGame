// This script handles touch events and sends messages to other game objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TouchManager : MonoBehaviour {

	public static TouchManager Instance { set; get;} //singleton
	public GameObject recipient;

	private void Start(){
		Instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void Update (){
		if (!recipient) { recipient = GameObject.FindGameObjectWithTag ("Player"); }

	#if UNITY_EDITOR
		Vector3 mp = Input.mousePosition;
		mp.z = 10;
		Vector3 v = Camera.main.ScreenToWorldPoint (mp);

		if (recipient){
			if (Input.GetMouseButtonDown(0)){
				recipient.SendMessage ("OnTouchDown", v);
			} else if (Input.GetMouseButtonUp(0)){
				recipient.SendMessage ("OnTouchUp", v);
			} else if(Input.GetMouseButton(0)) {
				recipient.SendMessage ("OnTouchHold", v);
			}
		}
	#endif
		// For mobile
		if (Input.touchCount > 0 && recipient) {
			foreach (Touch touch in Input.touches) {
				Vector3 tp = touch.position;
				tp.z = 10;
				Vector3 w = Camera.main.ScreenToWorldPoint (tp);
				if (touch.phase == TouchPhase.Began) {
					recipient.SendMessage ("OnTouchDown", w);
				} else if (touch.phase == TouchPhase.Ended) {
					recipient.SendMessage ("OnTouchUp", w);
				} else if (touch.phase == TouchPhase.Moved) {
					recipient.SendMessage ("OnTouchHold", w);
				} else {
					recipient.SendMessage ("OnTouchCancelled", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
