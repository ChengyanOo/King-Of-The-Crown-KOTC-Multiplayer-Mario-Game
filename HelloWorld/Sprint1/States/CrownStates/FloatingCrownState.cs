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

    public class FloatingCrownState : CrownState
    {
        public FloatingCrownState(CrownEntity entity, ICrownState previousState) : base(entity, previousState)
        {}

        public override void Enter(ICrownState previousState)
        {
            base.Enter(previousState);
            entity.rigidbody = new Rigidbody(entity.game, entity.Position, new Vector2(0,0), 1, 0);
        }

        public override void collision (ICollidable collidee, int direction) 
        { 
            if (collidee is PlayerEntity)
            {
                toAttached((PlayerEntity)collidee);
            }    
        }
    }
}