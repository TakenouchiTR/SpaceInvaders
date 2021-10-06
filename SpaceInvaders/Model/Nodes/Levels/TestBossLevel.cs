using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model.Nodes.Entities;
using SpaceInvaders.Model.Nodes.Entities.Enemies;
using SpaceInvaders.View;

namespace SpaceInvaders.Model.Nodes.Levels
{
    public class TestBossLevel : LevelBase
    {
        public TestBossLevel()
        {
            AttachChild(new TestBoss());
            this.addPlayer();
        }


        private void addPlayer()
        {
            PlayerShip player = new PlayerShip();
            player.X = MainPage.ApplicationWidth / 2 - player.Collision.Width / 2;
            player.Y = MainPage.ApplicationHeight - 64;
            AttachChild(player);
            
        }

    }
}
