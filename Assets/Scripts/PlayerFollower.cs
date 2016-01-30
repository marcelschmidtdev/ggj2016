using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

    public PlayerView playerView;
    public Vector3 cameraOffset = new Vector3(0, 2, 0);
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
        Vector3 behindPlayer = playerView.transform.position - playerView.getDirection() * followDistance;
        Vector3 desiredPosition = behindPlayer + cameraOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed);
        transform.LookAt(playerView.transform);
	}
}
