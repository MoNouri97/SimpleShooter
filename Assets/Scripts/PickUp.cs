using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
	public bool isEquipped { get; protected set; }
	public float rotateSpeed;

	void FixedUpdate()
	{
		if (isEquipped)
			return;
		transform.Rotate(Vector3.up, 1f * rotateSpeed);
	}
	public void Equip()
	{
		isEquipped = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isEquipped) return;
		if (other.tag.Equals("Player"))
		{
			GunController.instance.EquipGun(gameObject.GetComponent<Gun>(), pickup: true);
		}
	}
}
