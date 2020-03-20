using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
	public GameObject holder;
	public Sprite[] sprites;
	public SpriteRenderer[] spritesRenderer;
	public float flashTime;
	void Start()
	{
		Deactivate();
	}
	public void Activate()
	{
		holder.SetActive(true);
		int randSprite = Random.Range(0, sprites.Length);
		for (int i = 0; i < spritesRenderer.Length; i++)
		{
			spritesRenderer[i].sprite = sprites[randSprite];
		}
		Invoke("Deactivate", flashTime);
	}
	void Deactivate()
	{
		holder.SetActive(false);
	}
}
