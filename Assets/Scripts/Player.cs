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


	PlayerController controller;
	GunController gunController;


	[HideInInspector] public Vector3 aimPoint;



	// Start is called before the first frame update

	private void Awake()
	{
		controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		spawner.OnNewWave += OnNewWave;
	}
	override protected void Start()
	{
		base.Start();

	}

	private void Update()
	{
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


	}


	void OnNewWave(int wave) => health = startingHealth;
}
