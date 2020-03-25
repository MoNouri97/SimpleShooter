using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public ParticleSystem deathEffect;
	public float startingHealth;
	public float health { get; protected set; }
	protected bool dead;
	public event System.Action OnDeath;

	virtual protected void Start()
	{
		health = startingHealth;
	}

	public virtual void takeHit(float damage, Vector3 hirPoint, Vector3 direction)
	{
		if (health <= damage)
		{
			Destroy(
				Instantiate(deathEffect.gameObject, hirPoint, Quaternion.FromToRotation(Vector3.forward, direction)),
				 deathEffect.main.startLifetime.constant
			);
		}
		takeDamage(damage);
	}
	public virtual void takeDamage(float damage)
	{
		health -= damage;
		if (health <= 0 && !dead)
		{
			Die();
		}
	}


	[ContextMenu("Die")]
	protected virtual void Die()
	{
		if (OnDeath != null)
		{
			OnDeath();
		}
		dead = true;
		Destroy(gameObject);
	}

}
