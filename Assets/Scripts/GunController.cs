using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	Gun equippedGun;
	public Transform weaponHold;
	public Gun startingGun;
	public float gunHeight { get => weaponHold.position.y; }

	private void Start()
	{
		if (startingGun != null)
		{
			EquipGun(startingGun);
		}
	}
	public void EquipGun(Gun gunToEquip)
	{

		if (equippedGun != null)
		{
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
		equippedGun.transform.parent = weaponHold;
		equippedGun.transform.localPosition = Vector3.zero;
		equippedGun.transform.localRotation = Quaternion.identity;

	}

	public void OnTriggerHold()
	{
		if (equippedGun != null)
		{
			equippedGun.OnTriggerHold();
		}
	}
	public void OnTriggerRelease()
	{
		if (equippedGun != null)
		{
			equippedGun.OnTriggerRelease();
		}
	}
	public void Aim(Vector3 point)
	{
		if (equippedGun != null)
		{
			equippedGun.Aim(point);
		}
	}
	public void Reload()
	{
		if (equippedGun != null)
		{
			equippedGun.Reload();
		}
	}



}
