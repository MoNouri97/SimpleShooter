using UnityEngine;


public class Inventory : MonoBehaviour
{
	GunController gunController;
	private void Awake()
	{
		FindObjectOfType<Spawner>().OnNewWave += EquipWaveGun;
		gunController = GetComponent<GunController>();
	}

	void EquipWaveGun(int waveIndex) => gunController.EquipGun(waveIndex);
}