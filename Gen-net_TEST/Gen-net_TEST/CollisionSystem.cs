using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gen_net_TEST
{
	public class CollisionSystem: GameComponent
	{
		List<BoundingCircle> hitboxes;

		public CollisionSystem(Game game) : base(game)
		{
			hitboxes = new List<BoundingCircle>();
		}

		public void Add(BoundingCircle hitbox){
			hitboxes.Add(hitbox);
		}

		public void Add(List<BoundingCircle> hitboxes)
		{
			this.hitboxes.AddRange(hitboxes);
		}

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < hitboxes.Count; i++)
			{
				for (int j = i + 1; j < hitboxes.Count; j++)
				{
					hitboxes[i].Contains(hitboxes[j]);
				}
			}
				base.Update(gameTime);
		}
	}
}
