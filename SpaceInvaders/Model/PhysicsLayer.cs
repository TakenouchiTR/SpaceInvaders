using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public enum PhysicsLayer
    {
        World        = 0b00000001,
        Player       = 0b00000010,
        Enemy        = 0b00000100,
        PlayerHitbox = 0b00001000,
        EnemyHitbox  = 0b00010000
    }
}
