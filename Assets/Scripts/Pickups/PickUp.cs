using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
	public float bounceHeight;
	public float rotateSpeed;
	public GameObject pickUpEffect;
	public Vector3 rotateAxe = Vector3.up;
	public Texture sprite;
	public Transform render;
	public Color trailsColor;

	void Start()
	{
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
		particleSystem.GetComponent<Renderer>().material.mainTexture = sprite;

		var psr = particleSystem.GetComponent<ParticleSystemRenderer>();
		Material psrTrailMat = new Material(psr.trailMaterial);
		psrTrailMat.SetColor("_Color", trailsColor);
		psrTrailMat.SetColor("_EmissionColor", trailsColor);
		psr.trailMaterial = psrTrailMat;

		render.GetComponent<Renderer>().material.color = trailsColor;
	}
	void Update()
	{
		render.Rotate(axis: rotateAxe, angle: 1f * rotateSpeed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Player"))
		{
			Destroy(Instantiate(pickUpEffect, transform.position, transform.rotation), 3f);
			Equip(other.GetComponent<Player>());
			Destroy(gameObject);
		}
	}

	protected abstract void Equip(Player player);
}
