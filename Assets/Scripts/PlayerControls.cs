using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;


public class PlayerControls : MonoBehaviour {

    public enum PlayerNumber { ONE, TWO, THREE, FOUR };

    public PlayerNumber playerNumber = PlayerNumber.ONE;
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
    private InputMapper input;

	void Start () {
        body = GetComponent<Rigidbody>();
        body.transform.forward = Vector3.forward;
        rotation = 0;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            input = new InputMapperWindows((int)playerNumber);
        }
        else
        {
            input = new InputMapperDefault((int)playerNumber);
        }
    }
	
	void FixedUpdate () {
        input.Update();
        if (input.startedCharging())
        {
            chargeDirection = body.transform.forward;
        } else if (input.isCharging())
        {
            // do nothing; they will slowly slow down as they "charge up"
        } else if (input.finishedCharging())
        {
            // they just released the charge button and will boost!
            body.AddForce(chargeDirection.normalized * maxSpeed * Time.deltaTime);
        } else
        {
            // not boosting, and haven't been boosting. just steer normally
            rotation += input.getMovement().x * rotationMultiplier * Time.deltaTime;
            float forwardMovement = input.getMovement().y * speedMultiplier;
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

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log(collision.gameObject.name);
		if(collision.gameObject.layer == LayerMask.NameToLayer("Minions")){
			SimplePool.Despawn(collision.gameObject);
		}
	}
}
