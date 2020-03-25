using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	public Gun equippedGun { get; private set; }
	public Transform weaponHold;
	public Gun startingGun;
	public Gun[] guns;
	public float gunHeight { get => weaponHold.position.y; }



	private void Start()
	{
		EquipGun(guns[0]);
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
	public void EquipGun(int gunIndex)
	{
		EquipGun(guns[gunIndex]);
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
