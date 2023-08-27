using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 6f;

	public float jumpSpeed = 8f;

	public float gravity = 20f;

	private Vector3 moveDir = Vector3.zero;

	private bool grounded;

	private void FixedUpdate()
	{
		if (grounded)
		{
			moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			moveDir = Quaternion.AngleAxis(base.transform.localEulerAngles.y, Vector3.up) * moveDir;
			moveDir *= speed;
			if (Input.GetButton("Jump"))
			{
				moveDir.y = jumpSpeed;
			}
		}
		moveDir.y -= gravity * Time.deltaTime;
		CharacterController component = GetComponent<CharacterController>();
		CollisionFlags collisionFlags = component.Move(moveDir * Time.deltaTime);
		grounded = (collisionFlags & CollisionFlags.Below) != 0;
	}
}
