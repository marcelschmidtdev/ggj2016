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

	[SerializeField]
	private Material RedTowerMaterial; 

	[SerializeField]
	private Material BlueTowerMaterial; 

	[SerializeField]
	private ParticleSystem RedTowerActiveParticleSystem; 
	[SerializeField]
	private ParticleSystem BlueTowerActiveParticleSystem; 

	[SerializeField]
	private MeshRenderer SpawnerTower; 

	[SerializeField]
	private ParticleSystem BlueSpawnField; 

	[SerializeField]
	private ParticleSystem RedSpawnField; 


	private bool gameStarted = false;
	private float spawnTimer = 0;

	void Awake()
	{
		Game.Instance.EventGameStateChanged += HandleGameStateChange;
	}

	void OnDisable(){
		this.RedTowerActiveParticleSystem.Stop(); 
		this.BlueTowerActiveParticleSystem.Stop();
		this.SpawnerTower.material.color = Color.white; 
	}
	
	void HandleGameStateChange(Game.GameStateId newState) {
		if (newState == Game.GameStateId.Playing) {
			gameStarted = true;
			teamId = Lobby.GameConfig.PlayerTeamNumbers[playerId];
			if ( teamId == 0 ) {
				this.SpawnerTower.material = this.RedTowerMaterial; 
				this.RedTowerActiveParticleSystem.Play(); 
				this.RedSpawnField.Play(); 

			} else if(teamId == 1 ) {
				this.SpawnerTower.material = this.BlueTowerMaterial; 
				this.BlueTowerActiveParticleSystem.Play(); 
				this.BlueSpawnField.Play(); 
			}

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
		if(Game.DoesInstanceExist())
			Game.Instance.EventGameStateChanged -= HandleGameStateChange;
	}
}
