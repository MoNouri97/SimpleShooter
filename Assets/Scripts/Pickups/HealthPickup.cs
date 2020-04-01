using UnityEngine;

public class HealthPickup : PickUp
{
	override protected void Equip(Player player)
	{
		player.GainHealth(5);
	}
}
