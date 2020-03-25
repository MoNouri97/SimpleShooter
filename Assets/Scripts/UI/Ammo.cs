using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
	#region Refs

	[Header("Ammo")]
	public Text currentAmmo;
	public Text maxAmmo;

	[Header("Refs")]
	public Spawner spawner;
	public GunController gunController;
	#endregion

	void Awake()
	{
		spawner.OnNewWave += OnNewWave;
	}
	private void Update()
	{
		currentAmmo.text = gunController.equippedGun.remainingInMag.ToString("D2");
	}


	void OnNewWave(int wave)
	{
		maxAmmo.text = "/" + gunController.equippedGun.magSize; ;
	}


}
