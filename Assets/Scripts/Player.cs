using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
	public float moveSpeed = 5;
	float moveSpeedInit;
	public Camera viewCamera;
	public Spawner spawner;
	public Menus menus;
	PlayerController controller;
	GunController gunController;


	public Vector3 aimPoint { get; set; }
	public static bool isPaused = false;

	public GameObject trail;
	Coroutine boost = null;


	// Start is called before the first frame update

	private void Awake()
	{
		moveSpeedInit = moveSpeed;
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		if (spawner != null)
			spawner.OnNewWave += OnNewWave;
	}
	override protected void Die(bool permenant = true)
	{
		AudioManager.instance.PlaySound(clip: "Player Death");
		base.Die(false);
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


		if (moveSpeed > moveSpeedInit && moveVelocity != Vector3.zero)
		{
			// boost activated
			Vector3 position = transform.position;
			position.y = 0;

			Destroy(Instantiate(trail, position, Quaternion.identity), .7f);

		}

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
		if (Input.GetMouseButton(1))
		{
			Time.timeScale = 0.2f;
		}
		if (Input.GetMouseButtonUp(1))
		{
			Time.timeScale = 1f;
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

	[ContextMenu("speed")]
	public void GainSpeed(float val = 5)
	{

		//check for an already active boost
		if (moveSpeed > moveSpeedInit && boost != null)
		{
			// boost Refill;
			moveSpeed = moveSpeedInit;
			StopCoroutine(boost);
			print("boost refill " + moveSpeed);

		}
		boost = StartCoroutine(SpeedBoost(val, 5));
	}
	IEnumerator SpeedBoost(float boost, float duration)
	{
		moveSpeed += boost;
		yield return new WaitForSeconds(duration);
		moveSpeed -= boost;
	}
	override public void Resurrect()
	{
		//cancel boosts;
		StopAllCoroutines();
		moveSpeed = moveSpeedInit;
		base.Resurrect();
	}

}
