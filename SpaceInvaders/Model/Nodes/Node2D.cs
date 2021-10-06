using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes
{
    public class Node2D : Node
    {
        private Vector2 position;

        public virtual double X
        {
            get => this.position.X;
            set
            {
                var moveDistance = value - this.X;
                foreach (var child in this.children)
                {
                    if (child is Node2D node)
                    {
                        node.X += moveDistance;
                    }
                }
                this.Moved?.Invoke(this, new Vector2(moveDistance, 0));
                this.position.X = value;
            }
        }
        
        public virtual double Y
        {
            get => this.position.Y;
            set
            {
                var moveDistance = value - this.Y;
                foreach (var child in this.children)
                {
                    if (child is Node2D node)
                    {
                        node.Y += moveDistance;
                    }
                }

                this.Moved?.Invoke(this, new Vector2(0, moveDistance));
                this.position.Y = value;
            }
        }

        public virtual Vector2 Position
        {
            get => new Vector2(this.X, this.Y);
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public EventHandler<Vector2> Moved;

        public void Move(Vector2 distance)
        {
            this.X += distance.X;
            this.Y += distance.Y;
        }

        public override void CompleteRemoval()
        {
            base.CompleteRemoval();

            if (this.Moved != null)
            {
                foreach (var subscriber in this.Moved?.GetInvocationList())
                {
                    this.Removed -= subscriber as EventHandler;
                }
            }
        }

        public virtual bool IsOffScreen()
        {
            return this.X > MainPage.ApplicationWidth ||
                   this.X < 0 ||
                   this.Y > MainPage.ApplicationHeight ||
                   this.Y < 0;
        }
    }
}
