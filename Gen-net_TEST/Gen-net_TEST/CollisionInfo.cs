using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gen_net_TEST
{

	public enum IndividualType { Critter, Spike, Sensor };

	public class CollisionInfo
	{
		public ContainmentType type;
		public Vector2 vector, normal;
		public IndividualType other;

		public CollisionInfo(ContainmentType type, Vector2 vector, IndividualType iType)
		{
			this.vector = vector;
			normal = new Vector2(vector.X, -vector.Y);
			this.type = type;
			other = iType;
		}
	}
}
