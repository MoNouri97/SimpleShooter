using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed;
	Rigidbody rb;

	private void Start()
	{
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
			Debug.Log("enemy");
		}
	}
}
