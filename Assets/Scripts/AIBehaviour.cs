﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIBehaviour : MonoBehaviour 
{
	public List<Transform> path;
	public Transform pathGroup;
	public int currentPathObj;

	public enum PathReset
	{
		loop = 0,
		reverse = 1,
		random = 2
	}

	public PathReset pathReset;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;

	public float moveSpeed = 0.1f;
	private float radius = 0.000000000000000000001f;

	// Use this for initialization
	void Start () 
	{
		GetPath();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Move();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			other.gameObject.transform.position = GameObject.FindGameObjectWithTag("Checkpoint").transform.position;
		}
	}

	void Move()
	{
		float dis = Vector3.Distance(gameObject.transform.position, path[currentPathObj].position);

		if(dis < radius)
		{
			currentPathObj++;
		}

		if(currentPathObj >= path.Count)
		{
			if(pathReset == PathReset.loop)
			{
				currentPathObj = 0;
			}
			else if(pathReset == PathReset.reverse)
			{
				currentPathObj = 0;
				path.Reverse();
			}
			else if (pathReset == PathReset.random)
			{
				currentPathObj = 0;
				pathReset = GetRandomEnum<PathReset>();
			}
		}

		iTween.MoveTo (gameObject, new Vector3(path[currentPathObj].position.x, path[currentPathObj].position.y, 0.0f), moveSpeed);
	}

	void GetPath()
	{
		List<Transform> pathObjs = new List<Transform>();
		pathObjs.AddRange(pathGroup.GetComponentsInChildren<Transform>());
		path = new List<Transform>();
		
		foreach(Transform pathObj in pathObjs)
		{
			if(pathObj != pathGroup)
			{
				path.Add (pathObj);
			}
		}
	}

	static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}
}
