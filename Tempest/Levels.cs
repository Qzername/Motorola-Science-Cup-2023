using VGE;

namespace Tempest
{
	public static class Levels
	{
		public static Point[] Circle =
		{
			new(-200, 0, 750),
			new(-150, 50, 750),
			new(-100, 100, 750),
			new(-50, 150, 750),
			
			new(0, 200, 750),
			new(50, 150, 750),
			new(100, 100, 750),
			new(150, 50, 750),

			new(200, 0, 750),
			new(150, -50, 750),
			new(100, -100, 750),
			new(50, -150, 750),

			new(0, -200, 750),
			new(-50, -150, 750),
			new(-100, -100, 750),
			new(-150, -50, 750),
		};
		
		public static float[] CircleRotations =
		{
			-45,
			-45,
			-45,
			-45,
			
			45,
			45,
			45,
			45,

			135,
			135,
			135,
			135,
			
			225,
			225,
			225,
			225,
		};	
	}
}
