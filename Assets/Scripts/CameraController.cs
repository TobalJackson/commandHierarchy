using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour{
	public Transform target;
	public float smoothing;

	Vector3 offset;
	void Start(){
		smoothing = 10000f;
		offset = transform.position - target.position;
	}

	void FixedUpdate(){
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
	}
}