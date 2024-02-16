﻿using VGE;

namespace Tempest
{
	public static class Levels
	{
		// https://www.arcade-history.com/?n=tempest-upright-model&page=detail&id=2865
		public static Point[] Circle =
		[
			new(-200, 0, 750),
			new(-180, 80, 750),
			new(-140, 140, 750),
			new(-80, 180, 750),
			
			new(0, 200, 750),
			new(80, 180, 750),
			new(140, 140, 750),
			new(180, 80, 750),

			new(200, 0, 750),
			new(180, -80, 750),
			new(140, -140, 750),
			new(80, -180, 750),

			new(0, -200, 750),
			new(-80, -180, 750),
			new(-140, -140, 750),
			new(-180, -80, 750)
		];

		public static Point[] Square =
		[
			new(-200, 200, 750),
			new(-100, 200, 750),
			new(0, 200, 750),
			new(100, 200, 750),

			new(200, 200, 750),
			new(200, 100, 750),
			new(200, 0, 750),
			new(200, -100, 750),

			new(200, -200, 750),
			new(100, -200, 750),
			new(0, -200, 750),
			new(-100, -200, 750),

			new(-200, -200, 750),
			new(-200, -100, 750),
			new(-200, 0, 750),
			new(-200, 100, 750)
		];

		public static Point[] Plus =
		[
			new(-100, 100, 750),
			new(-100, 200, 750),
			new(0, 200, 750),
			new(100, 200, 750),

			new(100, 100, 750),
			new(200, 100, 750),
			new(200, 0, 750),
			new(200, -100, 750),

			new(100, -100, 750),
			new(100, -200, 750),
			new(0, -200, 750),
			new(-100, -200, 750),

			new(-100, -100, 750),
			new(-200, -100, 750),
			new(-200, 0, 750),
			new(-200, 100, 750)
		];

		public static Point[] BowTie =
		[
			new(-200, 0, 750),
			new(-150, 200, 750),
			new(-100, 200, 750),
			new(-50, 150, 750),
			new(50, 150, 750),
			new(100, 200, 750),
			new(150, 200, 750),
			new(200, 0, 750),

			new(200, -100, 750),
			new(150, -200, 750),
			new(100, -200, 750),
			new(50, -150, 750),
			new(-50, -150, 750),
			new(-100, -200, 750),
			new(-150, -200, 750),
			new (-200, -100, 750)
		];

		public static Point[] StylizedCross =
		[
			new(-200, 50, 750),
			new(-120, 70, 750),
			new(-70, 120, 750),
			new(-50, 200, 750),
			
			new(50, 200, 750),
			new(70, 120, 750),
			new(120, 70, 750),
			new(200, 50, 750),

			new(200, -50, 750),
			new(120, -70, 750),
			new(70, -120, 750),
			new(50, -200, 750),
			
			new(-50, -200, 750),
			new(-70, -120, 750),
			new(-120, -70, 750),
			new (-200, -50, 750)
		];

		public static Point[] Triangle =
		[
			new(-210, 210, 750),
			new(-140, 210, 750),
			new(-70, 210, 750),
			new(0, 210, 750),
			new(70, 210, 750),
			new(140, 210, 750),
			new(210, 210, 750),

			new(168, 126, 750),
			new(126, 42, 750),
			new(84, -42, 750),
			new(42, -126, 750),
			new(0, -210, 750),
			new(-42, -126, 750),	
			new(-84, -42, 750),
			new(-126, 42, 750),			
			new(-168, 126, 750),
		];
	}
}
