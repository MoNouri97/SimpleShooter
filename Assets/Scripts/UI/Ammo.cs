using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
	#region Refs

	[Header("Ammo")]
	public Text currentAmmo;
	public Text maxAmmo;

	[Header("Refs")]
	Spawner spawner;
	GunController gunController;
	#endregion

	void Start()
	{
		spawner = Spawner.instance;
		gunController = GunController.instance;
		if (spawner != null)
			spawner.OnNewWave += OnNewWave;
	}
	private void Update()
	{
		if (gunController.equippedGun == null) return;
		currentAmmo.text = gunController.equippedGun.remainingInMag.ToString("D2");
	}


	void OnNewWave(int wave)
	{
		maxAmmo.text = "/" + gunController.equippedGun.magSize; ;
	}


}
