using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
	public float moveSpeed = 5;
	public Camera viewCamera;
	public Spawner spawner;
	public Menus menus;
	PlayerController controller;
	GunController gunController;


	[HideInInspector] public Vector3 aimPoint;
	public static bool isPaused = false;



	// Start is called before the first frame update

	private void Awake()
	{
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		if (spawner != null)
			spawner.OnNewWave += OnNewWave;
	}
	override protected void Die()
	{
		AudioManager.instance.PlaySound(clip: "Player Death");
		base.Die();
	}

	private void Update()
	{
		// Pause
		if (Input.GetButtonDown("Pause"))
		{
			if (isPaused)
			{
				menus.Resume();
				return;
			}
			menus.PauseGame();
		}
		if (isPaused)
			return;

		// fall
		if (transform.position.y < -10)
		{
			Die();
		}
		// movement Input
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move(moveVelocity);

		// Aim Input
		Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
		Plane ground = new Plane(Vector3.up, Vector3.up * gunController.gunHeight);


		if (ground.Raycast(ray, out float rayDistance))
		{
			aimPoint = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, aimPoint, Color.red);

			controller.LookAt(aimPoint);
			if ((new Vector2(aimPoint.x, aimPoint.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 4)
			{
				gunController.Aim(aimPoint);

			}

		}

		// Weapon Input
		if (Input.GetMouseButton(0))
		{
			gunController.OnTriggerHold();
		}
		if (Input.GetMouseButtonUp(0))
		{
			gunController.OnTriggerRelease();
		}

		if (Input.GetButtonDown("Reload"))
		{
			gunController.Reload();
		}
		if (Input.GetButtonDown("Grenade"))
		{
			gunController.ThrowGrenade();
		}






	}


	void OnNewWave(int wave) => health = startingHealth;

	public void GainHealth(float val)
	{
		if (health + val > startingHealth)
		{
			health = startingHealth;
			return;
		}
		health += val;
	}
	public void GainSpeed(float val)
	{
		StartCoroutine(SpeedBoost(val, 5));
	}
	IEnumerator SpeedBoost(float boost, float duration)
	{
		moveSpeed += boost;
		yield return new WaitForSeconds(duration);
		moveSpeed -= boost;
	}
}
