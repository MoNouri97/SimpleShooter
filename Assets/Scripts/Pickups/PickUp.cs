using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
	public float bounceHeight;
	public float rotateSpeed;
	public GameObject pickUpEffect;
	public Vector3 rotateAxe = Vector3.up;

	void Update()
	{
		transform.Rotate(axis: rotateAxe, angle: 1f * rotateSpeed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Player"))
		{
			print("player");
			Destroy(Instantiate(pickUpEffect, transform.position, transform.rotation), 3f);
			Equip();
			Destroy(gameObject);
		}
	}

	protected virtual void Equip()
	{
		// GunController.instance.EquipGun(gameObject.GetComponent<Gun>());
	}
}
