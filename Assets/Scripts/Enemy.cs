using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
	NavMeshAgent pathfinder;
	Transform target;
	override protected void Start()
	{
		pathfinder = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").transform;

		StartCoroutine(UpdatePath());
	}

	IEnumerator UpdatePath()
	{
		float refreshRate = 1;
		while (target != null)
		{
			pathfinder.SetDestination(new Vector3(target.position.x, 0, target.position.z));
			yield return new WaitForSeconds(refreshRate);
		}

	}

}
