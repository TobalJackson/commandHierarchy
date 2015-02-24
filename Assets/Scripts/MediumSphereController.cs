using UnityEngine;
using System.Collections;

public class MediumSphereController : MonoBehaviour {
	public int speed;
	public int minDistanceToOtherMedSpheres;
	public int visionRadius;

	GameObject[] otherMedSpheres;
	GameObject player;
	Vector3 distanceToPlayer;
	Vector3 distanceToMedSphere;
	int maxDistance;
	int minDistance;
	Rigidbody MSRigidBody;
	Vector3 targetLocation;
	Vector3 distanceToTarget;

	GameObject[] targets;
	ArrayList visibleTargets;

	//ArrayList mySmallSpheres;
	//public GameObject smallSphere1;
	//GameObject[] smallSpheres;
	//int maxFollowers;

	bool isEngaged;

	void Start () {
		//mySmallSpheres = new ArrayList ();
		otherMedSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		MSRigidBody = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag("Player");
		distanceToPlayer = player.transform.position - this.transform.position;
		maxDistance = player.GetComponent<BigSphereController>().followerMaxDistance;
		minDistance = player.GetComponent<BigSphereController> ().followerMinDistance;
		targets = GameObject.FindGameObjectsWithTag ("Target");
		visibleTargets = new ArrayList ();
		//maxFollowers = 1;
		//smallSpheres = GameObject.FindGameObjectsWithTag ("SmallSphere");
		//mySmallSpheres.Add (smallSphere1.gameObject);

		//if (mySmallSpheres.Count < maxFollowers) {
		//	foreach (GameObject sphere in smallSpheres){
		//		if (!sphere.GetComponent<smallSphereController>().getHasParent()){
		//			sphere.GetComponent<smallSphereController>().setParent(this.gameObject);
		//			this.mySmallSpheres.Add (sphere);
		//		}
		//	}
		//}
	}
	void Awake(){

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		//foreach(GameObject sphere in otherMedSpheres){
		//	distanceToMedSphere = sphere.transform.position - this.transform.position;
		//	if (distanceToMedSphere.magnitude < this.minDistanceToOtherMedSpheres){
		//		MSRigidBody.AddForce(-1f * distanceToMedSphere * speed * Time.fixedDeltaTime);
		//	}
		//}

		pursueTarget();
		updateTargetVisibility ();
		if (targetInRange()) {
			foreach (GameObject target in visibleTargets){
				target.GetComponent<TargetController>().takeDamage(10);
			}
			//if (mySmallSpheres.Count != 0){
			//	foreach (GameObject sphere in mySmallSpheres){
			//		this.dispatchTargetLocation(sphere, targetLocation);
			//	}
			//}
		}
		//else if(mySmallSpheres.Count != 0){
		//	foreach (GameObject sphere in mySmallSpheres){
		//		this.dispatchTargetLocation(sphere, this.transform.position);
		//	}
		//}
	}
	public bool getIsEngaged(){
		return this.isEngaged;
	}
	public void setIsEngaged(bool state){
		this.isEngaged = state;
	}

	public void setDestination(Vector3 location){
		targetLocation = location;
	}
	void pursueTarget(){
		if (targetLocation != null) {
			distanceToTarget = targetLocation - this.transform.position;
			if (distanceToTarget.magnitude < minDistance) {
				MSRigidBody.AddForce(-1 * distanceToTarget * speed * Time.fixedDeltaTime);
			}
			if (distanceToTarget.magnitude > maxDistance) {
				MSRigidBody.AddForce(distanceToTarget * speed * Time.fixedDeltaTime);
			}
		}
	}
	//public void addSmallSphere(GameObject sphere){
	//	this.mySmallSpheres.Add (sphere);
	//}
	void dispatchTargetLocation(GameObject smallSphere, Vector3 location){
		smallSphere.GetComponent<smallSphereController> ().setTargetLocation (location);
	}
	public bool targetInRange(){
		if (this.visibleTargets.Count == 0)	return false; 
		else return true;
	}
	void updateTargetVisibility(){
		foreach (GameObject uTarget in targets) {
			if (Mathf.Abs((uTarget.transform.position - this.transform.position).magnitude) < visionRadius){
				if (!this.visibleTargets.Contains(uTarget)){
					visibleTargets.Add(uTarget);
				}
			}
			else if (visibleTargets.Contains (uTarget)){
				visibleTargets.Remove (uTarget);
			}
		}
	}
}
