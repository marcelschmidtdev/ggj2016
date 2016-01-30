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

	//Since we are pooling these objects we have to reset all values here
	void OnEnable() 
	{
		this.transform.localScale = Vector3.one;
		navMeshAgent.destination = targetPosition;
	}

	void Update() 
	{
		if(navMeshAgent.remainingDistance <= minDistToTarget) { 
			SimplePool.Despawn(this.gameObject);
			Game.Instance.NotifyPlayerScrored(playerId);
		}
	}

	public void Kill()
	{
		StartCoroutine(Co_Kill());
	}

	private IEnumerator Co_Kill()
	{
		animator.Play("MinionStand");
		navMeshAgent.Stop();
		Vector3 scale = this.transform.localScale;
		scale.y = 0.1f;
		this.transform.localScale = scale;
		yield return new WaitForSeconds(timeUntilDespawn);
		SimplePool.Despawn(this.gameObject);
	}
}
