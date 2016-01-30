using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class CreepsAI : MonoBehaviour 
{
	[SerializeField]
	private NavMeshAgent navMeshAgent;
	[SerializeField]
	private float minDistToTarget;
	public Vector3 targetPosition {get; set;}
	public int playerId {get; set;}

	//Since we are pooling these objects we have to reset all values here
	void OnEnable() 
	{
		navMeshAgent.destination = targetPosition;
	}

	void Update() 
	{
		if(navMeshAgent.remainingDistance <= minDistToTarget) { 
			SimplePool.Despawn(this.gameObject);
			//TODO add message which minion died
		}
	}
}
