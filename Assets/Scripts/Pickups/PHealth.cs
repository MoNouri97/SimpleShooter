using UnityEngine;

public class PHealth : PickUp
{
	override protected void Equip()
	{
		FindObjectOfType<Player>().GainHealth(5);
	}
}
