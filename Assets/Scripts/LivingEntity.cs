using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public float startingHealth;
	float health;
	private bool dead;
	public event System.Action OnDeath;

	virtual protected void Start()
	{
		health = startingHealth;
	}

	public void takeHit(float damage, ContactPoint hit)
	{
		//TODO do something with hit
		takeDamage(damage);
	}
	public void takeDamage(float damage)
	{
		health -= damage;
		if (health <= 0 && !dead)
		{
			Die();
		}
	}


	protected void Die()
	{
		if (OnDeath != null)
		{
			OnDeath();
		}
		dead = true;
		Destroy(gameObject);
	}

}
