using VGE;
using VGE.Graphics.Shapes;
using VGE.Resources;

namespace Battlezone
{
    public static class ObstacleShapeDefinitions
    {
        static List<ShapeDefinition> obstacles;
        static List<IShape> shapes;

        static ObstacleShapeDefinitions()
        {
            obstacles = new List<ShapeDefinition>();
            shapes = new List<IShape>();

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

            // --- FROM FILES ---
            //barrier
            shapes.Add(ResourcesHandler.Get3DShape("Obstacles/barrier"));
            shapes.Add(ResourcesHandler.Get3DShape("Obstacles/dragonsTeeth"));
        }

        public static IShape GetRandom()
        {
            var obstacleId = new Random().Next(0, obstacles.Count + shapes.Count);

            if (obstacleId < obstacles.Count)
            {
                var definition = obstacles[obstacleId];
                return new PredefinedShape(definition.PointsDefinition, definition.LinesDefinition);
            }
            else
                return shapes[obstacleId - obstacles.Count];
        }

        public static ShapeDefinition GetByIndex(int index)
        {
            return obstacles[index];
        }
    }
}
