using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.States.ActionStates; //Maybe use this to change the ActionStates 
using Sprint1.Entities; //attempts
using Sprint1.Commands;
using Sprint1.Collisions;

namespace Sprint1.Controllers
{
    internal class KeyboardController : Controller
    {
        private KeyboardState previousState;
        //protected PlayerEntity entity;

        public KeyboardController() : base()
        {
            previousState = Keyboard.GetState();
        }

        public override void ProcessInputs()
        {
            KeyboardState currentState = Keyboard.GetState();
            Keys[] pressed = currentState.GetPressedKeys();
            Keys[] previouslyPressed = previousState.GetPressedKeys();

            foreach (Keys key in pressed)
            {
                if (!previousState.IsKeyDown(key))
                {
                    if (pressCommands.ContainsKey((int)key))
                    {
                        pressCommands[(int)key].Execute();
                    }
                }
            }

            foreach (Keys key in previouslyPressed)
            {
                if (!currentState.IsKeyDown(key))
                {
                    if (releaseCommands.ContainsKey((int)key))
                    {
                        releaseCommands[(int)key].Execute();
                    }
                }
            }
         
            previousState = currentState;
        }

        public override void AddControlsGeneral(Game1 game, CollisionDetector2 collisionDetector)
        {
            base.AddPressCommand((int)Keys.Q, new QuitCommand(game));
            base.AddPressCommand((int)Keys.R, new ResetCommand(game));
            base.AddPressCommand((int)Keys.C, new ToggleVisibleCollidersCommand(collisionDetector));
            base.AddPressCommand((int)Keys.M, new MuteCommand(game.Sounds));
            base.AddPressCommand((int)Keys.P, new PauseCommand(game));
            base.AddPressCommand((int)Keys.Back, new StartCommand(game));
        }

        public override void AddControlsMario(PlayerEntity player)
        {
            base.AddPressCommand((int)Keys.Y, new TurnSmallCommand(player));
            base.AddPressCommand((int)Keys.U, new TurnSuperCommand(player));
            base.AddPressCommand((int)Keys.I, new TurnFireCommand(player));
            base.AddPressCommand((int)Keys.O, new TakeDamageCommand(player));

            base.AddPressCommand((int)Keys.W, new ArrowUpCommand(player));
            base.AddPressCommand((int)Keys.S, new ArrowDownCommand(player));
            base.AddPressCommand((int)Keys.A, new ArrowLeftCommand(player));
            base.AddPressCommand((int)Keys.D, new ArrowRightCommand(player)); 
            base.AddPressCommand((int)Keys.Space, new InteractCrownCommand(player));

            base.AddReleaseCommand((int)Keys.W, new ReleaseUpCommand(player));
            base.AddReleaseCommand((int)Keys.S, new ReleaseDownCommand(player));
            base.AddReleaseCommand((int)Keys.A, new ReleaseLeftCommand(player));
            base.AddReleaseCommand((int)Keys.D, new ReleaseRightCommand(player));
        }

        public override void AddControlsLuigi(PlayerEntity player)
        {
            base.AddPressCommand((int)Keys.NumPad7, new TurnSmallCommand(player));
            base.AddPressCommand((int)Keys.NumPad8, new TurnSuperCommand(player));
            base.AddPressCommand((int)Keys.NumPad9, new TurnFireCommand(player));
            base.AddPressCommand((int)Keys.NumPad4, new TakeDamageCommand(player));

            base.AddPressCommand((int)Keys.Up, new ArrowUpCommand(player));
            base.AddPressCommand((int)Keys.Down, new ArrowDownCommand(player));
            base.AddPressCommand((int)Keys.Left, new ArrowLeftCommand(player));
            base.AddPressCommand((int)Keys.Right, new ArrowRightCommand(player)); 
            base.AddPressCommand((int)Keys.NumPad0, new InteractCrownCommand(player));
            base.AddPressCommand((int)Keys.Enter, new InteractCrownCommand(player));
            
            base.AddReleaseCommand((int)Keys.Up, new ReleaseUpCommand(player));
            base.AddReleaseCommand((int)Keys.Down, new ReleaseDownCommand(player));
            base.AddReleaseCommand((int)Keys.Left, new ReleaseLeftCommand(player));
            base.AddReleaseCommand((int)Keys.Right, new ReleaseRightCommand(player));
        }

        public override void WinAndLoseAddControls(Game1 game)
        {
            base.AddPressCommand((int)Keys.Q, new QuitCommand(game));
            base.AddPressCommand((int)Keys.R, new ResetCommand(game));
        }
    }
}
