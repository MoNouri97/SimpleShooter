using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	Player player;
	public Vector3 offset;
	public float speed;
	public Vector3 originalPos;

	void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 targetPos = player.transform.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
	}
}
