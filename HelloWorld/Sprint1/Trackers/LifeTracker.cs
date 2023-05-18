using Sprint1.States.ActionStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.Transformations;
using Microsoft.Xna.Framework;
using Sprint1.States.PowerStates;
using Sprint1.Sprites;
using Sprint1.Factories.SpriteFactories;
using Sprint1.Collisions;
using Sprint1.Commands;

namespace Sprint1.Trackers
{
    public class LifeTracker
    {
        public int life { get; private set; }
        ICommand resetLevel;
        Game1 reciever;

        public LifeTracker(Game1 game)
        {
            life = 3;
            resetLevel = new ResetCommand(game);
            reciever = game;
        }

        public void DecreaseLife(object o, EventArgs a)
        {
            life--;

            if (life > 0)
            { 
                resetLevel.Execute();
            }
            else
            {
                reciever.isGameOverReload = true;
                reciever.isGameOver = true;
            }
            Console.WriteLine("life has been decreamented(from life tracker) " + "ramining life: " + life);
        }

        public void IncreaseLife(object o, EventArgs a)
        {
            life++;
            Console.WriteLine("life has been increamented");
        }

        //this is for changing the life variable without passing a EventArgs
        public void DecreaseLife()
        {
            life--;
            Console.WriteLine("life has been deced by: " + 1);
        }

        public void Reset()
        {
            life = 3;
        }
    }
}

