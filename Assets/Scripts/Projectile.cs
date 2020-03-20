using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed;
	public float damage = 1;
	Rigidbody rb;

	private void Start()
	{
		Destroy(gameObject, 2f);  //TODO improve this
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed;
	}
	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
		IDamageable hit = other.GetComponent<IDamageable>();
		if (hit != null)
		{
			hit.takeHit(damage, transform.position, transform.forward);
		}
	}
}
