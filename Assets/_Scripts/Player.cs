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

	CharacterController _controller;
	Vector3 _velocity;
	float _xRotation;

	void Start()
	{
		_controller = GetComponent<CharacterController>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Look();
		Movement();
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
}