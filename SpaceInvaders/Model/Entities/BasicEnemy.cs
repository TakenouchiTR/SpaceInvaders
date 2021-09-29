using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model.Entities
{
    public class BasicEnemy : Enemy
    {
        public BasicEnemy(GameManager parent) : base(parent, new BasicEnemySprite())
        {
            Score = 10;
        }

        public override void Update(double delta)
        {

        }
    }
}
