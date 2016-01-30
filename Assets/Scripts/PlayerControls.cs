using UnityEngine;
using System.Collections;
using System;


public class PlayerControls : MonoBehaviour {

    public enum PlayerNumber { ONE, TWO, THREE, FOUR };

    public PlayerNumber playerNumber = PlayerNumber.ONE;
    public float speedMultiplier = 1000;
    public float rotationMultiplier = 100;
    public float maxSpeed = 4000;
    public float boostSpeed = 20000;
    public float boostBounce = 100;
    public float brakeSlowing = 0.1f;
    public float turnMultiplier = 10;
    public Transform sphereZ;
    public Transform sphereX;

	[SerializeField]
	private NavMeshObstacle navMeshObstacle;
	[SerializeField]
	private float maxVelocityToCarveNavMesh;

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
        input = PlayerInput.GetInput((int)playerNumber);
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
            body.AddForce(chargeDirection.normalized * boostSpeed);
            body.AddForce(Vector3.up * boostBounce);
        } else
        {
            // not boosting, and haven't been boosting. just steer normally
            Vector2 movement = input.getMovement();
            direction += movement.x * Time.deltaTime * rotationMultiplier;
            body.AddForce(body.transform.right * movement.x * body.velocity.magnitude * turnMultiplier * speedMultiplier);
            body.AddForce(body.transform.forward * movement.y * speedMultiplier);
            body.rotation = Quaternion.AngleAxis(direction, Vector3.up);
            Vector3 velocity = body.velocity;
            float x = velocity.x;
            velocity.x = velocity.z;
            velocity.z = -x;
            sphereZ.Rotate(velocity, Space.World);
        }

        forwardsVector = body.transform.forward;

		if(Vector3.Magnitude(body.velocity) <= maxVelocityToCarveNavMesh){
			navMeshObstacle.carving = true;
		}
		else
		{
			navMeshObstacle.carving = false;
		}
		
	}

    private bool isOnGround()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(body.position, -Vector3.up, out hitInfo))
        {
            return hitInfo.collider.tag == "ground" && hitInfo.distance < sphereCollider.radius * this.transform.root.localScale.y + 0.5;
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

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Minions")){
			Game.Instance.EventPlayerKilledMinion((int)playerNumber, other.gameObject.GetComponent<CreepsAI>().playerId);
			other.gameObject.GetComponent<CreepsAI>().Kill();
		}
	}
}
