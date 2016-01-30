using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

    public GameObject playerView;
    public Vector3 cameraOffset = new Vector3(0, -2, 0);
    public float followDistance = 4;
    public float lerpSpeed = 0.1f;

    private Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        transform.position = new Vector3(64, 64, 64);
	}

	// Update is called once per frame
	void FixedUpdate () {
        Vector3 towardsPlayer = playerView.transform.position - transform.position;
        Vector3 desiredPosition = playerView.transform.position - towardsPlayer.normalized * followDistance;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed);
        transform.LookAt(playerView.transform);
	}
}
