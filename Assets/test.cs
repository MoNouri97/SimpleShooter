using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
	Rigidbody rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		SetSpeed(10);
	}
	public void SetSpeed(float speed)
	{
		// this.speed = speed;
		rb.velocity = Vector3.forward * speed;
	}
}
