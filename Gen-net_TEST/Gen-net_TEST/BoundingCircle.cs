using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gen_net_TEST
{

	public delegate void collionEventHandler(CollisionInfo info);

	public class BoundingCircle
	{
		public Vector2 center;
		public float radius;
		public event collionEventHandler Collision;
		public IndividualType type;

		public BoundingCircle(Vector2 center, float radius)
		{
			this.center = center;
			this.radius = radius;
		}

		public ContainmentType Contains(BoundingCircle other)
		{
			float d = (center - other.center).Length();
			ContainmentType type;
			if (d > (radius + other.radius))
				type = ContainmentType.Disjoint;
			else if ((d + other.radius) < radius)
				type = ContainmentType.Contains;
			else
				type = ContainmentType.Intersects;
			if (type != ContainmentType.Disjoint)
				Collision(new CollisionInfo(type, center - other.center, other.type));
			return type;
		}

	}
}
