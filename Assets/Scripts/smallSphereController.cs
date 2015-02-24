using UnityEngine;
using System.Collections;

public class smallSphereController : MonoBehaviour {

	public int minFollowDistance;
	public int maxFollowDistance;
	public int speed;

	GameObject medSphereParent;
	Rigidbody smallSphereRigid;
	bool isEngaged;
	Vector3 targetLocation;
	GameObject[] medSpheres;


	// Use this for initialization
	void Start () {
		medSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		//attachToNearest (medSpheres);

		isEngaged = false;
		targetLocation = medSphereParent.transform.position;
		medSphereParent = this.transform.parent.gameObject;
		smallSphereRigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		pursueTarget ();
	}

	void pursueTarget(){
		Vector3 distanceToTarget = this.targetLocation - this.transform.position;
		if (distanceToTarget.magnitude < this.minFollowDistance) {
			smallSphereRigid.AddForce (distanceToTarget * speed * Time.fixedDeltaTime);
		}
		if (distanceToTarget.magnitude < this.maxFollowDistance) {
			smallSphereRigid.AddForce (-1 * distanceToTarget * speed * Time.fixedDeltaTime);
		}
	}
	public void setTargetLocation(Vector3 location){
		this.targetLocation = location;
	}
	public bool getHasParent(){
		if (this.medSphereParent == null)
			return false;
		else
			return true;
	}
	public void setParent(GameObject medSphere){
		medSphereParent = medSphere;
	}

	//void attachToNearest(GameObject[] spheres){
	//	float minDistance = 9999;
	//	foreach (GameObject sphere in spheres){
	//		float distance = Mathf.Abs ((sphere.transform.position - this.transform.position).magnitude);
	//		Debug.Log (distance);
	//		if (distance < minDistance){
	//			minDistance = distance;
	//			this.medSphereParent = sphere;
	//		}
	//	}
	//	Debug.Log (medSphereParent == null);
	//	this.medSphereParent.GetComponent<MediumSphereController> ().addSmallSphere (this.gameObject);
	//}
}
