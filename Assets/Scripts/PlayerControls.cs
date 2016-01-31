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
    public Transform rotatingSphere; // for visual rotation effect we spin this
    public GameObject screenShaker;

	public float sideFriction;

    [SerializeField]
    private NavMeshObstacle navMeshObstacle;
    [SerializeField]
    private float maxVelocityToCarveNavMesh;

	[SerializeField]
	private float explosionRadius = 15f;
	[SerializeField]
	private float upwardsModifier = 2f;
	[SerializeField]
	private float explosionForce = 15f;

    private Rigidbody body;
    private Vector3 groundFrictionVector;
    private bool isCharging;
    private bool isOnGround;
    private Vector3 chargeDirection;
    public float direction;
    private SphereCollider sphereCollider;
    private Vector3 forwardsVector;
    private float chargingTime;

    private InputMapper input;
	private Vector2 movement;

    private float previousSpeed;

	void Start () {
        body = GetComponent<Rigidbody>();
        forwardsVector = Vector3.forward;
        sphereCollider = GetComponent<SphereCollider>();

        input = PlayerInput.GetInput((int)playerNumber);
    }

	void Update() {
		movement = input.getMovement();
	}
	
	void FixedUpdate () {
        // people can turn at any time.
        movement = input.getMovement();
        direction += movement.x * Time.deltaTime * rotationMultiplier;
		body.rotation = Quaternion.AngleAxis(direction, Vector3.up);

        shakeScreen();

		bool wasCharging = isCharging;
        isCharging = input.GetConfirm();

        if (Game.Instance.GameState != Game.GameStateId.Playing || !getOnGround())
        {
            // do nothing
        } else if (input.GetCancel())
        {
            Explode();
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
		if (getOnGround()) {
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
        rotatingSphere.Rotate(velocity, Space.World);

        forwardsVector = body.transform.forward;


        if (Vector3.Magnitude(body.velocity) <= maxVelocityToCarveNavMesh){
			navMeshObstacle.carving = true;
		}
		else
		{
			navMeshObstacle.carving = false;
		}
		
	}

    /// <summary>
    /// Screen shake is generated based on the difference between this frame's speed and the last.
    /// </summary>
    private void shakeScreen()
    {
        float speed = body.velocity.magnitude;
        float shakeAmount = Math.Abs(previousSpeed - speed);
        screenShaker.GetComponent<ScreenShake>().addShake(shakeAmount * shakeAmount);
        previousSpeed = speed;
    }

    private void Explode()
    {
		Debug.LogError("BOOOMM!");
		for (int i = 0; i < 4; i++) {
			if(i == (int) playerNumber)
				continue;

			var player = Game.Instance.GetPlayer(i);
			if(player == null)
				continue;

			if(Vector3.Distance(this.transform.position, player.PlayerControls.transform.position) <= explosionRadius)
			{
				player.PlayerControls.AddExplosionForce(this.transform.position, explosionRadius);
			}
		}
    }

	public void AddExplosionForce(Vector3 explosionCenter, float explosionRadius) 
	{
		body.AddExplosionForce(explosionForce, explosionCenter, explosionRadius, upwardsModifier, ForceMode.VelocityChange);
	}

    private bool getOnGround()
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
