using UnityEngine;
using System.Collections;

public class CreepsSpawner : MonoBehaviour
{
	[SerializeField]
	private float spawnRateInSeconds;
	[SerializeField]
	private int amountOfCreeps;
	[SerializeField]
	private GameObject[] availableCreeps;
	[SerializeField]
	private Transform creepTarget;
	[SerializeField]
	private int playerId;

	private bool gameStarted = false;
	private float spawnTimer = 0;

	void Awake()
	{
		Game.Instance.EventGameStarted += () => gameStarted = true;
		Game.Instance.EventGameOver += () => gameStarted = false;
	}

	void Update () 
	{
		// For testing only
		if(Input.GetKeyDown(KeyCode.Space)){
			Debug.Log("Game started.");
			gameStarted = true;
		}

		if(gameStarted) {
			spawnTimer -= Time.deltaTime;
			if(spawnTimer <= 0) {
				spawnTimer += spawnRateInSeconds;
				SpawnWave();
			}
		}
	}

	void SpawnWave ()
	{
		for (int i = 0; i < amountOfCreeps; i++) {
			Vector3 spawnPos = new Vector3(
				this.transform.position.x + Random.Range(-1.5f,1.5f),
				this.transform.position.y,
				this.transform.position.z + Random.Range(-1.5f,1.5f));
			var instance = SimplePool.Spawn(availableCreeps[playerId], spawnPos, Quaternion.identity);
			instance.GetComponent<CreepsAI>().targetPosition = creepTarget.position;
		}
	}

	void OnDestroy(){
		Game.Instance.EventGameStarted -= () => gameStarted = true;
		Game.Instance.EventGameOver -= () => gameStarted = false;
	}
}
