using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public float startingHealth = 1;
	float health;
	private bool dead;

	virtual protected void Start() => health = startingHealth;

	public void takeHit(float damage, RaycastHit hit)
	{
		health -= damage;
		if (health <= 0 && !dead)
		{
			Die();
		}
	}

	protected void Die()
	{
		dead = true;
		Destroy(gameObject);
	}

}
