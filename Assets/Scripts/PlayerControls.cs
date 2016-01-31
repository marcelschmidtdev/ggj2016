using UnityEngine;
using System.Collections;
using System;


public class PlayerControls : MonoBehaviour {

    public enum PlayerNumber { ONE, TWO, THREE, FOUR };

    public PlayerNumber playerNumber = PlayerNumber.ONE;
    public float speedMultiplier = 1000;
    public float rotationMultiplier = 100;
    public float boostBounce = 100;
    public float turnMultiplier = 10;
    public float secondsBetweenExplodes = 5;
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
    private float chargingTime;
    private float secondsSinceExplode;
	public float sideFriction = 0.01f;

    private InputMapper input;
	private Vector2 movement;

	void Start () {
        body = GetComponent<Rigidbody>();
        forwardsVector = Vector3.forward;
        sphereCollider = GetComponent<SphereCollider>();
        //direction = 0;
        secondsSinceExplode = -secondsBetweenExplodes;
        input = PlayerInput.GetInput((int)playerNumber);
    }

	void Update() {
		movement = input.getMovement();
	}

	public float testVarA = 1.0f;

	void FixedUpdate () {
        // people can turn at any time.
        movement = input.getMovement();
        direction += movement.x * Time.deltaTime * rotationMultiplier;

		//var forwardWeight = Vector3.Dot( body.velocity, body.transform.forward.normalized );
		//body.AddForce( -forwardWeight * body.transform.forward.normalized * testVarA );

		body.rotation = Quaternion.AngleAxis(direction, Vector3.up);

		//body.AddForce( forwardWeight * body.transform.forward.normalized * testVarA );

		bool wasCharging = isCharging;
        isCharging = input.GetConfirm();

        secondsSinceExplode += Time.deltaTime;

        if (Game.Instance.GameState != Game.GameStateId.Playing || !isOnGround())
        {
            // do nothing
        } else if (input.GetCancel())
        {
            explode();
        } else if (!wasCharging && isCharging)
        {
            chargingTime = Time.deltaTime;
        } else if (isCharging)
        {
            chargingTime += Time.deltaTime;
        } else if (wasCharging && !isCharging)
        {
            body.AddForce(Vector3.up * boostBounce * Mathf.Clamp(chargingTime, 0, 1));
        } else
        {
            // not boosting, and haven't been boosting. just steer normally
            body.AddForce(body.transform.right * movement.x * body.velocity.magnitude * turnMultiplier * speedMultiplier);
            body.AddForce(body.transform.forward * movement.y * speedMultiplier);
        }
		if (isOnGround()) {
			// remove all sidewards velocity
			Vector3 vel = body.velocity;
			var rightWeight = Vector3.Dot( body.velocity, body.transform.right.normalized );
			body.velocity -= sideFriction * rightWeight * body.transform.right.normalized;
		}

        // spin
        Vector3 velocity = body.velocity;
        float x = velocity.x;
        velocity.x = velocity.z;
        velocity.z = -x;
        sphereZ.Rotate(velocity, Space.World);

        forwardsVector = body.transform.forward;


        if (Vector3.Magnitude(body.velocity) <= maxVelocityToCarveNavMesh){
			navMeshObstacle.carving = true;
		}
		else
		{
			navMeshObstacle.carving = false;
		}
		
	}

    private void explode()
    {
        if (secondsSinceExplode < secondsBetweenExplodes)
        {
            return;
        }
        secondsSinceExplode = 0;


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
			Game.Instance.NotifyPlayerKill( (int)playerNumber, other.gameObject.GetComponent<CreepsAI>().playerId);
			other.gameObject.GetComponent<CreepsAI>().Kill();
		}
	}
}
