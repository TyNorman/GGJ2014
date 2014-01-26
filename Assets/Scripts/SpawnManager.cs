﻿using UnityEngine;
using System.Collections;

public class SpawnManager
{
	private PlayerScript playerStats = null;

	private const float SCALEMODIFIER = 2.5f;

	private float spawnTimer = 0;
	private float maxSpawnTime = 10;
	private float minMobsToSpawn = 2;
	private float maxMobsToSpawn = 5;
	private float minMobSize = 0.075f;
	private float maxMobSize = 0.75f;

	private bool isSpawnTimerRunning = true;

	private static SpawnManager instance = null;
	
	public static SpawnManager Instance
	{
		get
		{
			if(instance == null)
				instance = new SpawnManager();
			return instance;
		}
	}

	// Use this for initialization
	private SpawnManager ()
	{
		playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();


	}
	
	// Update is called once per frame
	public void Update ()
	{
		UpdateSpawnTimer();
	}

	private void UpdateSpawnTimer()
	{
		if(isSpawnTimerRunning)
		{
			spawnTimer += Time.deltaTime;

			if(spawnTimer >= maxSpawnTime)
			{
				int spawnAmount = GenerateNumMobs();

				for(int i = 0; i < spawnAmount; i++)
				{
					AssembleMob();
				}

				spawnTimer = 0;
			}
		}
	}

	private int GenerateNumMobs()
	{
		return Mathf.CeilToInt(Random.Range(minMobsToSpawn, maxMobsToSpawn));
	}

	public void AssembleMob()
	{
		SpriteRenderer segmentRenderer = null;

		GameObject mob = GameObject.Instantiate(Resources.Load("Prefabs/Mob")) as GameObject;

		//Get the individual sprite segments of the mob and generate their respective parts.
		GameObject head = mob.transform.FindChild("Head").gameObject;
		segmentRenderer = head.GetComponent<SpriteRenderer>();
		segmentRenderer.sprite = GenerateRandHead();

		GameObject tail = mob.transform.FindChild("Tail").gameObject;
		segmentRenderer = tail.GetComponent<SpriteRenderer>();
		segmentRenderer.sprite = GenerateRandTail();

		GameObject back = mob.transform.FindChild("Back").gameObject;
		segmentRenderer = back.GetComponent<SpriteRenderer>();
		segmentRenderer.sprite = GenerateRandBack();

		GameObject limbLeft = mob.transform.FindChild("Limb_Left").gameObject;
		segmentRenderer = limbLeft.GetComponent<SpriteRenderer>();
		segmentRenderer.sprite = GenerateRandLimb();

		GameObject limbRight = mob.transform.FindChild("Limb_Right").gameObject;
		segmentRenderer = limbRight.GetComponent<SpriteRenderer>();
		segmentRenderer.sprite = GenerateRandLimb();

		float newScale = GenerateRandSize();
		Vector3 newScaleVec = new Vector3(newScale, newScale, mob.transform.localScale.z);
		mob.transform.localScale = newScaleVec;

		head.transform.localScale = mob.transform.localScale * SCALEMODIFIER;
		tail.transform.localScale = mob.transform.localScale * SCALEMODIFIER;
		back.transform.localScale = mob.transform.localScale * SCALEMODIFIER;
		limbLeft.transform.localScale = mob.transform.localScale * SCALEMODIFIER;
		limbRight.transform.localScale = mob.transform.localScale * SCALEMODIFIER;

	}


	#region Randomized Segment Generation
	private Sprite GenerateRandHead()
	{
		int randNum = Mathf.FloorToInt(Random.Range(1, 4)); //+1 because Random with ints is exclusive for max

		return Resources.Load<Sprite>("Sprites/Heads/Eye" + randNum);
	}

	private Sprite GenerateRandTail()
	{
		Sprite returnSprite = null;

		int randTailType = Mathf.FloorToInt(Random.Range(1, 6));
		int randTailNum = Mathf.FloorToInt(Random.Range(1, 4));
		
		switch(randTailType)
		{
			case 1:
			returnSprite = Resources.Load<Sprite>("Sprites/Tails/DinoTail" + randTailNum);
				break;
			case 2:
				returnSprite = Resources.Load<Sprite>("Sprites/Tails/FishTail" + randTailNum);
				break;
			case 3:
				returnSprite = Resources.Load<Sprite>("Sprites/Tails/Jelly" + randTailNum);
				break;
			case 4:
				returnSprite = Resources.Load<Sprite>("Sprites/Tails/Stinger" + randTailNum);
				break;
			case 5:
				returnSprite = Resources.Load<Sprite>("Sprites/Tails/TailFeathers" + randTailNum);
				break;

		}
		
		return returnSprite;
	}

	private Sprite GenerateRandBack()
	{
		Sprite returnSprite = null;
		
		int randBackType = Mathf.FloorToInt(Random.Range(1, 4));
		int randBackNum = Mathf.FloorToInt(Random.Range(1, 4));
		
		switch(randBackType)
		{
		case 1:
			returnSprite = Resources.Load<Sprite>("Sprites/Backs/Plant" + randBackNum);
			break;
		case 2:
			returnSprite = Resources.Load<Sprite>("Sprites/Backs/Shell" + randBackNum);
			break;
		case 3:
			returnSprite = Resources.Load<Sprite>("Sprites/Backs/Wings" + randBackNum);
			break;
		}
		
		return returnSprite;
	}

	private Sprite GenerateRandLimb()
	{
		Sprite returnSprite = null;
		
		int randLimbType = Mathf.FloorToInt(Random.Range(1, 6));
		int randLimbNum = Mathf.FloorToInt(Random.Range(1, 4));
		
		switch(randLimbType)
		{
		case 1:
			returnSprite = Resources.Load<Sprite>("Sprites/Limbs/BugLeg" + randLimbNum);
			break;
		case 2:
			returnSprite = Resources.Load<Sprite>("Sprites/Limbs/Claw" + randLimbNum);
			break;
		case 3:
			returnSprite = Resources.Load<Sprite>("Sprites/Limbs/Paw" + randLimbNum);
			break;
		case 4:
			returnSprite = Resources.Load<Sprite>("Sprites/Limbs/RoboFoot" + randLimbNum);
			break;
		case 5:
			returnSprite = Resources.Load<Sprite>("Sprites/Limbs/Tentacle" + randLimbNum);
			break;
		}
		
		return returnSprite;
	}
	#endregion

	private float GenerateRandSize()
	{
		return Random.Range(minMobSize, maxMobSize);
	}

}