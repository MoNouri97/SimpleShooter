using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicParticules : MonoBehaviour
{

	void Start()
	{
		Texture sprite = GetComponentInChildren<PickUp>().sprite;
		GetComponent<Renderer>().material.mainTexture = sprite;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
