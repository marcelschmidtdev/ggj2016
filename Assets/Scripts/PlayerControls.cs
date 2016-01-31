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
    public Transform sphereZ;
    public Transform sphereX;

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
	[SerializeField]
	private float explosionCooldown = 3f;
	[SerializeField]
	private ParticleSystem boomParticle;

    private Rigidbody body;
    private Vector3 groundFrictionVector;
    private bool isCharging;
    private Vector3 chargeDirection;
    public float direction;
    private SphereCollider sphereCollider;
    private Vector3 forwardsVector;
    private float chargingTime;

    private InputMapper input;
	private Vector2 movement;

	public ParticleSystem ExplosionCooldownIndicator;

	private float explosionCooldownTimer = 0;

	void Start () {
        body = GetComponent<Rigidbody>();
        forwardsVector = Vector3.forward;
        sphereCollider = GetComponent<SphereCollider>();

        input = PlayerInput.GetInput((int)playerNumber);
    }

	void Update() {
		movement = input.getMovement();
		if(explosionCooldownTimer > 0)
		{
			explosionCooldownTimer -= Time.deltaTime;
			if(this.explosionCooldownTimer <= 0.0f) {
				this.ExplosionCooldownIndicator.enableEmission = true;
			}
		}
	}
	
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

        if (Game.Instance.GameState != Game.GameStateId.Playing || !isOnGround())
        {
            // do nothing
        } else if (input.GetCancel() && explosionCooldownTimer <= 0)
        {
			explosionCooldownTimer = explosionCooldown;
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
		if (isOnGround()) {
			// remove all sidewards velocity
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

    private void Explode()
    {
		boomParticle.Clear();
		boomParticle.Play();
		this.ExplosionCooldownIndicator.enableEmission = false;
		for (int i = 0; i < 4; i++) {
			if(i == (int) playerNumber)
				continue;

			var player = Game.Instance.GetPlayer(i);
			if(player == null)
				continue;

			if(Vector3.Distance(this.transform.position, player.PlayerControls.transform.position) <= explosionRadius)
			{
				player.PlayerControls.AddExplosionForce(this.transform.position, explosionRadius, explosionForce, upwardsModifier);
			}
		}
    }

	public void AddExplosionForce(Vector3 explosionCenter, float explosionRadius, float explosionForce, float upwardsModifier) 
	{
		body.AddExplosionForce(explosionForce, explosionCenter, explosionRadius, upwardsModifier, ForceMode.VelocityChange);
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
			var creepsAi = other.gameObject.GetComponent<CreepsAI>();
			if (creepsAi.isDead)
				return;
			Game.Instance.NotifyPlayerKill( (int)playerNumber, creepsAi.playerId);
			other.gameObject.GetComponent<CreepsAI>().Kill();
		}
	}
}
