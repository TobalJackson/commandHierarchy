﻿using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	GameObject bigSphere;
	GUIText text;

	void Start () {
		bigSphere = GameObject.FindGameObjectWithTag ("Player");
		text = GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = bigSphere.GetComponent<BigSphereController> ().targetInRange ().ToString () + 
			"\n" + bigSphere.GetComponent<BigSphereController>().targetStateChanged.ToString () +
				"\n" + bigSphere.GetComponent<BigSphereController>().followerStateString();
	}
}
