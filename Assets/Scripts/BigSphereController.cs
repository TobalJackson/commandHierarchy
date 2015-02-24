using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigSphereController : MonoBehaviour {
	public int speed;
	public int followerMinDistance;
	public int followerMaxDistance;
	public int visionRadius;

	GameObject[] mediumSpheres;
	GameObject[] targets;
	ArrayList myTargets;
	Dictionary<GameObject, int> coverage;
	ArrayList visibleTargets;
	Vector3 sphereForce;
	Rigidbody rigidSphere;
	public bool targetStateChanged;

	// Use this for initialization
	void Start () {
		myTargets = new ArrayList ();
		coverage = new Dictionary<GameObject, int> ();
		mediumSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		rigidSphere = GetComponent<Rigidbody> ();
		sphereForce = new Vector3 ();
		targets = GameObject.FindGameObjectsWithTag ("Target");
		foreach (GameObject target in targets) {
			myTargets.Add(target);
		}
		visibleTargets = new ArrayList ();
		targetStateChanged = false;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject uTarget in myTargets) {
			if (Mathf.Abs((uTarget.transform.position - this.transform.position).magnitude) < visionRadius && !uTarget.GetComponent<TargetController>().isDead()){
				if (!this.visibleTargets.Contains(uTarget)){
					visibleTargets.Add(uTarget);
					coverage.Add (uTarget, 0);
					targetStateChanged = true;
				}
			}
			else if (visibleTargets.Contains (uTarget)){
				visibleTargets.Remove (uTarget);
				coverage.Remove (uTarget);
				targetStateChanged = true;
			}
		}
	}
	void FixedUpdate(){
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		sphereForce.Set (h, 0f, v);
		rigidSphere.AddForce (sphereForce * speed * Time.fixedDeltaTime);
		if (targetInRange()) {
			if (targetStateChanged){
				foreach (GameObject target in visibleTargets){
					if (coverage[target] == 0 && !target.GetComponent<TargetController>().isDead()){
						GameObject medSphere = getAvailableMedSphere();
						if (medSphere != null){
							medSphere.GetComponent<MediumSphereController>().setDestination(target.transform.position);
							medSphere.GetComponent<MediumSphereController>().setIsEngaged (true);
							coverage[target] += 1;
						}
					}
				}
				targetStateChanged = false;
			}
			else foreach(GameObject medSphere in mediumSpheres) {
				if(!medSphere.GetComponent<MediumSphereController>().getIsEngaged()){
					medSphere.GetComponent<MediumSphereController>().setDestination(this.transform.position);
					//medSphere.GetComponent<MediumSphereController>().setIsEngaged (false);
				}
			}
		}
		else foreach(GameObject medSphere in mediumSpheres) {
			medSphere.GetComponent<MediumSphereController>().setDestination(this.transform.position);
			medSphere.GetComponent<MediumSphereController>().setIsEngaged (false);
		}
	}
	public bool targetInRange(){
		if (this.visibleTargets.Count == 0)	return false; 
		else return true;
	}
	GameObject getAvailableMedSphere(){
		foreach(GameObject medSphere in mediumSpheres){
			if (medSphere.GetComponent<MediumSphereController>().getIsEngaged() == false) return medSphere;
		}
		return null;
	}
}
