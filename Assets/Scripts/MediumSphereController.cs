﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MediumSphereController : MonoBehaviour {
	public int speed;
	public int minDistanceToOtherMedSpheres;
	public int visionRadius;

	GameObject[] otherMedSpheres;		//list of other fellow medium spheres
	GameObject player;					//reference to BigSphere
	Vector3 distanceToPlayer;			//distance to player
	Vector3 distanceToMedSphere;		//distance to other medium spheres
	int maxDistance;					//maximum distance allowed from BigSphere
	int minDistance;					//minimum distance allowed from BigSphere
	Rigidbody MSRigidBody;				//MediumSphere's RigidBody
	Vector3 targetLocation;				//Target's Location
	Vector3 distanceToTarget;			//Target's distance

	float timeSinceLastSeenEnemy;		//absolute time since last seen enemy
		
	List<GameObject> targets;			//list of targets
	ArrayList visibleTargets;			//list of visible targets

	bool canAttack;						//can I attack?
	float lastAttack;					//time of last attack
	//ArrayList mySmallSpheres;
	//public GameObject smallSphere1;
	//GameObject[] smallSpheres;
	//int maxFollowers;

	bool isEngaged;						//Am I engaged

	void Start () {
		timeSinceLastSeenEnemy = 0;														//initialize

		canAttack = true;																//initialize
		targets = new List<GameObject> ();
		//mySmallSpheres = new ArrayList ();
		otherMedSpheres = GameObject.FindGameObjectsWithTag ("MediumSphere");
		MSRigidBody = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag("Player");							//Get reference to BigSphere
		distanceToPlayer = player.transform.position - this.transform.position;			//Set distance to player
		maxDistance = player.GetComponent<BigSphereController>().followerMaxDistance;	//Set/Get max/min distance to player
		minDistance = player.GetComponent<BigSphereController> ().followerMinDistance;
		targets.AddRange(GameObject.FindGameObjectsWithTag ("Target"));					//populate list of targets
		visibleTargets = new ArrayList ();												//initialize visible targets to empty
		lastAttack = 0;																	//initialize last attack time to 0
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
		foreach(GameObject sphere in otherMedSpheres){												//keep distance from other medium spheres.
			distanceToMedSphere = sphere.transform.position - this.transform.position;
			if (distanceToMedSphere.magnitude < this.minDistanceToOtherMedSpheres){
				MSRigidBody.AddForce(-1f * distanceToMedSphere * speed * Time.fixedDeltaTime);
			}
		}

		pursueTarget();																		//go toward whatever the target location is (Player/Enemy)
		updateTargetVisibility ();															//recalculate what is visible, what is not
		if (targetInRange()) {																//if visibleTargets.Count != 0
			timeSinceLastSeenEnemy = Time.time;												//update time since last seen enemy
			foreach (GameObject target in visibleTargets){									//for every visible target...
				if (canAttack){																//if attack timer cooled down...
					lastAttack= Time.time;													//set attack cooldown
					target.GetComponent<TargetController>().takeDamage(10);					//attack the target for 10 damage.
					Debug.Log ("MediumSphere " + this.gameObject.GetInstanceID() + " attacked target " + target.gameObject.GetInstanceID() + " for 10 damage!");
					canAttack = false;														
					if(target.GetComponent<TargetController>().isDead ()){					//check if target is dead
						visibleTargets.Remove(target);										
						targets.Remove (target);
						player.GetComponent <BigSphereController>().removeTarget(target);
						this.setIsEngaged(false);
						Debug.Log ("Set isengaged to False after killing enemy!");
					}
				}
			}
			//if (mySmallSpheres.Count != 0){
			//	foreach (GameObject sphere in mySmallSpheres){
			//		this.dispatchTargetLocation(sphere, targetLocation);
			//	}
			//}
		}
		else {
			if (timeSinceLastSeenEnemy + 1 < Time.time){
				if (this.isEngaged == true){
					this.setIsEngaged(false);
					Debug.Log("MediumSphere " + this.gameObject.GetInstanceID() + " no longer engaged! Returning to Commander!");
				}
			}
		}
		//else if(mySmallSpheres.Count != 0){
		//	foreach (GameObject sphere in mySmallSpheres){
		//		this.dispatchTargetLocation(sphere, this.transform.position);
		//	}
		//}
		if ((lastAttack + 1f) < Time.time) {
			canAttack = true;
		}
	}
	public bool getIsEngaged(){
		return this.isEngaged;
	}
	public void setIsEngaged(bool state){
		if (state == true) this.timeSinceLastSeenEnemy = Time.time;
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
	public int getTargetsInRange(){
		return this.visibleTargets.Count;
	}
	void updateTargetVisibility(){
		foreach (GameObject uTarget in targets) {
			if (Mathf.Abs((uTarget.transform.position - this.transform.position).magnitude) < visionRadius && !uTarget.GetComponent<TargetController>().isDead()){
				if (!this.visibleTargets.Contains(uTarget)){
					visibleTargets.Add(uTarget);
				}
			}
			else if (visibleTargets.Contains (uTarget)){
				visibleTargets.Remove (uTarget);
			}
			if(uTarget.GetComponent<TargetController>().isDead()) visibleTargets.Remove (uTarget);
		}
	}
}
