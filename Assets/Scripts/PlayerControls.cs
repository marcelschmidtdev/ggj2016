using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;


public class PlayerControls : MonoBehaviour {

    public PlayerIndex playerNumber;
    public float speedMultiplier = 1000;
    public float rotationMultiplier = 100;
    public float maxSpeed = 4000;
    public float maxSpeedCharging = 2000;
    public float brakeSlowing = 0.1f;

    private Rigidbody body;
    private Vector3 groundFrictionVector;
    private bool isCharging;
    private Vector3 chargeDirection;
    private float rotation;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        body.transform.forward = Vector3.forward;
        rotation = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        GamePadState gamepad = GamePad.GetState(playerNumber);

        bool wasCharging = isCharging;
        isCharging = gamepad.Buttons.A == ButtonState.Pressed;

        if (isCharging && !wasCharging)
        {
            // they began charging; store direction
            chargeDirection = new Vector3(gamepad.ThumbSticks.Left.X, 0, gamepad.ThumbSticks.Left.Y);
        } else if (isCharging)
        {
            // do nothing; they will slowly slow down as they "charge up"
        } else if (wasCharging && !isCharging)
        {
            // they just released the charge button and will boost!
            body.AddForce(chargeDirection.normalized * maxSpeed * Time.deltaTime);
        } else
        {
            // not boosting, and haven't been boosting. just steer normally
            rotation += gamepad.ThumbSticks.Left.X * rotationMultiplier * Time.deltaTime;
            Vector3 facingDirection = Quaternion.AngleAxis(rotation, Vector3.up) * Vector3.forward;

            float forwardMovement = gamepad.ThumbSticks.Left.Y * speedMultiplier;
            body.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
            body.AddForce(body.transform.forward * forwardMovement * Time.deltaTime);
        }

        limitSpeed();
	}

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (body != null)
        {
            Gizmos.DrawRay(new Ray(body.position, body.transform.forward));
        }
    }

    // is this doing anything??
    private void limitSpeed()
    {
        float speed = body.velocity.magnitude;
        float max = isCharging ? maxSpeedCharging : maxSpeed;
        if (speed > max)
        {
            float brakeSpeed = speed - max;
            Vector3 normalizedVelocity = body.velocity.normalized;
            Vector3 brakeVelocity = normalizedVelocity * brakeSpeed;
            body.AddForce(-brakeVelocity * brakeSlowing);
        }
    }
}
