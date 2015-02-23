using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {
	Quaternion rotation;
	// Use this for initialization
	void Start () {
		rotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = rotation;
	}
}
