using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

    public GameObject playerView;
    public PlayerControls controls;
    public Vector3 cameraOffset = new Vector3(0, 2, 0);
	public Vector3 lookAtOffset = new Vector3( 0, 2, 0 );
    public float followDistance = 4;
    public float lerpSpeed = 0.1f;
	Vector3 cameraVelocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        controls = playerView.GetComponent<PlayerControls>();
        transform.position = new Vector3(64, 64, 64);
	}

	// Update is called once per frame
	void FixedUpdate () {
        Vector3 behindPlayer = controls.getForwardsVector();
        behindPlayer.y = 0;
        Vector3 desiredPosition = playerView.transform.position - behindPlayer.normalized * followDistance + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraVelocity, lerpSpeed);
        transform.LookAt(playerView.transform.position+playerView.transform.InverseTransformVector(lookAtOffset));
        //controls.considerSettingDirection(transform.forward);
	}
}
