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
    public float minimumSpeedForScreenShake = 1;
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
	[SerializeField]
	private float explosionCooldown = 3f;
	[SerializeField]
	private ParticleSystem boomParticle;

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

    private float previousSpeed = 0;
	public ParticleSystem ExplosionCooldownIndicator;

	private float explosionCooldownTimer = 0;

	public float collisionMultiplier = 3.0f;

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

		if(transform.position.y < -5.0f) {
			var player = Game.Instance.GetPlayer( (int)this.playerNumber );
			var spawner = Map.Instance.PlayerSpawners[(int)this.playerNumber];
			player.Position = spawner.Position;
			player.Rotation = spawner.Rotation;
			body.velocity = Vector3.zero;
		}
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
		if (getOnGround()) {
			// remove all sidewards velocity
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
        if (shakeAmount > minimumSpeedForScreenShake) {
            screenShaker.GetComponent<ScreenShake>().addShake(shakeAmount * shakeAmount);
        }
        previousSpeed = speed;
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
			var creepsAi = other.gameObject.GetComponent<CreepsAI>();
			if (creepsAi.isDead)
				return;
			Game.Instance.NotifyPlayerKill( (int)playerNumber, creepsAi.playerId);
			other.gameObject.GetComponent<CreepsAI>().Kill();
		}
	}

	public float HeightOffset = -3.0f;
	
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.gameObject.tag == "Player") {
			// velocity in player direction;
			Vector3 playerDirection = (collision.collider.transform.position - transform.position).normalized;
			Vector3 offsetPlayerDirection = (collision.collider.transform.position - (transform.position + new Vector3(0.0f, this.HeightOffset, 0.0f))).normalized;
			float playerImpact = Vector3.Dot( this.body.velocity, playerDirection );
			if(playerImpact > 0.0f)
				StartCoroutine( Co_ApplyCollisionForce( collision.collider.GetComponent<Rigidbody>(), playerImpact * this.collisionMultiplier * offsetPlayerDirection ) );
		}
	}

	IEnumerator Co_ApplyCollisionForce (Rigidbody body, Vector3 force) {
		yield return new WaitForFixedUpdate();
		body.AddForce( force );
	}
}
