using VGE;

namespace Tempest
{
	public static class Levels
	{
		/*
			0
			100
			200
			300
			400
			500
			600
			700
			800
		*/
		public static Point[] Circle =
		{
			new(0, 100, 750),
			new(25, 125, 750),
			new(50, 150, 750),
			new(75, 175, 750),
			
			new(100, 200, 750),
			new(125, 175, 750),
			new(150, 150, 750),
			new(175, 125, 750),

			new(200, 100, 750),
			new(175, 75, 750),
			new(150, 50, 750),
			new(125, 25, 750),

			new(100, 0, 750),
			new(75, 25, 750),
			new(50, 50, 750),
			new(25, 75, 750),
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
