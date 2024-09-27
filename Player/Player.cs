using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
	public const float Speed = 170.0f;
	public const float JumpVelocity = -300.0f;
	private static AnimatedSprite2D animationPlayer;
	private Godot.Vector2 velocity;
	private Godot.Vector2 particlesScale;
	private Godot.Vector2 particlesPosition;
	private static CpuParticles2D particles;

	public override void _PhysicsProcess(double delta)
	{
		// Vector2 position = Position;
		animationPlayer = GetTree().GetNodesInGroup("Player")[0].GetChild(1) as AnimatedSprite2D;
		particles = GetTree().GetNodesInGroup("Player")[0].GetChild(2) as CpuParticles2D;
		particlesScale = particles.Scale;
		particlesPosition = particles.Position;
		particles.Emitting = false;
		velocity = Velocity;
		Animations();

		if (!IsOnFloor())
		{
			if (velocity.Y > 0)
			{
				animationPlayer.Play("fall");
			}
			velocity += GetGravity() * (float)delta;

		}

		if (Input.IsActionJustPressed("ui_space") && IsOnFloor())
		{
			animationPlayer.Play("jump");
			velocity.Y = JumpVelocity;
		}


		Godot.Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Godot.Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (velocity.X > 0)
			{
				animationPlayer.FlipH = false;
			}
			else
			{
				animationPlayer.FlipH = true;
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		}

		Velocity = velocity;
		MoveAndSlide();
	}
	private void Animations()
	{

		if (velocity.X != 0)
		{
			animationPlayer.Play("run");
			
			if (IsOnFloor())

			{
				particles.Emitting = true;

				if (velocity.X > 0)
				{
					particlesScale.X = (float)0.6;
					particlesPosition.X = -4;
					particles.Scale = particlesScale;
					particles.Position = particlesPosition;
				}
				else
				{
					particlesScale.X = (float)-0.6;
					particlesPosition.X = 4;
					particles.Scale = particlesScale;
					particles.Position = particlesPosition;
				}
			}
		}
		else
		{
			animationPlayer.Play("idle");
		}
	}

	public static void _on_cpu_particles_2d_finished()
	{
		particles.Emitting = false;
	}




}