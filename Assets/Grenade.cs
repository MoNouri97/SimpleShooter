using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	Rigidbody rb;
	public float throwForce;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	void Start()
	{
		rb.AddForce((transform.forward) * throwForce, ForceMode.Impulse);
		StartCoroutine(Detonate(3));
	}
	IEnumerator Detonate(float time)
	{
		yield return new WaitForSeconds(time);
		print("boom");
		Destroy(gameObject);
	}

}
