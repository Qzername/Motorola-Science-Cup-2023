using VGE;

namespace Battlezone
{
	public static class ObstacleShapeDefinitions
	{
		static List<ShapeDefinition> obstacles;

		static ObstacleShapeDefinitions()
		{
			obstacles = new List<ShapeDefinition>();

			//Cube
			obstacles.Add(new ShapeDefinition()
			{
				PointsDefinition = [
				new Point(10, 10, 10),
					new Point(-10, 10, 10),
					new Point(-10, -10, 10),
					new Point(10, -10, 10),
					new Point(10, 10, -10),
					new Point(-10, 10, -10),
					new Point(-10, -10, -10),
					new Point(10, -10, -10),
				],
				LinesDefinition = [
                //first rectangle
                new Point(0, 1),
					new Point(1, 2),
					new Point(2, 3),
					new Point(3, 0),
                    //second rectangle
                    new Point(4, 5),
					new Point(5, 6),
					new Point(6, 7),
					new Point(7, 4),
                    //connection between rectangles
                    new Point(0, 4),
					new Point(1, 5),
					new Point(2, 6),
					new Point(3, 7),
				]
			});

			//Pyramid
			obstacles.Add(new ShapeDefinition()
			{
				PointsDefinition = [
					new Point(10, 10, 10),
					new Point(-10, 10, 10),
					new Point(-10, 10, -10),
					new Point(10, 10, -10),
					new Point(0, -10, 0),
				],
				LinesDefinition = [
                    //rectangle
                    new Point(0, 1),
					new Point(1, 2),
					new Point(2, 3),
					new Point(3, 0),
                    //connection with top point
                    new Point(0, 4),
					new Point(1, 4),
					new Point(2, 4),
					new Point(3, 4),
				]
			});
		}

		public static ShapeDefinition GetRandom()
		{
			return obstacles[new Random().Next(0, obstacles.Count)];
		}

		public static ShapeDefinition GetByIndex(int index)
		{
			return obstacles[index];
		}
	}
}
