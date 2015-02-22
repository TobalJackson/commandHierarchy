using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	public int speed;
	public int followerMinDistance;
	public int followerMaxDistance;
	public int visionRadius;

	GameObject[] mediumSpheres;
	GameObject[] targets;


	ArrayList visibleTargets;

	Vector3 sphereForce;
	Rigidbody rigidSphere;
	// Use this for initialization
	void Start () {
		mediumSpheres = GameObject.FindGameObjectsWithTag ("MediumSpheres");
		rigidSphere = GetComponent<Rigidbody> ();
		sphereForce = new Vector3 ();
		targets = GameObject.FindGameObjectsWithTag ("Target");
		visibleTargets = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject uTarget in targets) {
			if (uTarget.transform.position - this.transform.position < visionRadius){
				if (!this.visibleTargets.Contains(uTarget)){
					visibleTargets.Add(uTarget);
				}
			}
		}
	}
	void FixedUpdate(){
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		sphereForce.Set (h, 0f, v);

		rigidSphere.AddForce (sphereForce * speed * Time.fixedDeltaTime);

	}
}
