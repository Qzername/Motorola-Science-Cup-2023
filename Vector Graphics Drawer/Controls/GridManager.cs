using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VectorGraphicsDrawer.Controls
{
    internal class GridManager : Control
    {
        Point GridCellSize;
        Point GridOffest;

        Pen linePen;

        List<List<Point>> shapes;

        public GridManager()
        {
            GridCellSize = new Point(25, 25);
            GridOffest = new Point(0, 0); //debug

            linePen = new Pen(Brushes.White);

            Clear();

            PointerPressed += GridManager_PointerPressed;
        }

        public override void Render(DrawingContext context)
        {  
            //if that line wouldnt exist, avalonia would not detect pointer click
            context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Bounds.Width, Bounds.Height));

            for (int x = 1; x < Bounds.Width / GridCellSize.X - 1; x++)
                for (int y = 1; y < Bounds.Height / GridCellSize.Y; y++)
                    context.DrawEllipse(Brushes.Orange, null, new Point(x * GridCellSize.X, y * GridCellSize.Y), 1f, 1f);

            foreach(var shape in shapes)
            {
                for (int i = 1; i < shapes.Count; i++)
                    context.DrawLine(linePen, GetRealPosition(shape[i - 1]), GetRealPosition(shape[i]));

                if (shapes.Count > 2)
                    context.DrawLine(linePen, GetRealPosition(shape[0]), GetRealPosition(shape[^1]));
            }
        }

        public void Clear()
        {
            shapes = new();
            AddShape();
            InvalidateVisual();
        }

        public void AddShape()
        {
            shapes.Add(new List<Point>());
        }

        void GridManager_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            Debug.WriteLine(mousePosition);

            shapes[^1].Add(GetGridPosition(mousePosition));

            InvalidateVisual();
        }

        Point GetRealPosition(Point gridPosition) => new Point((gridPosition.X + GridOffest.X) * GridCellSize.X, (gridPosition.Y + GridOffest.Y) * GridCellSize.Y);
        Point GetGridPosition(Point realPosition) => new Point(Math.Round(realPosition.X / GridCellSize.X), Math.Round(realPosition.Y / GridCellSize.Y));
    }
}
