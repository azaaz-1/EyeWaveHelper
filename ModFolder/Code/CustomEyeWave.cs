using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste.Mod.Entities;
using Directions = Celeste.Mod.EyeWaveHelper.Entities.TempleBigEyeWaveController.Directions;

namespace Celeste.Mod.EyeWaveHelper.Entities;

[CustomEntity("EyeWaveHelper/TempleBigEyeWaveController"), Tracked]

[Pooled]
public class CustomEyeWave : Entity
{
	private MTexture distortionTexture;

	private float distortionAlpha;

	private bool hasHitPlayer;

	private Directions direction;

	public CustomEyeWave()
	{
		base.Depth = -1000000;
		MTexture mTexture = GFX.Game["util/displacementcirclehollow"];
		if (direction == Directions.Left)
        {
			base.Collider = new Hitbox(48f, 200f, -30f, -100f);
			distortionTexture = mTexture.GetSubtexture(0, 0, mTexture.Width / 2, mTexture.Height);
		}
		else if (direction == Directions.Up)
        {
			base.Collider = new Hitbox(200f, 48f, -100f, -30f);
			distortionTexture = mTexture.GetSubtexture(0, 0, mTexture.Width, mTexture.Height / 2);
		}
		Add(new PlayerCollider(OnPlayer));
		Add(new DisplacementRenderHook(RenderDisplacement));
	}

	public CustomEyeWave Init(Vector2 position, float distance, Directions direction)
	{
		if (direction == Directions.Up)
			Position = position + new Vector2(0f, distance);
		else if (direction == Directions.Left)
			Position = position + new Vector2(distance, 0f);
		this.direction = direction;
		Collidable = true;
		distortionAlpha = 0f;
		hasHitPlayer = false;

		MTexture mTexture = GFX.Game["util/displacementCircleHollow"];
		if (direction == Directions.Left)
		{
			base.Collider = new Hitbox(48f, 200f, -30f, -100f);
			distortionTexture = mTexture.GetSubtexture(0, 0, mTexture.Width / 2, mTexture.Height);
		}
		else if (direction == Directions.Up)
		{
			base.Collider = new Hitbox(200f, 48f, -100f, -30f);
			distortionTexture = mTexture.GetSubtexture(0, 0, mTexture.Width, mTexture.Height / 2);
		}

		return this;
	}

	public override void Update()
	{
		base.Update();
		//Logger.Log("wave", direction.ToString());
		if (direction == Directions.Left)
        {
			base.X -= 300f * Engine.DeltaTime;
			distortionAlpha = Calc.Approach(distortionAlpha, 1f, Engine.DeltaTime * 4f);
			if (base.X < (float)(SceneAs<Level>().Bounds.Left - 20))
			{
				RemoveSelf();
			}
		}
		else if (direction == Directions.Up)
        {
			base.Y -= 300f * Engine.DeltaTime;
			distortionAlpha = Calc.Approach(distortionAlpha, 1f, Engine.DeltaTime * 4f);
			if (base.Y < (float)(SceneAs<Level>().Bounds.Top - 20))
			{
				RemoveSelf();
			}
		}
	}

	private void RenderDisplacement()
	{
		//MTexture mTexture = GFX.Game["util/111"];
		//mTexture.DrawCentered(Position);
		if (direction == Directions.Left)
			distortionTexture.DrawCentered(Position, Color.White * 0.8f * distortionAlpha, new Vector2(0.9f, 1.5f));
		else if (direction == Directions.Up)
			distortionTexture.DrawCentered(Position, Color.White * 0.8f * distortionAlpha, new Vector2(1.5f, 0.9f));
	}


	private void OnPlayer(Player player)
	{
		if (player.StateMachine.State != 2)
		{
			if (direction == Directions.Left)
			{
				player.Speed.X = -100f;
				if (player.Speed.Y > 30f)
				{
					player.Speed.Y = 30f;
				}
			}
			else if (direction == Directions.Up)
			{
				player.Speed.Y = -100f;
			}
			if (!hasHitPlayer)
			{
				Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
				Audio.Play("event:/game/05_mirror_temple/eye_pulse", player.Position);
				hasHitPlayer = true;
			}
		}
	}
}
