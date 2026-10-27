
using Microsoft.Maui.Graphics;

namespace BlockBlast.Models
{
    public class BlockFactory
    {
        private readonly Random _random = new Random();

        private readonly List<int[,]> _shapes = new()
        {
            // Üks ruut
            new int[,]
            {
                { 1 }
            },

            // Kaks ruutu
            new int[,]
            {
                { 1, 1 }
            },

            // Kolm ruutu horisontaalselt
            new int[,]
            {
                { 1, 1, 1 }
            },

            // Kolm ruutu vertikaalselt
            new int[,]
            {
                { 1 },
                { 1 },
                { 1 }
            },

            // Neli ruutu horisontaalselt
            new int[,]
            {
                { 1, 1, 1, 1 }
            },

            // Neli ruutu vertikaalselt
            new int[,]
            {
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            },

            // 2x2 ruut
            new int[,]
            {
                { 1, 1 },
                { 1, 1 }
            },

            // 3x3 ruut
            new int[,]
            {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            },

            // L-kujuline klots
            new int[,]
            {
                { 1, 0 },
                { 1, 0 },
                { 1, 1 }
            },

            // Tagurpidi L
            new int[,]
            {
                { 0, 1 },
                { 0, 1 },
                { 1, 1 }
            },

            // T-kujuline klots
            new int[,]
            {
                { 1, 1, 1 },
                { 0, 1, 0 }
            },

            // Z-kujuline klots
            new int[,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 }
            },

            // Pluss-kujuline klots
            new int[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            }
        };

        private readonly List<Color> _colors = new()
        {
            Colors.DodgerBlue,
            Colors.Orange,
            Colors.MediumPurple,
            Colors.LimeGreen,
            Colors.DeepPink,
            Colors.Gold,
            Colors.Cyan
        };

        // Genereerib ühe juhusliku klotsi
        public Block GenerateBlock()
        {
            int shapeIndex = _random.Next(_shapes.Count);
            int colorIndex = _random.Next(_colors.Count);

            return new Block(
                (int[,])_shapes[shapeIndex].Clone(),
                _colors[colorIndex]
            );
        }

        // Genereerib kolm klotsi
        public List<Block> GenerateThreeBlocks()
        {
            List<Block> blocks = new List<Block>();

            for (int i = 0; i < 3; i++)
            {
                blocks.Add(GenerateBlock());
            }

            return blocks;
        }
    }
}
