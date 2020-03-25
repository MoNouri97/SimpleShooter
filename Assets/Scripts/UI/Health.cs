using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	#region Refs
	public Slider healthBar;

	[Header("Refs")]
	public Player player;
	#endregion


	void Update()
	{
		healthBar.value = player.health / player.startingHealth;
	}


}
