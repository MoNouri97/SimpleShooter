using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	#region Refs
	[Header("Refs")]
	public Slider healthBar;

	Player player;
	#endregion
	public float smoothSpeed = 1;
	private void start()
	{
		player = GetComponent<UI>().player;
	}

	void Update()
	{
		if (player == null)
		{
			player = GetComponent<UI>().player;
			healthBar.value = 0;
			return;
		}
		float v = player.health / player.startingHealth;
		healthBar.value = Mathf.Lerp(healthBar.value, v, Time.deltaTime * smoothSpeed);
	}


}
