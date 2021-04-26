using LD48;
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
        private const float platformFriction = 0.1f;

        private readonly List<Entity> entities = new();

        private readonly PhysicsSimulation simulation = new();

        public IEnumerable<Entity> Entities => entities.ToArray();

        public float CameraSpeed { get; private set; } = 50;

        public float CameraOffset { get; private set; }

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
    }
}
