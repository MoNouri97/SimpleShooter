using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public enum FireMode { Auto, Burst, Single };
	public FireMode fireMode;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	public int burstCount;


	public Transform[] muzzle;
	public Transform shell;
	public Transform shellEjector;
	public Projectile projectile;
	MuzzleFlash muzzleFlash;

	float nextShotTime;
	int shotRemainingInBurst;
	Projectile newProjectile;
	bool triggerReleased;
	private void Start()
	{
		muzzleFlash = GetComponent<MuzzleFlash>();
		shotRemainingInBurst = burstCount;
	}
	void Shoot()
	{

		if (Time.time <= nextShotTime)
		{
			return;
		}
		if (fireMode == FireMode.Burst)
		{
			if (shotRemainingInBurst == 0)
			{
				return;
			}
			shotRemainingInBurst--;
		}
		if (fireMode == FireMode.Single && !triggerReleased)
		{
			return;
		}

		for (int i = 0; i < muzzle.Length; i++)
		{
			nextShotTime = Time.time + msBetweenShots / 1000;
			newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
			newProjectile.SetSpeed(muzzleVelocity);
		}

		Instantiate(shell, shellEjector.position, shellEjector.rotation);
		muzzleFlash.Activate();
	}

	public void OnTriggerHold()
	{
		Shoot();
		triggerReleased = false;
	}
	public void OnTriggerRelease()
	{
		triggerReleased = true;
		shotRemainingInBurst = burstCount;

	}
}
