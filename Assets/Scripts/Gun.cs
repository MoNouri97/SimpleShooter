using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Gun : MonoBehaviour
{
	#region vars

	public enum FireMode { Auto, Burst, Single };
	public FireMode fireMode;
	public int magSize;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	public int burstCount;
	public float reloadTime = .3f;
	public float maxReloadAngle = 30;

	[Header("Recoil")]
	public Vector2 kickMinMax = new Vector2(.5f, .2f);
	public Vector2 recoilAngleMinMax = new Vector2(3, 5);
	public float kickSettleTime = .1f;
	public float angleSettleTime = .1f;

	[Header("Effects")]
	public Transform[] muzzle;
	public Transform shell;
	public Transform shellEjector;
	public Projectile projectile;
	[Header("Audio")]
	public AudioClip shootAudio;
	public AudioClip reloadAudio;
	MuzzleFlash muzzleFlash;

	float nextShotTime;
	int shotRemainingInBurst;
	public int remainingInMag { get; private set; }
	Projectile newProjectile;
	bool triggerReleased;
	bool isReloading;


	Vector3 recoilVelocity = Vector3.zero;
	float recoilAngle;
	float recoilAngleVelocity;

	#endregion

	private void Start()
	{
		muzzleFlash = GetComponent<MuzzleFlash>();
		shotRemainingInBurst = burstCount;
		remainingInMag = magSize;
	}
	private void LateUpdate()
	{
		#region animate recoil*/
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilVelocity, kickSettleTime);
		recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilAngleVelocity, angleSettleTime);
		transform.localEulerAngles += Vector3.left * recoilAngle;
		#endregion

		if (!isReloading && remainingInMag == 0)
		{
			Reload();
		}
	}
	public void Aim(Vector3 point)
	{
		if (isReloading) return;
		transform.LookAt(point);
	}
	void Shoot()
	{
		#region checks : time & firemode
		if (Time.time <= nextShotTime || remainingInMag == 0 || isReloading) return;

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

		#endregion
		remainingInMag--;

		for (int i = 0; i < muzzle.Length; i++)
		{
			nextShotTime = Time.time + msBetweenShots / 1000;
			newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
			newProjectile.SetSpeed(muzzleVelocity);
		}

		Instantiate(shell, shellEjector.position, shellEjector.rotation);
		muzzleFlash.Activate();

		transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
		recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
		recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

		AudioManager.instance.PlaySound(shootAudio);
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

	public void Reload()
	{
		if (remainingInMag == magSize || isReloading) return;
		StartCoroutine(AnimateReload());
	}

	IEnumerator AnimateReload()
	{
		isReloading = true;
		yield return new WaitForSeconds(.2f);
		float percent = 0;
		float speed = 1f / reloadTime;
		float reloadAngle;
		Vector3 initRotation = transform.localEulerAngles;
		float interpolation;
		bool audio = false;
		while (percent < 1)
		{
			percent += Time.deltaTime * speed;

			interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
			reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
			transform.localEulerAngles = initRotation + Vector3.left * reloadAngle;

			if (percent >= .5f && !audio)
			{
				audio = true;
				AudioManager.instance.PlaySound(reloadAudio);
			}

			yield return null;
		}

		remainingInMag = magSize;
		isReloading = false;
	}

}
