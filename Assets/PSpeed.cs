using UnityEngine;

public class PSpeed : PickUp
{
	override protected void Equip()
	{
		FindObjectOfType<Player>().GainSpeed(5);
	}
}
