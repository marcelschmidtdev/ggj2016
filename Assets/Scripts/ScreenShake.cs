using UnityEngine;
using System.Collections;
using System;

public class ScreenShake : MonoBehaviour {

    private float shakeAmount = 0;
    public float damping = 0.9f;
    public float speed = 10;
    public float maxShake = 10;

    public void addShake(float amount)
    {
        shakeAmount += amount;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        shakeScreen();
        shakeAmount *= damping;
	}

    private void shakeScreen()
    {
        transform.localPosition = new Vector3(Mathf.Cos(Time.time * speed) * shakeAmount, Mathf.Sin(Time.time * speed) * shakeAmount, Mathf.Cos(Time.time * speed) * shakeAmount);
        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, maxShake);
    }
}
