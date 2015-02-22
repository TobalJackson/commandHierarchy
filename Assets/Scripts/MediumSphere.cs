using UnityEngine;
using System.Collections;

public class MediumSphere : MonoBehaviour {
	public int speed;
	public int minDistanceToOtherMedSpheres;

	GameObject[] otherMedSpheres;
	GameObject player;
	Vector3 distanceToPlayer;
	Vector3 distanceToMedSphere;
	int maxDistance;
	int minDistance;
	Rigidbody MSRigidBody;

	void Start () {
		otherMedSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		MSRigidBody = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag("Player");
		distanceToPlayer = player.transform.position - this.transform.position;
		maxDistance = player.GetComponent<SphereController>().followerMaxDistance;
		minDistance = player.GetComponent<SphereController> ().followerMinDistance;


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		foreach(GameObject sphere in otherMedSpheres){
			distanceToMedSphere = sphere.transform.position - this.transform.position;
			if (distanceToMedSphere.magnitude < this.minDistanceToOtherMedSpheres){
				MSRigidBody.AddForce(-1 * distanceToPlayer * speed * Time.fixedDeltaTime);
			}
		}

		distanceToPlayer = player.transform.position - this.transform.position;
		if (distanceToPlayer.magnitude < minDistance) {
			MSRigidBody.AddForce(-1 * distanceToPlayer * speed * Time.fixedDeltaTime);
		}
		if (distanceToPlayer.magnitude > maxDistance) {
			MSRigidBody.AddForce(distanceToPlayer * speed * Time.fixedDeltaTime);
		}
	}
}
