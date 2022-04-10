using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] float _gravity;
	[SerializeField] float _jumpHeight;
	[SerializeField] float _speed;

	[Header("Mouse")]
	[SerializeField] float _mouseSensetivity;
	[SerializeField] Transform _lookRoot;

	CharacterController _controller;
	Vector3 _velocity;
	float _xRotation;

	BlockID _heldBlock = BlockID.Dirt;

	void Start()
	{
		transform.position = new Vector3(0, Chunk.Height + 1, 0);

		_controller = GetComponent<CharacterController>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Look();
		Movement();
		SelectBlock();
		MouseControl();
	}

	void Look()
	{
		Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		_xRotation -= mouseDelta.y * _mouseSensetivity;
		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		transform.Rotate(Vector3.up, mouseDelta.x * _mouseSensetivity);
		_lookRoot.localRotation = Quaternion.Euler(Vector3.right * _xRotation);
	}

	void Movement()
	{
		bool isGrounded = _controller.isGrounded;
		if (isGrounded && _velocity.y < 0f)
			_velocity.y = 0f;

		Vector3 movement = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
		_controller.Move(_speed * Time.deltaTime * movement);

		if (Input.GetButtonDown("Jump") && isGrounded)
			_velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);

		_velocity.y += _gravity * Time.deltaTime;
		_controller.Move(_velocity * Time.deltaTime);
	}

	void MouseControl()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (Physics.Raycast(_lookRoot.position, _lookRoot.forward, out var hit))
			{
				Vector3Int blockPos = new Vector3Int(
					Mathf.FloorToInt(Util.MoveWithinBlock(hit.point.x + Chunk.Width / 2f, hit.normal.x, false)),
					Mathf.FloorToInt(Util.MoveWithinBlock(hit.point.y, hit.normal.y, false)),
					Mathf.FloorToInt(Util.MoveWithinBlock(hit.point.z + Chunk.Width / 2f, hit.normal.z, false))
				);

				World.Instance.SetBlock(blockPos.x, blockPos.y, blockPos.z, BlockID.Air);
			}
		}
		else if (Input.GetButtonDown("Fire2"))
		{
			if (Physics.Raycast(_lookRoot.position, _lookRoot.forward, out var hit))
			{
				Vector3Int blockPos = new Vector3Int(
					Mathf.FloorToInt(Util.MoveWithinBlock(hit.point.x + Chunk.Width / 2f, hit.normal.x, true)),
					Mathf.FloorToInt(Util.MoveWithinBlock(hit.point.y, hit.normal.y, true)),
					Mathf.FloorToInt(Util.MoveWithinBlock(hit.point.z + Chunk.Width / 2f, hit.normal.z, true))
				);

				World.Instance.SetBlock(blockPos.x, blockPos.y, blockPos.z, _heldBlock);
			}
		}
	}

	void SelectBlock()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			_heldBlock = BlockID.Brick;
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			_heldBlock = BlockID.Glass;
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			_heldBlock = BlockID.WoodLog;
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			_heldBlock = BlockID.WoodPlank;
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			_heldBlock = BlockID.Cobblestone;
	}
}
