using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicParticules : MonoBehaviour
{

	void Start()
	{
		Texture sprite = GetComponentInChildren<PickUp>().sprite;
		Material material = GetComponent<Renderer>().material;
		material.mainTexture = sprite;
		// material.SetColor("_EmissionColor");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
