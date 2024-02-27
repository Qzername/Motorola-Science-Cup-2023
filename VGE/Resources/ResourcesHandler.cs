using Newtonsoft.Json;
using SkiaSharp;
using System.IO;
using VGE.Graphics.Shapes;

namespace VGE.Resources
{
	public static class ResourcesHandler
	{
		static Dictionary<string, ShapeSet> database;

		static ResourcesHandler()
		{
			database = new Dictionary<string, ShapeSet>();
		}

		public static ShapeSet GetShapeSet(string name)
		{
			if (!database.ContainsKey(name))
				LoadSet(name);

			return database[name];
		}

		public static IShape GetShape(string shapeSetName, string shapeName)
		{
			if (!database.ContainsKey(shapeSetName))
				LoadSet(shapeSetName);

			return database[shapeSetName].Set[shapeName][0];
		}

		public static IShape Get3DShape(string shapeName)
		{
            List<Point> pointDefinitions = [];
            List<Point> lineDefinitions = [];

            int mode = 0;

			SKColor color = SKColors.Green;

            foreach (var line in File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + $"Resources/{shapeName}.3Dshape"))
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
                    continue;

                if (line[0] == '[')
                {
                    mode++;
                    continue;
                }

                var coordinates = line.Split([",", ", "], StringSplitOptions.RemoveEmptyEntries);

                Point point;

                if (coordinates.Length == 2)
                    point = new Point(Convert.ToSingle(coordinates[0].Replace('.', ',')), Convert.ToSingle(coordinates[1].Replace('.', ',')), 0);
                else
                    point = new Point(Convert.ToSingle(coordinates[0].Replace('.', ',')), Convert.ToSingle(coordinates[1].Replace('.', ',')), Convert.ToSingle(coordinates[2].Replace('.', ',')));

                if (mode == 1)
                    pointDefinitions.Add(point);
                else if(mode == 2)
                    lineDefinitions.Add(point);
				else
					color = new SKColor(Convert.ToByte(coordinates[0].Replace('.', ',')), Convert.ToByte(coordinates[1].Replace('.', ',')), Convert.ToByte(coordinates[2].Replace('.', ',')));
			}

			return new PredefinedShape(pointDefinitions.ToArray(), lineDefinitions.ToArray(), color);
        }

		static void LoadSet(string setName)
		{
			string setJson = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"Resources/{setName}.json");
			var rawSet = JsonConvert.DeserializeObject<RawShapeSet>(setJson);

			Dictionary<string, IShape[]> set = new Dictionary<string, IShape[]>();

			foreach (var shapeArray in rawSet.Set)
			{
				set[shapeArray.Key] = new IShape[shapeArray.Value.Length];

				for (int i = 0; i < shapeArray.Value.Length; i++)
				{
					List<Point> points = new List<Point>();

					foreach (var point in shapeArray.Value[i].Points)
						points.Add(new Point(point.X, point.Y));

					if (points.Count == 0)
						continue;

					set[shapeArray.Key][i] = new PointShape(points.ToArray());
				}
			}

			ShapeSet shapeSet = new ShapeSet()
			{
				Name = rawSet.Name,
				Set = set,
			};

			database[setName] = shapeSet;
		}


	}
}
