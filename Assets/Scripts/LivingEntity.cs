using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public ParticleSystem deathEffect;
	public float startingHealth;
	public float health { get; protected set; }
	public bool dead { get; protected set; }
	public event System.Action OnDeath;
	public event System.Action OnBirth;

	virtual protected void Start()
	{
		health = startingHealth;
	}

	public virtual void takeHit(float damage, Vector3 hirPoint, Vector3 direction)
	{
		if (health <= damage)
		{
			AudioManager.instance.PlaySound(clip: "Enemy Death");
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
	protected virtual void Die(bool permenant = true)
	{
		if (OnDeath != null)
		{
			OnDeath();
		}
		dead = true;
		if (permenant)
		{
			Destroy(gameObject);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	public virtual void Resurrect()
	{
		dead = false;
		if (OnBirth != null)
		{
			OnBirth();
		}
		gameObject.SetActive(true);
		health = startingHealth;
	}
}
