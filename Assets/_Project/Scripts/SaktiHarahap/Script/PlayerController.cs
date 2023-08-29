using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform playerView;
	private bool _facingRight = true;
	private const float Speed = 10f;

	void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			MoveHorizontal(Vector3.left);
			if (_facingRight)
			{
				_facingRight = false;
				UpdateFacing();
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			MoveHorizontal(Vector3.right);
			if (!_facingRight)
			{
				_facingRight = true;
				UpdateFacing();
			}
		}
	}

	private void MoveHorizontal(Vector3 direction)
	{
		transform.position += (direction * Speed * Time.deltaTime);
	}

	private void UpdateFacing()
	{
		playerView.localScale = new Vector3(_facingRight ? 1 : -1, 1, 1);
	}
}
