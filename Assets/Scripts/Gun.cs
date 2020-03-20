using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public Transform muzzle;
	public Transform shell;
	public Transform shellEjector;
	public Projectile projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	MuzzleFlash muzzleFlash;

	float nextShotTime;
	Projectile newProjectile;
	private void Start()
	{
		muzzleFlash = GetComponent<MuzzleFlash>();
	}
	public void Shoot()
	{
		if (Time.time > nextShotTime)
		{
			nextShotTime = Time.time + msBetweenShots / 1000;
			newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed(muzzleVelocity);

			Instantiate(shell, shellEjector.position, shellEjector.rotation);
			muzzleFlash.Activate();
		}
	}
}
