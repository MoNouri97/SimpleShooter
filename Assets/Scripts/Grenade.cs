using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	public GameObject effect;
	public float detonationTime;
	public float throwForce;
	public float radius;
	public float tickSpeed = 100;
	public LayerMask layerMask;

	List<LivingEntity> damaged = new List<LivingEntity>();
	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	void Start()
	{
		rb.AddForce((transform.forward) * throwForce, ForceMode.Impulse);
		StartCoroutine(Detonate(detonationTime));

		StartCoroutine(Tick(tickSpeed));
	}
	IEnumerator Tick(float speed)
	{
		float percent = 0;
		while (true)
		{
			transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.1f, Mathf.PingPong(percent * speed, 1));
			percent += Time.deltaTime;
			yield return null;
		}
	}
	IEnumerator Detonate(float time)
	{
		yield return new WaitForSeconds(time);

		RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, 0, layerMask, QueryTriggerInteraction.UseGlobal);
		foreach (RaycastHit i in hits)
		{
			LivingEntity item = i.collider.GetComponent<LivingEntity>();

			if (item == null) continue;
			item.takeDamage(1);
			damaged.Add(item);
		}
		Destroy(gameObject);
		Destroy(Instantiate(effect, transform.position, transform.rotation), 3f);
		AudioManager.instance.PlaySound(clip: "Explosion");

	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}


}
