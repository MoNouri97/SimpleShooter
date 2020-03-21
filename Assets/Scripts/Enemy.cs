using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
	public float damage = 1;
	NavMeshAgent pathfinder;
	Transform target;
	bool hasTarget;
	LivingEntity targetEntity;
	enum State { Idle, Chasing, Attacking };
	State currentState;
	float attackDisThreshold = .5f;
	float timeBetweenAttacks = 1f;
	float nextAttackTime;
	float myCollisionRadius = .5f;
	float targetCollisionRadius = .5f;
	Material skinMat;
	Color skinColor;

	private void Awake()
	{
		pathfinder = GetComponent<NavMeshAgent>();
		GameObject targetObject = GameObject.FindGameObjectWithTag("Player");
		hasTarget = (targetObject != null);
		if (hasTarget)
		{
			target = targetObject.transform;
			targetEntity = target.GetComponent<LivingEntity>();
			targetEntity.OnDeath += OnTargetDeath;
		}
	}
	override protected void Start()
	{
		base.Start();

		if (!hasTarget) return;

		currentState = State.Chasing;

		StartCoroutine(UpdatePath());

	}
	private void Update()
	{
		if (!hasTarget || Time.time <= nextAttackTime)
		{
			return;
		}
		float disToTarget = (target.position - transform.position).sqrMagnitude;
		if (disToTarget >= Mathf.Pow(attackDisThreshold + myCollisionRadius + targetCollisionRadius, 2))
		{
			return;
		}
		nextAttackTime = Time.time + timeBetweenAttacks;
		StartCoroutine(Attack());
	}
	void OnTargetDeath()
	{
		currentState = State.Idle;
		hasTarget = false;
	}
	IEnumerator Attack()
	{
		currentState = State.Attacking;
		// pathfinder.enabled = false;

		Vector3 originalPos = transform.position;
		Vector3 directionToTarget = (target.position - transform.position).normalized;
		Vector3 attackPos = target.position
															- directionToTarget
															* (myCollisionRadius);
		attackPos.y = originalPos.y;

		float attackSpeed = 3;
		float percent = 0;
		bool hasAppliedDamage = false;

		while (percent < 1)
		{
			if (!hasAppliedDamage && percent >= .5f)
			{
				hasAppliedDamage = true;
				targetEntity.takeDamage(damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			transform.position = Vector3.Lerp(originalPos, attackPos, interpolation);
			yield return null;
		}
		currentState = (hasTarget) ? State.Chasing : State.Idle;
		// pathfinder.enabled = true;

	}

	IEnumerator UpdatePath()
	{
		float refreshRate = 0.5f;
		while (hasTarget)
		{
			if (currentState == State.Chasing)
			{
				Vector3 directionToTarget = (target.position - transform.position).normalized;
				Vector3 destination = target.position
																	- directionToTarget
																	* (myCollisionRadius + targetCollisionRadius + attackDisThreshold / 2);
				pathfinder.SetDestination(destination);
			}
			yield return new WaitForSeconds(refreshRate);
		}
		pathfinder.destination = transform.position;

	}

	public void setProperties(float speed, int hitsToKillPlayer, float health, Color color)
	{
		pathfinder.speed = speed;
		if (hasTarget)
		{
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = health;
		skinMat = GetComponent<Renderer>().material;
		skinMat.color = color;
		skinColor = color;
	}
	public override void takeHit(float damage, Vector3 hitPoint, Vector3 direction)
	{

		base.takeHit(damage, hitPoint, direction);
	}

}
