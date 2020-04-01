using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : PickUp
{
	protected override void Equip(Player player)
	{
		GunController.instance.AddGrenade();
	}
}
