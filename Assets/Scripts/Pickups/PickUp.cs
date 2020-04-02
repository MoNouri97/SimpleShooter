using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
	public float bounceHeight;
	public float rotateSpeed;
	public GameObject pickUpEffect;
	Vector3 rotateAxe = new Vector3(x: 1, y: 1, z: .5f);
	public Texture sprite;
	public Transform render;
	public Color color;

	void Start()
	{
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
		particleSystem.GetComponent<Renderer>().material.mainTexture = sprite;

		var psr = particleSystem.GetComponent<ParticleSystemRenderer>();
		Material mat = psr.material;
		mat.SetColor("_EmissionColor", color);
		render.GetComponent<Renderer>().material.color = color;
		Destroy(gameObject, 3f);
		/*trails test*/

		// Material psrTrailMat = new Material(psr.trailMaterial);
		// psrTrailMat.SetColor("_Color", trailsColor);
		// psrTrailMat.SetColor("_EmissionColor", trailsColor);
		// psr.trailMaterial = psrTrailMat;

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
