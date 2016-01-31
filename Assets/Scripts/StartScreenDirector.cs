using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class StartScreenDirector : MonoBehaviour {


	public List<Animator> Actors; 
	public AudioSource BackgroundSound; 
	public GameObject MeteorStorm; 
	public Animator Player; 
	public float TimeTillMeteor; 
	public float TimeTillDisable;
	public float TimeTillPlayer; 
	public float TimeTillEnd; 
	public float TimeTillMinionDie; 
	public float AudioOffset; 
	public float AudioFadeoff; 
	public float AudioFadeIn; 
	public float TimeTillLogoAnimation; 
	public Animator LogoAnimator; 

	void Start () {
		foreach( var actor in this.Actors ) {
			actor.SetTrigger("Rock"); 
		}
		this.BackgroundSound.time = this.AudioOffset; 
		this.BackgroundSound.volume = 0; 
		this.BackgroundSound.Play(); 

	}

	void Update(){
		if ( this.BackgroundSound.volume < 1 ) {
			this.BackgroundSound.volume += Time.deltaTime * this.AudioFadeIn; 
		}
		this.TimeTillMeteor -= Time.deltaTime; 
		if ( this.TimeTillMeteor <= 0 ) {
			StartMeteor(); 
			this.TimeTillPlayer -= Time.deltaTime; 
			if ( this.TimeTillPlayer <= 0 ) {
				StartPlayer(); 
				this.TimeTillMinionDie -= Time.deltaTime; 
				if ( this.TimeTillMinionDie <= 0) {
					foreach( var actor in this.Actors ) {
						actor.SetTrigger("Kill"); 
					}
					this.TimeTillLogoAnimation -= Time.deltaTime; 
					if (this.TimeTillLogoAnimation <= 0){
						this.LogoAnimator.SetTrigger("Logo"); 
					}
				}
				this.TimeTillEnd -= Time.deltaTime; 
				if (this.TimeTillEnd <= 0){
					Application.LoadLevel("MainMenu"); 
				}
				this.TimeTillDisable -= Time.deltaTime; 
				if ( this.TimeTillDisable <= 0 ) {
					this.MeteorStorm.SetActive( false ) ; 
					this.BackgroundSound.volume -= Time.deltaTime * this.AudioFadeoff; 
				}
			} 

		} 

	}

	void StartPlayer(){
		this.Player.SetTrigger("Kill"); 
	}

	void StartMeteor(){
		this.MeteorStorm.SetActive(true); 
	}
	

}
