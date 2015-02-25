using UnityEngine;
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
		text.text = "Commander can See Targets?: " + bigSphere.GetComponent<BigSphereController> ().targetInRange ().ToString () + 
				"\n\n" + bigSphere.GetComponent<BigSphereController>().followerStateString();
	}
}
