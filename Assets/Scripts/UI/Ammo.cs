using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
	#region Refs

	[Header("Ammo")]
	public Text currentAmmo;
	public Text maxAmmo;
	public TextMeshProUGUI grenadeCount;


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
		grenadeCount.text = gunController.grenadeCount.ToString("D2");
		if (gunController.equippedGun == null) return;
		currentAmmo.text = gunController.equippedGun.remainingInMag.ToString("D2");
	}


	void OnNewWave(int wave)
	{
		maxAmmo.text = "/" + gunController.equippedGun.magSize; ;
	}


}
