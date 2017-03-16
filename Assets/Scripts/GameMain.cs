using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Call ad banner
		AdManager.Instance.ShowBanner ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
