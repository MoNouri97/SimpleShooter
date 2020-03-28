using UnityEngine;


public class Inventory : MonoBehaviour
{
	GunController gunController;
	private void Awake()
	{
		Spawner spawner = FindObjectOfType<Spawner>();
		if (spawner != null)
		{
			spawner.OnNewWave += EquipWaveGun;
		}

		gunController = GetComponent<GunController>();
	}

	void EquipWaveGun(int waveIndex) => gunController.EquipGun(waveIndex);
}