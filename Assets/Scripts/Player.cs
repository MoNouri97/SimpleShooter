using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]        
[RequireComponent(typeof(GunController))]        
public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public Camera viewCamera;

    PlayerController controller;
    GunController gunController;


    // Start is called before the first frame update


    void Start()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
    }

    private void Update() {
        // movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);
        
        // Aim Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray,out float rayDistance))
        {
            Vector3 intersectPoint = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin,intersectPoint);
            controller.LookAt(intersectPoint);
        }

        // Weapon Input
        if (Input.GetMouseButton(0))
        {
			gunController.Shoot();
		}

    }

}
