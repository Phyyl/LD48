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
        private const float platformHeight = 5;

        private readonly Random random = new();
        private readonly List<Entity> entities = new();
        private readonly PhysicsSimulation simulation = new();
        private PlatformEntity leftWall;
        private PlatformEntity rightWall;

        public IEnumerable<Entity> Entities => entities.ToArray();
        public float CameraSpeed = 100;
        public float CameraOffset { get; private set; }

        public Level()
        {
            CameraOffset = -Game.Instance.Window.Height;

            GeneratePlatforms();

            leftWall.Bounce = 0.5f;
            rightWall.Bounce = 0.5f;
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
            foreach (var entity in entities)
            {
                entity.Render(renderContext);
            }
        }

        private void GeneratePlatforms()
        {
            AddEntity(leftWall = new PlatformEntity(new(0, -1000), new(25, 100000)));
            AddEntity(rightWall = new PlatformEntity(new(Game.Instance.Window.Width - 25, -1000), new(25, 100000)));
            AddEntity(new PlatformEntity(new(0, 0), new(400, platformHeight)));

            for (int i = 1; i < 1000; i++)
            {
                float width = (float)random.NextDouble() * (maxPlatformWidth - minPlatformWidth) + minPlatformWidth;

                float x = (float)random.NextDouble() * (Game.Instance.Window.Width - width);
                float y = i * platformDistance;

                AddEntity(new PlatformEntity(new(x, y), new(width, platformHeight))
                {
                    Bounce = (float)random.NextDouble() / 2
                });
            }
        }
    }
}
