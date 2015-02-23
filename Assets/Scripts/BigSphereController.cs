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
	Dictionary<GameObject, int> coverage;
	ArrayList visibleTargets;
	Vector3 sphereForce;
	Rigidbody rigidSphere;

	// Use this for initialization
	void Start () {
		coverage = new Dictionary<GameObject, int> ();
		mediumSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		rigidSphere = GetComponent<Rigidbody> ();
		sphereForce = new Vector3 ();
		targets = GameObject.FindGameObjectsWithTag ("Target");
		visibleTargets = new ArrayList ();
		Debug.Log (targets.Length);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject uTarget in targets) {
			if (Mathf.Abs((uTarget.transform.position - this.transform.position).magnitude) < visionRadius){
				if (!this.visibleTargets.Contains(uTarget)){
					visibleTargets.Add(uTarget);
					coverage.Add (uTarget, 0);
				}
			}
			else if (visibleTargets.Contains (uTarget)){
				visibleTargets.Remove (uTarget);
				coverage.Remove (uTarget);
			}
		}
	}
	void FixedUpdate(){
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		sphereForce.Set (h, 0f, v);
		rigidSphere.AddForce (sphereForce * speed * Time.fixedDeltaTime);

		foreach (GameObject medSphere in mediumSpheres) {
			if (!targetInRange()){
				medSphere.GetComponent<MediumSphereController>().setDestination(this.transform.position);
				medSphere.GetComponent<MediumSphereController>().setIsEngaged (false);
			}
			else {
				foreach (GameObject target in visibleTargets){
					if (coverage[target] == 0){
						if (!medSphere.GetComponent<MediumSphereController>().getIsEngaged()){ 
							medSphere.GetComponent<MediumSphereController>().setDestination(target.transform.position);
							medSphere.GetComponent<MediumSphereController>().setIsEngaged (true);
							coverage[target] += 1;
						}
					}
					else if (!medSphere.GetComponent<MediumSphereController>().getIsEngaged()){
						medSphere.GetComponent<MediumSphereController>().setDestination(this.transform.position);
					}
				}
			}
		}
	}
	public bool targetInRange(){
		if (this.visibleTargets.Count == 0)	return false; 
		else return true;
	}
}
