using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class CreepsAI : MonoBehaviour 
{
	[SerializeField]
	private NavMeshAgent navMeshAgent;
	public Vector3 targetPosition {get; set;}

	//Since we are pooling these objects we have to reset all values here
	void OnEnable() 
	{
		navMeshAgent.destination = targetPosition;
	}

	void Update() 
	{
		if(navMeshAgent.remainingDistance <= 0.5f){
			SimplePool.Despawn(this.gameObject);
		}
	}
}
