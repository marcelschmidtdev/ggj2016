using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour {

	public Image[] Crowns;
	public Text[] MurderedLabels;
	public Text[] TraitoredLabels;
	public Text[] SavedLabels;

	void Start () {
		InitFields();
	}

	void InitFields () {
		for(int i = 0; i < 2; i++) {
			var teamResult = Game.Results.Results[i];
			Crowns[i].color = teamResult.Winner ? Color.white : new Color();
			MurderedLabels[i].text = teamResult.KilledMinions.ToString();
			TraitoredLabels[i].text = teamResult.KilledOwnMinions.ToString();
			SavedLabels[i].text = teamResult.SavedMinions.ToString();
		}
	}

	float activeTime = 0.0f;
	public float minTime = 0.5f;
	void Update () {
		if(activeTime < minTime) {
			activeTime += Time.deltaTime;
			return;
		}
		if (Input.anyKey)
			Application.LoadLevel( "Lobby" );
	}
}
