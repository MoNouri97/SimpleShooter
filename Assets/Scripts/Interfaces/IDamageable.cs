using UnityEngine;
interface IDamageable
{
	void takeHit(float damage, Vector3 hirPoint, Vector3 direction);
	void takeDamage(float damage);

}