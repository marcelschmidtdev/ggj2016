using UnityEngine;
using System.Collections;

public class CreepsAI : MonoBehaviour 
{
	float timer = 5f;

	//Since we are pooling these objects we have to reset all values here
	void OnEnable() {
		timer = 5f;
	}

	void Update() 
	{
		timer -= Time.deltaTime;
		if(timer <= 0){
			SimplePool.Despawn(this.gameObject);
		}
	}
}
