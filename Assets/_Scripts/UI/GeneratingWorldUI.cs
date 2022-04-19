using UnityEngine;

public class GeneratingWorldUI : MonoBehaviour
{
	void Start()
	{
		World.Instance.OnGeneratedInitialChunks += () => gameObject.SetActive(false);
	}
}
