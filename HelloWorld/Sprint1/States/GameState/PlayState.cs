using Microsoft.Xna.Framework;
using Sprint1.Collisions;
using Sprint1.Entities;
using Sprint1.Scrolling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.GameState
{
    public class PlayState 
    {
        protected Game1 Game1;
        public PlayState(Game1 game1) 
        {
            this.Game1 = game1;
        }
        public void PlayUpdate(GameTime gameTime)
        {
            this.Game1.timeTracker.Update(gameTime);
            this.Game1.coinTracker.Update();
            int controllerCount = this.Game1.controllers.Count;
            for (int i = 0; i < controllerCount; i++)
            {
                this.Game1.controllers[i].ProcessInputs();
            }

            int count = 0;
            while (count < this.Game1.spriteList.Count)
            {
                this.Game1.spriteList[count].Update(gameTime);
                count++;
            }
            /*
            foreach (var layer in this.Game1.layerList)
            {
                layer.Update(gameTime, this.Game1.GetScrollingCameraPosition(this.Game1.playerEntity));
            }

            //TODO - SHOULD this be before or after scrolling??
            this.Game1.collisionDetector.Update();

            if (this.Game1.screenCenter.X > this.Game1.camera.Origin.X)
            {
               // this.Game1.ScrollRight(this.Game1.GetScrollingCameraPosition(this.Game1.playerEntity));
            }
            */
            
        }
    }
}
