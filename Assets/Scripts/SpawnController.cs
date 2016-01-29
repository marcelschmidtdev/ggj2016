using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
	[SerializeField]
	private float spawnRateInSeconds;
	[SerializeField]
	private int amountOfCreeps;
	[SerializeField]
	private GameObject[] AvailableCreeps;

	public int PlayerId { get; private set; }

	//TODO register at game instance singleton and get game start message
	private bool gameStarted = false;

	void Update () {
		if(gameStarted) {

		}
	}
}
