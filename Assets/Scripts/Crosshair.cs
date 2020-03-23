using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
	public Player player;

	public float rotateSpeed;

	void Start()
	{
		Cursor.visible = false;
	}

	void Update()
	{
		transform.position = player.aimPoint;
		transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
	}
}
