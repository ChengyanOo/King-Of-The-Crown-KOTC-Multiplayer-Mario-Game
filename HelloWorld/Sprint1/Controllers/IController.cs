using Sprint1.Collisions;
using Sprint1.Commands;
using Sprint1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Controllers
{
    public interface IController
    {
        void AddPressCommand(int key, ICommand command);
        void AddReleaseCommand(int key, ICommand command);
        void RemovePressCommand(int key);
        void RemoveReleaseCommand(int key);
        void ClearPressCommands();
        void ClearReleaseCommands();
        void ProcessInputs();
        void AddControlsGeneral(Game1 game, CollisionDetector2 collisionDetector);
        void AddControlsMario(PlayerEntity player);
        void AddControlsLuigi(PlayerEntity player);
        void WinAndLoseAddControls(Game1 game);
    }
}
