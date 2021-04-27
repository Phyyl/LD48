using LD48;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Vildmark.Graphics.Rendering;
using Vildmark.Maths;
using Vildmark.Maths.Physics;

namespace LD48
{
    public class Level
    {
        private const float platformDistance = 150;
        private const float maxPlatformWidth = 400;
        private const float minPlatformWidth = 100;
        private const float powerupSize = 50;

        private readonly Random random = new();
        private readonly List<Entity> entities = new();
        private readonly PhysicsSimulation simulation = new();
        private WallEntity leftWall;
        private WallEntity rightWall;

        public IEnumerable<Entity> Entities => entities.ToArray();
        public float CameraSpeed = 100;
        public float CameraOffset;

        public Level()
        {
            ResetCameraOffset();

            GenerateEntities();

            leftWall.Bounce = 0.5f;
            leftWall.ZIndex = 1;
            rightWall.Bounce = 0.5f;
            rightWall.ZIndex = 1;
        }

        public void ResetCameraOffset()
        {
            CameraOffset = -Game.Instance.Window.Height;
        }

        public void AddEntity(Entity entity)
        {
            entity.Level = this;

            entities.Add(entity);
            simulation.AddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
            simulation.RemoveEntity(entity);
        }

        public void Update(float delta)
        {
            foreach (var entity in entities)
            {
                entity.Update(delta);
            }

            simulation.Update(delta);

            CameraOffset += CameraSpeed * delta;
        }

        public void Render(RenderContext2D renderContext)
        {
            foreach (var entity in entities.OrderBy(e => e.ZIndex))
            {
                entity.Render(renderContext);
            }
        }

        private void GenerateEntities()
        {
            AddEntity(leftWall = new WallEntity(0));
            AddEntity(rightWall = new WallEntity(Game.Instance.Window.Width - Textures.Wall.Width * Game.PixelScale));
            AddEntity(new PlatformEntity(new(0, 0), 400));

            for (int i = 1; i < 1000; i++)
            {
                float width = (float)random.NextDouble() * (maxPlatformWidth - minPlatformWidth) + minPlatformWidth;
                float x = (float)random.NextDouble() * (Game.Instance.Window.Width - width - Textures.Wall.Width * 2 * Game.PixelScale) + Textures.Wall.Width * Game.PixelScale;
                float y = i * platformDistance;

                AddEntity(new PlatformEntity(new(x, y), width)
                {
                    Bounce = (float)random.NextDouble() / 2
                });

                AddEntity(new PowerupEntity(new(x + width / 2 - powerupSize / 2, y - powerupSize * 2), new(powerupSize)));
            }
        }
    }
}
