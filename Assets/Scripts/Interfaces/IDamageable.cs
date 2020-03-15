using UnityEngine;
interface IDamageable
{
	void takeHit(float damage, ContactPoint hit);
	void takeDamage(float damage);

}