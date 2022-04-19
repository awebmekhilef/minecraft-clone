using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] float _gravity;
	[SerializeField] float _jumpHeight;
	[SerializeField] float _speed;

	[Header("Mouse")]
	[SerializeField] float _mouseSensetivity;
	[SerializeField] Transform _lookRoot;

	[Header("Block")]
	[SerializeField] Transform _highlightBlock;
	[SerializeField] float _blockWaitTime;

	CharacterController _controller;
	PlayerInventory _inventory;
	Vector3 _velocity;
	float _xRotation;
	float _blockTimer;

	void Start()
	{
		World.Instance.OnGeneratedInitialChunks += Spawn;

		_controller = GetComponent<CharacterController>();
		_inventory = GetComponent<PlayerInventory>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Look();
		Block();
		Movement();
	}

	void Spawn()
	{
		transform.position = new Vector3(Chunk.Width / 2f, Chunk.Height + 1, Chunk.Width / 2f);
		_velocity = Vector3.zero;
	}

	void Look()
	{
		Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		_xRotation -= mouseDelta.y * _mouseSensetivity;
		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		transform.Rotate(Vector3.up, mouseDelta.x * _mouseSensetivity);
		_lookRoot.localRotation = Quaternion.Euler(Vector3.right * _xRotation);
	}

	void Block()
	{
		_highlightBlock.gameObject.SetActive(false);

		if (Physics.Raycast(_lookRoot.position, _lookRoot.forward, out var hit))
		{
			// Move a small amount into block to prevent float accuracy issues with Mathf.Floor
			Vector3Int blockPos = Vector3Int.FloorToInt(hit.point - hit.normal * 0.1f);

			// Set highlight block position
			_highlightBlock.position = blockPos;
			_highlightBlock.gameObject.SetActive(true);

			// Update block placing/remove timer
			_blockTimer += Time.deltaTime;

			if (_blockTimer < _blockWaitTime)
				return;

			if (Input.GetButton("Destroy"))
			{
				BlockID prevBlock = World.Instance.GetBlock(blockPos.x, blockPos.y, blockPos.z);

				// it would be a cursed game if you could break bedrock..
				if (prevBlock == BlockID.Bedrock)
					return;

				_inventory.AddBlock(prevBlock);

				World.Instance.SetBlock(blockPos.x, blockPos.y, blockPos.z, BlockID.Air);

				_blockTimer = 0f;
			}
			else if (Input.GetButton("Place"))
			{
				BlockID selectedBlock = _inventory.PlaceBlock();

				if (selectedBlock == BlockID.Air)
					return;

				Vector3Int adjBlockPos = blockPos + Vector3Int.FloorToInt(hit.normal);

				World.Instance.SetBlock(adjBlockPos.x, adjBlockPos.y, adjBlockPos.z, selectedBlock);

				_blockTimer = 0f;
			}
		}
	}

	void Movement()
	{
		bool isGrounded = _controller.isGrounded;
		if (isGrounded && _velocity.y < 0f)
			_velocity.y = 0f;

		Vector3 movement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
		_controller.Move(_speed * Time.deltaTime * movement);

		if (Input.GetButton("Jump") && isGrounded)
			_velocity.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);

		_velocity.y += _gravity * Time.deltaTime;
		_controller.Move(_velocity * Time.deltaTime);
	}
}
