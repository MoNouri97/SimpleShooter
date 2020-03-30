
using UnityEngine;

public class PostP : MonoBehaviour
{
	public GameObject highSettings;
	public GameObject lowSettings;
	public static PostP instance;
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	[ContextMenu("gfx")]
	public void SetQuality(bool val)
	{
		highSettings.SetActive(val);
		lowSettings.SetActive(!val);
	}
}
