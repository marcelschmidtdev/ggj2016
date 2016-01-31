using UnityEngine;
using System.Collections;

public class CreepsSpawner : MonoBehaviour
{
	[SerializeField]
	private float spawnRateInSeconds;
	[SerializeField]
	private int amountOfCreeps;
	[SerializeField]
	private float minSpawnPosVariance = -1.5f;
	[SerializeField]
	private float maxSpawnPosVariance = 1.5f;
	[SerializeField]
	private GameObject[] availableCreeps;
	[SerializeField]
	private Transform creepTarget;
	[SerializeField]
	private int playerId;
	[SerializeField]
	private int teamId;

	private bool gameStarted = false;
	private float spawnTimer = 0;

	void Awake()
	{
		Game.Instance.EventGameStateChanged += HandleGameStateChange;
	}

	void HandleGameStateChange(Game.GameStateId newState) {
		if (newState == Game.GameStateId.Playing) {
			gameStarted = true;
			teamId = Lobby.GameConfig.PlayerTeamNumbers[playerId];
		}
		else
			gameStarted = false;
	}

	void Update () 
	{
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
				this.transform.position.x + Random.Range(minSpawnPosVariance, maxSpawnPosVariance),
				this.transform.position.y,
				this.transform.position.z + Random.Range(minSpawnPosVariance, maxSpawnPosVariance));
			// availableCreeps[playerId] is for "nations" and the team ID determines the color of that "nation"
			// As we currently only have one "nation", we're going to use the team ID to spawn different colored prefabs for this nation
			// This could be some kind of a nested array -> availableCreeps[playerId][teamId]
//			var instance = SimplePool.Spawn(availableCreeps[playerId], spawnPos, Quaternion.identity);
			var instance = SimplePool.Spawn(availableCreeps[teamId], spawnPos, Quaternion.identity);
			var creepsAI = instance.GetComponent<CreepsAI>();
			creepsAI.targetPosition = creepTarget.position;
			creepsAI.playerId = playerId;
		}
	}

	void OnDestroy(){
		if(Game.Instance!=null)
			Game.Instance.EventGameStateChanged -= HandleGameStateChange;
	}
}
