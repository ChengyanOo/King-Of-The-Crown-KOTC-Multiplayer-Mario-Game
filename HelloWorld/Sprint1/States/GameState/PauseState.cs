using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.States.GameState
{
    public class PauseState 
    {
        protected Game1 game1;
        public PauseState(Game1 game)
        {
            this.game1 = game;
        }
        public void PauseUpdate(GameTime gameTime)
        {
            int controllerCount = game1.controllers.Count;
            for (int i = 0; i < controllerCount; i++)
            {
                game1.controllers[i].ProcessInputs();
            }
        }
    }
}
