using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
	#region var

	public float damage = 1;
	NavMeshAgent pathfinder;
	Transform target;
	bool hasTarget;
	LivingEntity targetEntity;
	enum State { Idle, Chasing, Attacking };
	State currentState;
	float attackDisThreshold = .5f;
	float timeBetweenAttacks = 1f;
	float reactionTime = .5f;
	[SerializeField] float nextAttackTime;
	[SerializeField] float myCollisionRadius = .5f;
	[SerializeField] float targetCollisionRadius = .5f;
	Material skinMat;
	Color skinColor;
	public float stunTime = .1f;

	public static event System.Action OnDeathStatic;

	#endregion
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
		if (currentState != State.Chasing || !hasTarget || target == null || Time.time <= nextAttackTime)
		{
			return;
		}
		float disToTarget = (target.position - transform.position).sqrMagnitude;
		if (disToTarget >= Mathf.Pow(attackDisThreshold + myCollisionRadius + targetCollisionRadius, 2))
		{
			return;
		}
		StartCoroutine(React());
	}
	void OnTargetDeath()
	{
		currentState = State.Idle;
		hasTarget = false;
	}
	IEnumerator React()
	{
		nextAttackTime = Time.time + timeBetweenAttacks;
		yield return new WaitForSeconds(reactionTime);
		float disToTarget = (target.position - transform.position).sqrMagnitude;

		if (disToTarget >= Mathf.Pow(attackDisThreshold + myCollisionRadius + targetCollisionRadius, 2))
		{
			yield break;
		}
		StartCoroutine(Attack());
	}
	IEnumerator Attack()
	{
		AudioManager.instance.PlaySound("Enemy Attack");
		currentState = State.Attacking;
		// pathfinder.enabled = false;
		Material mat = GetComponent<Renderer>().material;
		mat.color = Color.red;

		Vector3 originalPos = transform.position;
		Vector3 directionToTarget = (target.position - transform.position).normalized;
		Vector3 attackPos = target.position - directionToTarget * (myCollisionRadius);
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
		mat.color = skinColor;

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

	public void setProperties(
		float speed,
		int hitsToKillPlayer,
		float health,
		Color color,
		float reactionTime = .5f)
	{
		pathfinder.speed = speed;
		if (hasTarget)
		{
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = health;
		skinMat = GetComponent<Renderer>().sharedMaterial;
		skinMat.color = color;
		skinColor = color;
	}
	public override void takeHit(float damage, Vector3 hitPoint, Vector3 direction)
	{
		AudioManager.instance.PlaySound("Impact");
		base.takeHit(damage, hitPoint, direction);
	}


	override protected void Die()
	{
		if (OnDeathStatic != null)
		{
			OnDeathStatic();
		}
		StopAllCoroutines();
		base.Die();
	}



	public override void takeDamage(float damage)
	{
		StartCoroutine(Stun());
		base.takeDamage(damage);
	}

	private IEnumerator Stun()
	{
		pathfinder.isStopped = true;
		yield return new WaitForSeconds(stunTime);
		pathfinder.isStopped = false;
	}
}
