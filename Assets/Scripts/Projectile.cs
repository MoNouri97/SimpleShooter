using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject impactEffect;
	public float speed;
	public float damage = 1;
	public Color trailColor;
	Rigidbody rb;


	private void Start()
	{
		Destroy(gameObject, 2f);
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed;
		Material mat = GetComponent<TrailRenderer>().material;
		mat.SetColor("_Color", trailColor);
		mat.SetColor("_EmissionColor", trailColor);
	}
	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	private void OnTriggerEnter(Collider other)
	{
		Impact(other);
	}
	private void OnCollisionEnter(Collision other)
	{
		Impact(other.collider, other);
	}

	private void Impact(Collider collider, Collision collision = null)
	{
		Destroy(gameObject);
		if (collision != null)
		{
			Destroy(Instantiate(impactEffect, transform.position, Quaternion.LookRotation(collision.GetContact(0).normal, Vector3.up)), 1f);
		}

		IDamageable hit = collider.GetComponent<IDamageable>();
		if (hit != null)
		{
			hit.takeHit(damage, transform.position, transform.forward);
		}
	}
}
