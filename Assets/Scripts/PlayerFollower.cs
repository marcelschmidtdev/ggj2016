using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

    public GameObject playerView;
    public Vector3 cameraOffset = new Vector3(0, -2, 0);
    public float followDistance = 4;
    public float lerpSpeed = 0.9f;

    private Camera cam;
    private Vector3 desiredPosition;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 behindPlayer = playerView.transform.position - playerView.GetComponent<Rigidbody>().transform.forward.normalized * followDistance;
        cam.transform.position = behindPlayer - cameraOffset;
        cam.transform.LookAt(playerView.transform);
	}
}
