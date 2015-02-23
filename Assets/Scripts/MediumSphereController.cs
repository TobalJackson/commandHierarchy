using UnityEngine;
using System.Collections;

public class MediumSphereController : MonoBehaviour {
	public int speed;
	public int minDistanceToOtherMedSpheres;

	GameObject[] otherMedSpheres;
	GameObject player;
	Vector3 distanceToPlayer;
	Vector3 distanceToMedSphere;
	int maxDistance;
	int minDistance;
	Rigidbody MSRigidBody;
	Vector3 targetLocation;
	Vector3 distanceToTarget;

	bool isEngaged;

	void Start () {
		otherMedSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		MSRigidBody = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag("Player");
		distanceToPlayer = player.transform.position - this.transform.position;
		maxDistance = player.GetComponent<BigSphereController>().followerMaxDistance;
		minDistance = player.GetComponent<BigSphereController> ().followerMinDistance;


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
}
