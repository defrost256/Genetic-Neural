using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GEN_NET;

namespace Gen_net_TEST
{

	class Critter : Individual<float>, IUpdateable, IGameComponent
	{

		public Vector2 position
		{
			get;
			set
			{
				hitbox.center = value;
				position = value;
			}
		}
		Vector2 velocity;
		public float r;
		BoundingCircle hitbox;
		public BoundingCircle Hitbox
		{
			get { return hitbox; }
		}
		Sensor[] sensors;
		Thruster[] thrusters;


		public Critter() : base()
		{
			hitbox = new BoundingCircle(Vector2.Zero, 0f);
			velocity = Vector2.Zero;
		}

		public void Update(GameTime time)
		{
			position += velocity;
		}

		bool enabled;
		public bool Enabled
		{
			get { return enabled; }
		}

		public event EventHandler<EventArgs> EnabledChanged;

		public int UpdateOrder
		{
			get { return 0; }
		}

		public event EventHandler<EventArgs> UpdateOrderChanged;

		public void Initialize()
		{
			//TODO: implement if needed
		}
	}
}
