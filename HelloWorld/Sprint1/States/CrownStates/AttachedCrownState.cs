using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using Sprint1.States.ActionStates;
using Sprint1.Collisions;
using Sprint1.Factories.SpriteFactories;
using Sprint1.States.EnemyStates;
using Sprint1.Trackers;
using Sprint1.Audio;
using System.Net.NetworkInformation;
using Sprint1.Physics;


namespace Sprint1.States.CrownStates
{
    public class AttachedCrownState : CrownState
    {
        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs SoundEffectArgs;

        public AttachedCrownState(CrownEntity entity, ICrownState previousState) : base(entity, previousState)
        { }

        public override void Enter(ICrownState previousState)
        {
            SetEffect += entity.game.audioManager.PlaySoundEffect;
            SoundEffectArgs = new SoundEffectEventArgs { effect = "steal" };
            onSetEffect(SoundEffectArgs);

            base.Enter(previousState);
            //Code to centralize crown on head

            Rectangle collider = entity.game.GetCollider(entity.playerEntity.spriteType, entity.playerEntity.Position);
            entity.rigidbody = new Rigidbody(entity.game, new Vector2(collider.Center.X - entity.Collider.Width / 2, collider.Top - entity.Collider.Height - 30), Vector2.Zero, 1, 0);
            //entity.Collider = Rectangle.Empty;

            entity.playerEntity.ChangingPosition += this.ChangingPosition;

            //if ((entity.playerEntity.spriteType & SpriteEnum.allPlayers & SpriteEnum.player) == (SpriteEnum.player | SpriteEnum.player2))
            if ((entity.playerEntity.spriteType & SpriteEnum.player1) == (SpriteEnum.player1))
            {
                entity.game.holdTimeTracker.MarioTimerStart();

                Console.WriteLine("(from AttachedCrownState)Mario timer start ");
                //holdTimeTracker.MarioHolding(,holdTimeTracker.MarioHoldTime);

            }
            else
            {

                entity.game.holdTimeTracker.LuigiTimerStart();

                Console.WriteLine("(from AttachedCrownState)Luigi timer start ");

            }
        }

        public override void Exit()
        {
            base.Exit();

            entity.playerEntity.ChangingPosition -= this.ChangingPosition;

            if ((entity.playerEntity.spriteType & SpriteEnum.player1) == (SpriteEnum.player1))
            {
                entity.game.holdTimeTracker.MarioTimerPause();
            }
            else
            {
                entity.game.holdTimeTracker.LuigiTimerPause();
            }
        }

        public void ChangingPosition(object o, PositionEventArgs a)
        {
            Rectangle collider = entity.game.GetCollider(entity.playerEntity.spriteType, a.PositionValue);
            entity.Position = new Vector2(collider.Center.X - entity.Collider.Width / 2, collider.Top - entity.Collider.Height - 30);
        }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }
    }
}