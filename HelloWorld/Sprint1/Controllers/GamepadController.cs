using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
    internal class GamepadController : Controller
    {
        private GamePadState previousState;
        private GamePadState emptyInput;
        private PlayerIndex index;

        public GamepadController (PlayerIndex index) : base ()
        {
            this.index = index;
            previousState = GamePad.GetState(index);
            emptyInput = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, new Buttons());
        }

        public override void ProcessInputs()
        {
            GamePadState currentState = GamePad.GetState(index);

            if (currentState.IsConnected)
            {
                if (currentState != emptyInput) // Button Pressed
                {
                    var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));

                    foreach (var button in buttonList)
                    {
                        if (currentState.IsButtonDown(button) &&
                            !previousState.IsButtonDown(button))
                            if (pressCommands.ContainsKey((int)button))
                                pressCommands[(int)button].Execute();
                    
                        if (previousState.IsButtonDown(button) &&
                            !currentState.IsButtonDown(button))
                        {
                            if (releaseCommands.ContainsKey((int)button))
                            {
                                releaseCommands[(int)button].Execute();
                            }
                        }
                            
                    }
                }

                previousState = currentState;
            }
        }

        public override void AddControlsGeneral(Game1 game, CollisionDetector2 collisionDetector)
        {
            base.AddPressCommand((int)Buttons.Start, new QuitCommand(game));
        }

        public override void AddControlsMario(PlayerEntity player)
        {  
            base.AddPressCommand((int)Buttons.A, new ArrowUpCommand(player));
            base.AddPressCommand((int)Buttons.DPadDown, new ArrowDownCommand(player));
            base.AddPressCommand((int)Buttons.DPadLeft, new ArrowLeftCommand(player));
            base.AddPressCommand((int)Buttons.DPadRight, new ArrowRightCommand(player));
            base.AddPressCommand((int)Buttons.B, new FireballCommand(player));
            base.AddPressCommand((int)Buttons.Y, new StartCommand(player.game));

            base.AddReleaseCommand((int)Buttons.A, new ArrowUpCommand(player));
            base.AddReleaseCommand((int)Buttons.DPadDown, new ArrowDownCommand(player));
            base.AddReleaseCommand((int)Buttons.DPadLeft, new ArrowLeftCommand(player));
            base.AddReleaseCommand((int)Buttons.DPadRight, new ArrowRightCommand(player));
        }

        public override void AddControlsLuigi(PlayerEntity player)
        {
            base.AddPressCommand((int)Buttons.A, new ArrowUpCommand(player));
            base.AddPressCommand((int)Buttons.DPadDown, new ArrowDownCommand(player));
            base.AddPressCommand((int)Buttons.DPadLeft, new ArrowLeftCommand(player));
            base.AddPressCommand((int)Buttons.DPadRight, new ArrowRightCommand(player));
            base.AddPressCommand((int)Buttons.B, new InteractCrownCommand(player));

            base.AddReleaseCommand((int)Buttons.A, new ReleaseUpCommand(player));
            base.AddReleaseCommand((int)Buttons.DPadDown, new ReleaseDownCommand(player));
            base.AddReleaseCommand((int)Buttons.DPadLeft, new ReleaseLeftCommand(player));
            base.AddReleaseCommand((int)Buttons.DPadRight, new ReleaseRightCommand(player));
        }

        public override void WinAndLoseAddControls(Game1 game)
        {
            base.AddPressCommand((int)Buttons.Start, new QuitCommand(game));
        }
    }
}

 