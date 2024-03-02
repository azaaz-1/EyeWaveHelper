using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste.Mod.Entities;


namespace Celeste.Mod.EyeWaveHelper.Entities;

[CustomEntity("EyeWaveHelper/TempleBigEyeWaveController"), Tracked]

public class TempleBigEyeWaveController : Entity
{
	public enum Directions  {Up, Left };

	private float shockwaveTimer = 2f;
	private float waveInterval = 2f;
	private float distance = 50f;
	private Directions direction;
	private string flag;
	private float waveStrength;

    public TempleBigEyeWaveController(EntityData e, Vector2 offset)
        : base(e.Position + offset)
    {
		waveInterval = e.Float("maxInterval", 2f);
		distance = e.Float("distance", 50f);
		flag = e.Attr("flag");
		direction = e.Enum("direction", Directions.Up);
		waveStrength = e.Float("strength", 100f);
    }

    public override void Awake(Scene scene)
    {
        base.Awake(scene);
		shockwaveTimer = waveInterval;
    }
    public override void Update()
    {
        base.Update();
		//Logger.Log(shockwaveTimer.ToString(), waveInterval.ToString());
		if (base.SceneAs<Level>().Session.GetFlag(flag) == false)
        {
			shockwaveTimer = waveInterval;
			return;
        }
		if (shockwaveTimer > 0f)
		{
			Player entity = base.Scene.Tracker.GetEntity<Player>();
			shockwaveTimer -= Engine.DeltaTime;
			if (shockwaveTimer <= 0f)
			{
				if (entity != null)
				{
					shockwaveTimer = waveInterval;
					base.Scene.Add(Engine.Pooler.Create<CustomEyeWave>().Init(entity.Center, distance, direction, waveStrength));
				}

			}
		}
	}
}
