using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigSphereController : MonoBehaviour {
	public int speed;
	public int followerMinDistance;
	public int followerMaxDistance;
	public int visionRadius;

	GameObject[] mediumSpheres; 			//list of all medium spheres
	GameObject[] targets;					//list of all target objects
	ArrayList myTargets;					//mutable list of targets
	Dictionary<GameObject, int> coverage;	//coverage of each target
	ArrayList visibleTargets;				//list of targets BigSphere can see
	Vector3 sphereForce;					//input-controlled vector 
	Rigidbody rigidSphere;					//Bigspheres rigidsphere


	// Use this for initialization
	void Start () {
		myTargets = new ArrayList ();											//initialize
		coverage = new Dictionary<GameObject, int> ();							//initialize
		mediumSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");		//initialize/populate
		rigidSphere = GetComponent<Rigidbody> ();								//initialize
		sphereForce = new Vector3 ();											//initialize
		targets = GameObject.FindGameObjectsWithTag ("Target");					//initialize/populate
		foreach (GameObject target in targets) {
			myTargets.Add(target);		//initialize
		}
		visibleTargets = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			if (Time.timeScale == 1.0f){
				Time.timeScale = 0f;
			}
			else Time.timeScale = 1.0f;
		}
		if (Input.GetKeyDown(KeyCode.Q)){
			Time.timeScale *= 0.5f;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			Time.timeScale /= 0.5f;
		}
		foreach (GameObject uTarget in myTargets) {
			if (Mathf.Abs((uTarget.transform.position - this.transform.position).magnitude) < visionRadius && !uTarget.GetComponent<TargetController>().isDead()){
				if (!this.visibleTargets.Contains(uTarget)){
					Debug.Log ("BigSphere can now see target " + uTarget.gameObject.GetInstanceID() + "!");
					visibleTargets.Add(uTarget);				//scan all targets to check which ones are within "visionRadius", add 
																	//them to "visibleTargets" list.
					coverage.Add (uTarget, 0);					//make a new "coverage" dict entry for the target with value of 0, 
																	//indicating nobody is covering it.
				}
			}
			else if (visibleTargets.Contains (uTarget)){
				visibleTargets.Remove (uTarget);				//if targets outside visionRadius, remove them from "visibleTargets".
				coverage.Remove (uTarget);						//and remove from "coverage"
			}
		}
	}
	void FixedUpdate(){
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		sphereForce.Set (h, 0f, v);											//get user input, set vector up
		rigidSphere.AddForce (sphereForce * speed * Time.fixedDeltaTime);	//move BigSphere
		if (targetInRange()) {												//if visibleTargets.Count != 0
				foreach (GameObject target in visibleTargets){															//for every visible target
					if (coverage[target] == 0 && !target.GetComponent<TargetController>().isDead()){					//if it doesn't have coverage, and isn't dead
						GameObject medSphere = getAvailableMedSphere();													//get a mediumSphere that isn't engaged
						if (medSphere != null){
							Debug.Log("BigSphere orders mediumSphere " + medSphere.gameObject.GetInstanceID() + " to attack " + target.gameObject.GetInstanceID() + "!");
							medSphere.GetComponent<MediumSphereController>().setDestination(target.transform.position);	//set that mediumSphere's destination to the target location
							medSphere.GetComponent<MediumSphereController>().setIsEngaged (true);						//and consider it engaged.
							coverage[target] += 1;																		//and set that target's coverage = 1
						}
					}
				}

			foreach(GameObject medSphere in mediumSpheres) {															//for every medium sphere
				if(medSphere.GetComponent<MediumSphereController>().getIsEngaged() == false){							//if it's not engaged
					medSphere.GetComponent<MediumSphereController>().setDestination(this.transform.position);			//follow the big sphere.

					//medSphere.GetComponent<MediumSphereController>().setIsEngaged (false);
				}
			}
		}
		else {
			foreach(GameObject medSphere in mediumSpheres) {												//If there are no targets in range
			medSphere.GetComponent<MediumSphereController>().setDestination(this.transform.position);		//Have every medium sphere follow BigSphere
			medSphere.GetComponent<MediumSphereController>().setIsEngaged (false);							//Set them to disengaged.
			}
			Debug.Log("BigSphere orders mediumSpheres to Follow!");
		}
	}
	public bool targetInRange(){							//are there any visible targets?
		if (this.visibleTargets.Count == 0)	return false; 
		else return true;
	}
	public void removeTarget(GameObject target){			//get rid of targets that BigSphere cares about
		if (myTargets.Contains(target)) myTargets.Remove (target);
		if (visibleTargets.Contains(target)) visibleTargets.Remove (target);
	}
	GameObject getAvailableMedSphere(){						//find the first available mediumSphere that isn't engaged
		foreach(GameObject medSphere in mediumSpheres){
			if (medSphere.GetComponent<MediumSphereController>().getIsEngaged() == false) return medSphere;
		}
		return null;
	}
	public string followerStateString(){					//print mediumSphere's status
		string status = "";
		int count = 0;
		foreach(GameObject medSphere in mediumSpheres){
			status += "MediumSphere " + count + " engaged: " +medSphere.GetComponent<MediumSphereController>().getIsEngaged().ToString() + "\n" +
				"\t" + "Targets In View: " + medSphere.GetComponent<MediumSphereController>().getTargetsInRange() + "\n";
			count++;
		}
		return status;
	}
}
