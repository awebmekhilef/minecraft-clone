using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
	TMP_Text _text;

	Player _player;
	World _world;

	bool _isEnabled;

	void Start()
	{
		_text = GetComponent<TMP_Text>();
		_player = FindObjectOfType<Player>();
		_world = World.Instance;
	}

	void Update()
	{
		// Toggle debug text
		if (Input.GetKeyDown(KeyCode.F3))
		{
			_isEnabled = !_isEnabled;
			_text.enabled = _isEnabled;
		}

		if (!_isEnabled)
			return;

		Vector3 playerPos = _player.transform.position;
		Vector2Int playerChunk = _world.PlayerChunk;

		_text.text =
			$"Player: {playerPos.x:0.000} / {playerPos.y:0.000} / {playerPos.z:0.000}\n" +
			$"Chunk: {playerChunk.x} {playerChunk.y}";
	}
}
