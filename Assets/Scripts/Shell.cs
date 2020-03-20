using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
	public float forceMin;
	public float forceMax;

	Rigidbody rb;
	float lifeTime = 4;
	float fadeTime = 2;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		float force = Random.Range(forceMin, forceMax);
		rb.AddForce(transform.right * force);
		rb.AddTorque(Random.insideUnitSphere * force);

		StartCoroutine(Fade());

	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(lifeTime);
		float percent = 0;
		float speed = 1 / fadeTime;
		Material mat = GetComponent<Renderer>().material;
		Color initColor = mat.color;
		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			mat.color = Color.Lerp(initColor, Color.clear, percent);
			yield return null;
		}
		Destroy(gameObject);
	}
}
