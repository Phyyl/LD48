using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Graphics.GLObjects;
using Vildmark.Graphics.Rendering;
using Vildmark.Resources;

namespace LD48
{
    public static class Textures
    {
        public static Texture2D TilesTexture;

        public static Texture2D CharEyesClosed;
        public static Texture2D CharEyesOpened;
        public static Texture2D CharEyesUp;
        public static Texture2D CharEyesDown;
        public static Texture2D PlatformLeft;
        public static Texture2D PlatformCenter;
        public static Texture2D PlatformRight;
        public static Texture2D Coin;
        public static Texture2D Wall;

        public static void Load()
        {
            TilesTexture = LoadTexture("tiles.png");

            Wall = TilesTexture.CreateSubTexture(new Rectangle(51, 16, 10, 16));
            CharEyesClosed = TilesTexture.CreateSubTexture(new Rectangle(1, 1, 14, 14));
            CharEyesOpened = TilesTexture.CreateSubTexture(new Rectangle(17, 1, 14, 14));
            CharEyesUp = TilesTexture.CreateSubTexture(new Rectangle(33, 1, 14, 14));
            CharEyesDown = TilesTexture.CreateSubTexture(new Rectangle(49, 1, 14, 14));
            PlatformLeft = TilesTexture.CreateSubTexture(new Rectangle(1, 19, 15, 10));
            PlatformCenter = TilesTexture.CreateSubTexture(new Rectangle(16, 19, 16, 10));
            PlatformRight = TilesTexture.CreateSubTexture(new Rectangle(32, 19, 15, 10));
            Coin = TilesTexture.CreateSubTexture(new Rectangle(4, 36, 8, 8));
        }

        private static GLTexture2D LoadTexture(string name)
        {
            return ResourceLoader.LoadEmbedded<GLTexture2D, TextureOptions>(name, TextureOptions.Nearest);
        }
    }
}
