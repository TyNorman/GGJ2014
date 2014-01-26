﻿using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
	//EXPOSED FOR TESTING PURPOSES
	public float maxDetectionRange = 2.5f;

	private const float MAX_WANDER_RANGE = 5;
	private const float MIN_WANDER_RANGE = 1;
	private const float MOVESPEED = 2;
	private const float WANDERTIME = 2.5f; //In seconds

	private GameObject target = null;

	private Vector2 wanderLocation = Vector2.zero;

	private float wanderTimer = 0;

	private bool isWanderTimeRunning = false;

	// Use this for initialization
	void Start () 
	{
		target = GameObject.FindGameObjectWithTag ("Player") as GameObject;
		maxDetectionRange += (gameObject.renderer.bounds.size.x + gameObject.renderer.bounds.size.y)/1.5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isWanderTimeRunning) 
		{
			wanderTimer += Time.deltaTime;

			if(wanderTimer >= WANDERTIME)
			{
				wanderTimer = 0;
				isWanderTimeRunning = false;
			}
		}

		UpdateAI ();
	}

	private void UpdateAI()
	{
		Vector3 newPos = gameObject.transform.position;
		int directionModifier = 0;

		//If the target is in range, move towards them.
		//Otherwise, wander.
		//Keep It Simple, Stupid.

		if (IsTargetInRange() && IsLargerThanTarget()) //Move towards the target
		{
			newPos = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, MOVESPEED * Time.deltaTime);
		}
		else //Generate a new wander position and move towards it.
		{
			if(!isWanderTimeRunning)
				wanderLocation = Wander();

			newPos = Vector3.MoveTowards(gameObject.transform.position, wanderLocation, MOVESPEED/4 * Time.deltaTime);
		}

		gameObject.transform.position = newPos;
	}

	private Vector2 Wander()
	{
		int randDirectionX = 0;
		int randDirectionY = 0;

		if (Random.value > 0.5)
			randDirectionX = 1;
		else
			randDirectionX = -1;

		if (Random.value > 0.5)
			randDirectionY = 1;
		else
			randDirectionY = -1;

		float randX = Random.Range (MIN_WANDER_RANGE, MAX_WANDER_RANGE);
		float randY = Random.Range (MIN_WANDER_RANGE, MAX_WANDER_RANGE);

		isWanderTimeRunning = true;

		return new Vector2 (gameObject.transform.position.x + (randX * randDirectionX), gameObject.transform.position.y + (randY * randDirectionY));
	}

	private bool IsTargetInRange()
	{
		if (target.transform.position.x >= gameObject.transform.position.x - maxDetectionRange && target.transform.position.x <= gameObject.transform.position.x + maxDetectionRange &&
		    target.transform.position.y >= gameObject.transform.position.y - maxDetectionRange && target.transform.position.y <= gameObject.transform.position.y + maxDetectionRange)
			return true;
		else
			return false;
	}

	private bool IsLargerThanTarget()
	{
		float combinedEnemySize = gameObject.renderer.bounds.size.x + gameObject.renderer.bounds.size.y + gameObject.renderer.bounds.size.z;
		float combinedTargetSize = target.renderer.bounds.size.x + target.renderer.bounds.size.y + target.renderer.bounds.size.z;

		return (combinedEnemySize > combinedTargetSize);
	}

	/*private GameObject GetClosestTarget()
	{
		//[CONTINGENCY] Get Player location
	}*/
}
