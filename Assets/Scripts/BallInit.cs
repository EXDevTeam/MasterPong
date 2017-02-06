﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0618, 0649
public class BallInit : MonoBehaviour {

	bool showStart; //flag to control Start Label
	private float countdown = 10f;
	private float roundTime= 10f;
	private float lastRoundTime = -1f;
	private Rigidbody2D m_Rigidbody2D;
	[SerializeField]private AudioSource playerBump;
	[SerializeField]private AudioSource countdownSound1;
	[SerializeField]private AudioSource countdownSound2;
	[SerializeField]private Text countdownText;
	// Use this for initialization
	void Start()
	{
		

		//get postion and velocity
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Rigidbody2D.velocity = new Vector2(0f, 0f);
		transform.position = new Vector3(0f, 0f, 0f);

		//Calls ball reset in 2 seconds
		Invoke ("ballReset", 1f);
	}

	//Resets all ball Properties
	private void ballReset()
	{
		Debug.Log ("Reset Ball!");
		countdown = 4f;
		m_Rigidbody2D.velocity = new Vector2(0f, 0f);
		transform.position = new Vector3(0f, 0f, 0f);
		Invoke ("playBall", 4f);

	}

	//Plays the Ball
	private void playBall()
	{
		showStart = false;

		float randomNumber = Random.Range (-2, 2);
		float dir = Random.Range (-1, 2);
		Debug.Log ("rand: " + randomNumber);
		Debug.Log ("dir: " + dir);
		float multiplier = Random.value;

		if (randomNumber <= 0.5)
			m_Rigidbody2D.AddForce(new Vector2 (-40, 30f * multiplier*dir));
		else
			m_Rigidbody2D.AddForce(new Vector2 (40, 30f * multiplier*dir));
	}
		

	void OnCollisionEnter2D(Collision2D collInfo)
	{
		//ball mechanics here, should make constants
		if (collInfo.collider.name == ("Player1") || collInfo.collider.name == ("Player2")) 
		{
			float randomPitchMult = Random.Range (0.8f, 1.2f);
			playerBump.pitch = 1 * randomPitchMult;
			playerBump.Play ();

			float velY = m_Rigidbody2D.velocity.y;
			float velX = m_Rigidbody2D.velocity.x;
			velY = velY / 2 + collInfo.rigidbody.velocity.y / 2;
			if (m_Rigidbody2D.velocity.x < 30f) 
			{
				if (velX > 0)
					velX = 15f;
				else
					velX = -15f;
			}
				m_Rigidbody2D.velocity = new Vector2(velX, velY);

		}
	}

	void Update()
	{
		countdown -= Time.deltaTime;
		roundTime = Mathf.Round (countdown);
		if (roundTime < 4 && roundTime > 0) 
		{
			showStart = true;	
		}
		if (showStart) 
		{
			if (lastRoundTime != roundTime) 
			{
				countdownText.text = roundTime.ToString();
				lastRoundTime = roundTime;

				if (lastRoundTime > 0) 
				{
					countdownSound1.Play ();
				}
				else 
				{
					countdownSound2.Play ();
				}
			}

		}
		else
		{
			countdownText.text = "";
		}
	}
}
