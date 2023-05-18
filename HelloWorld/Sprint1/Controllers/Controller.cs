using Sprint1.Collisions;
using Sprint1.Commands;
using Sprint1.Entities;
using System.Collections.Generic;
using Sprint1.Audio;

namespace Sprint1.Controllers
{
    internal abstract class Controller : IController
    {
        protected Dictionary<int, ICommand> pressCommands;
        protected Dictionary<int, ICommand> releaseCommands;

        public Controller ()
        {
            pressCommands = new Dictionary<int, ICommand> ();
            releaseCommands = new Dictionary<int, ICommand> ();
        }

        public void AddPressCommand(int key, ICommand command)
        {
            pressCommands.Add(key, command);
        }
        public void AddReleaseCommand(int key, ICommand command)
        {
            releaseCommands.Add(key, command);
        }
        public void RemovePressCommand(int key)
        {
            pressCommands.Remove(key);
        }
        public void RemoveReleaseCommand(int key)
        {
            releaseCommands.Remove(key);
        }
        public void ClearPressCommands() 
        {
            pressCommands.Clear(); 
        }
        public void ClearReleaseCommands() 
        {
            releaseCommands.Clear();
        }

        public abstract void ProcessInputs();
        public abstract void AddControlsGeneral(Game1 game, CollisionDetector2 collisionDetector);
        public abstract void AddControlsMario(PlayerEntity player);
        public abstract void AddControlsLuigi(PlayerEntity player);
        public abstract void WinAndLoseAddControls(Game1 game);
    }
}
