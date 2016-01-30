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
    public float direction;
    private SphereCollider sphereCollider;
    private Vector3 forwardsVector;

    private InputMapper input;

	void Start () {
        body = GetComponent<Rigidbody>();
        forwardsVector = Vector3.forward;
        sphereCollider = GetComponent<SphereCollider>();
        direction = 0;
#if UNITY_STANDALONE_WIN
        input = new InputMapperWindows((int)playerNumber);
#else
        input = new InputMapperDefault((int)playerNumber);
#endif
    }
	
	void FixedUpdate () {
        if (Game.Instance.GameState != Game.GameStateId.Playing)
        {
            return;
        }
        if (!isOnGround())
        {
            return;
        }

        bool wasCharging = isCharging;
        isCharging = input.GetConfirm();
        if (!wasCharging && isCharging)
        {
            chargeDirection = getForwardsVector();
        } else if (isCharging)
        {
            // do nothing; they will slowly slow down as they "charge up"
        } else if (wasCharging && !isCharging)
        {
            body.AddForce(chargeDirection.normalized * maxSpeed * Time.deltaTime);
            body.AddForce(Vector3.up);
        } else
        {
            // not boosting, and haven't been boosting. just steer normally
            Vector2 movement = input.getMovement();
            direction += movement.x * Time.deltaTime * rotationMultiplier;
            body.AddForce(body.transform.right * movement.x * movement.y * speedMultiplier);
            body.AddForce(body.transform.forward * movement.y * speedMultiplier);
            body.rotation = Quaternion.AngleAxis(direction, Vector3.up);
            Vector3 velocity = body.velocity;
            float x = velocity.x;
            velocity.x = velocity.z;
            velocity.z = -x;
            sphereZ.Rotate(velocity, Space.World);
        }

        forwardsVector = body.transform.forward;
        limitSpeed();
	}

    private bool isOnGround()
    {
        Ray belowPlayer = new Ray(body.position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(body.position, -Vector3.up, out hitInfo))
        {
            return hitInfo.collider.tag == "ground" && hitInfo.distance < sphereCollider.radius + 0.1;
        } else
        {
            return false;
        }
    }

    internal Vector3 getForwardsVector()
    {
        return forwardsVector;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (body != null)
        {
            Gizmos.DrawRay(new Ray(body.position, body.transform.forward));
        }
    }

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
			other.gameObject.GetComponent<CreepsAI>().Kill();
		}
	}
}
