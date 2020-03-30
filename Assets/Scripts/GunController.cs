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
	public static GunController instance;

	public Grenade grenade;



	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

	}
	private void Start()
	{
		if (startingGun == null)
			return;
		EquipGun(startingGun);
	}
	public void EquipGun(Gun gunToEquip)
	{
		if (equippedGun != null)
		{
			if (equippedGun == gunToEquip)
			{
				return;
			}
			Destroy(equippedGun.gameObject);
		}

		equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);

		equippedGun.gameObject.transform.parent = weaponHold;
		equippedGun.gameObject.transform.localPosition = Vector3.zero;
		equippedGun.gameObject.transform.localRotation = Quaternion.identity;
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

	public void ThrowGrenade()
	{
		Instantiate(grenade, weaponHold.position, weaponHold.rotation);
	}
}
