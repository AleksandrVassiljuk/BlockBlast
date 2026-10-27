
using Microsoft.Maui.Graphics;

namespace BlockBlast.Models
{
    public class Block
    {
        public int[,] Shape { get; set; }
        public Color Color { get; set; }

        public Block(int[,] shape, Color color)
        {
            Shape = shape;
            Color = color;
        }

        public int Width => Shape.GetLength(1);
        public int Height => Shape.GetLength(0);
    }
}
