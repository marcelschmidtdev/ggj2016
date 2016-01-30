using UnityEngine;
using System.Collections;
using System;


public class PlayerControls : MonoBehaviour {

    public enum PlayerNumber { ONE, TWO, THREE, FOUR };

    public PlayerNumber playerNumber = PlayerNumber.ONE;
    public float speedMultiplier = 1000;
    public float rotationMultiplier = 100;
    public float maxSpeed = 4000;
    public float maxSpeedCharging = 2000;
    public float brakeSlowing = 0.1f;
    public Transform sphereZ;
    public Transform sphereX;

    private Rigidbody body;
    private Vector3 groundFrictionVector;
    private bool isCharging;
    private Vector3 chargeDirection;
    private float direction;
    private InputMapper input;

	void Start () {
        body = GetComponent<Rigidbody>();
        body.transform.forward = Vector3.forward;
        direction = 0;
#if UNITY_STANDALONE_WIN
        input = new InputMapperWindows((int)playerNumber);
#else
        input = new InputMapperDefault((int)playerNumber);
#endif
    }
	
	void FixedUpdate () {
        input.Update();
        if (input.startedCharging())
        {
            chargeDirection = transform.forward;
        } else if (input.isCharging())
        {
            // do nothing; they will slowly slow down as they "charge up"
        } else if (input.finishedCharging())
        {
            body.AddForce(chargeDirection.normalized * maxSpeed * Time.deltaTime);
        } else
        {
            // not boosting, and haven't been boosting. just steer normally
            Vector2 movement = input.getMovement();
            direction += movement.x * Time.deltaTime * rotationMultiplier;
            body.AddForce(body.transform.right * movement.x * speedMultiplier);
            body.AddForce(body.transform.forward * movement.y * speedMultiplier);
            body.rotation = Quaternion.AngleAxis(direction, Vector3.up);
            //var angularVelocity = transform.InverseTransformVector(-body.velocity);
            Vector3 velocity = body.velocity;
            float x = velocity.x;
            velocity.x = velocity.z;
            velocity.z = -x;
            sphereZ.Rotate(velocity, Space.World);
        }

        //sphereX.Rotate(Vector3.right, body.velocity.z);
        //sphereZ.Rotate(Vector3.forward, -body.velocity.x);

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
            // apply negative torque
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Minions")){
			Game.Instance.EventPlayerKilledMinion((int)playerNumber, other.gameObject.GetComponent<CreepsAI>().playerId);
			SimplePool.Despawn(other.gameObject);
		}
	}
}
