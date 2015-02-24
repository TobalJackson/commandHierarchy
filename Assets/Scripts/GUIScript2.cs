using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIScript2 : MonoBehaviour {
	List<GameObject> targets;
	GUIText text;

	// Use this for initialization
	void Start () {
		text = this.GetComponent<GUIText> ();
		targets = new List<GameObject> ();
		foreach (GameObject target in GameObject.FindGameObjectsWithTag("Target")) {
			targets.Add (target);
		}
	}
	
	// Update is called once per frame
	void Update () {
		text.text = getTargetStatusString ();
	}
	string getTargetStatusString(){
		string result = "";
		int count = 0;
		foreach (GameObject target in targets) {
			result += "Target " + count + " isDead: " + target.GetComponent<TargetController>().isDead () + "\n";
			count++;
		}
		return result;
	}
}
