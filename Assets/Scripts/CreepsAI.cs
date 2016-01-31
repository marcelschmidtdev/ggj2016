using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class CreepsAI : MonoBehaviour 
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private NavMeshAgent navMeshAgent;
	[SerializeField]
	private float minDistToTarget;
	[SerializeField]
	private float timeUntilDespawn = 3f;
	public Vector3 targetPosition {get; set;}
	public int playerId {get; set;}
	public ParticleSystem HolyDespawn; 
	public bool isDead
	{
		get; private set;
	}
	private bool reachedTarget = false; 
	public float ascendingModifier = 4; 

	//Since we are pooling these objects we have to reset all values here
	void OnEnable() 
	{
		StopAllCoroutines(); 
		isDead = false;
		this.transform.localScale = Vector3.one;
		this.reachedTarget = false; 
		this.navMeshAgent.enabled = true; 
		navMeshAgent.ResetPath();
		navMeshAgent.SetDestination(targetPosition);
		this.HolyDespawn.Stop(); 
		this.HolyDespawn.Clear(); 
		this.HolyDespawn.time = 0; 


	}

	void OnDisable(){
		StopAllCoroutines(); 
	}

	void OnDestroy(){
		StopAllCoroutines(); 
	}
	void Update() 
	{
		if(!isDead && Vector3.Distance(this.transform.position, targetPosition) <= minDistToTarget) { 
			this.isDead = true; 
			Game.Instance.NotifyPlayerScrored(playerId);
			StopAllCoroutines(); 
			StartCoroutine(Co_Despawn()); 

		}

		if ( this.reachedTarget ) {
			var pos = this.transform.position; 
			pos.y += this.ascendingModifier*Time.deltaTime ; 
			this.transform.position = pos; 
		}

	}

	public void Kill()
	{
		StartCoroutine(Co_Kill());
	}

	private IEnumerator Co_Despawn(){
		this.navMeshAgent.Stop(); 
		this.HolyDespawn.Clear(); 
		this.HolyDespawn.time = 0; 
		this.HolyDespawn.Play(); 
		this.reachedTarget = true;
		this.navMeshAgent.enabled = false; 
		yield return new WaitForSeconds( 2) ; 
		SimplePool.Despawn(this.gameObject);
	}
	private IEnumerator Co_Kill()
	{
		isDead = true;
		animator.Play("MinionStand");
		navMeshAgent.Stop();
		Vector3 scale = this.transform.localScale;
		scale.y = 0.1f;
		this.transform.localScale = scale;
		yield return new WaitForSeconds(timeUntilDespawn);
		SimplePool.Despawn(this.gameObject);
	}
}
