using UnityEngine;

public class SpeedPickup : PickUp
{
	override protected void Equip(Player player)
	{
		player.GainSpeed(5);
	}
}
