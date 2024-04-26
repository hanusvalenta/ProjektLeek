using Godot;
using System;

public class Tom : KinematicBody
{
	[Export]
	public float Speed = 2.5f;

	[Export]
	public float JumpSpeed = 10.0f;

	[Export]
	public float JumpAcceleration = 0.2f;

	[Export]
	public float Gravity = 50f;

	[Export]
	public float RotationSpeed = 1.0f; // Speed of rotation in degrees per second

	private Vector3 velocity = new Vector3();
	private Camera camera; // Reference to the camera node

	private bool isJumping = false;

	public override void _Ready()
	{
		GD.Print("Tom ready...");
		camera = GetNode
		("Camera") as Camera; // Assuming the camera is named "Camera"
	}

	public override void _PhysicsProcess(float delta)
	{
		if (camera == null)
			return; // Camera reference not found, exit function

		Vector3 movementInput = new Vector3();

		// Get camera rotation
		Vector3 cameraRotation = camera.GlobalTransform.basis.GetEuler();
		float cameraYaw = cameraRotation.y;

		// Adjust movement input based on camera rotation
		if (Input.IsActionPressed("move_forward"))
			movementInput -= new Vector3((float)Math.Sin(cameraYaw), 0, (float)Math.Cos(cameraYaw));
		if (Input.IsActionPressed("move_backward"))
			movementInput += new Vector3((float)Math.Sin(cameraYaw), 0, (float)Math.Cos(cameraYaw));
		if (Input.IsActionPressed("move_left"))
			movementInput -= new Vector3((float)Math.Cos(cameraYaw), 0, (float)Math.Sin(cameraYaw));
		if (Input.IsActionPressed("move_right"))
			movementInput += new Vector3((float)Math.Cos(cameraYaw), 0, (float)Math.Sin(cameraYaw));

		movementInput = movementInput.Normalized();

		velocity = movementInput * Speed;

		if (isJumping && velocity.y < 0)
		{
			isJumping = false;
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			velocity.y = JumpSpeed;
			isJumping = true;
		}

		if (isJumping && Input.IsActionPressed("jump"))
		{
			velocity.y += JumpAcceleration * delta;
		}

		velocity.y -= Gravity * delta;

		MoveAndSlide(velocity, Vector3.Up);

		// Rotate the player when E or Q is pressed
		if (Input.IsActionPressed("rotate_clockwise"))
		{
			RotateY(Mathf.Deg2Rad(90) * delta * RotationSpeed); // Rotate clockwise by 90 degrees
		}
		else if (Input.IsActionPressed("rotate_counterclockwise"))
		{
			RotateY(Mathf.Deg2Rad(-90) * delta * RotationSpeed); // Rotate counterclockwise by 90 degrees
		}
	}
}
