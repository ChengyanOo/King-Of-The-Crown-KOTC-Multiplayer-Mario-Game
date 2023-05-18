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

namespace Sprint1.Trackers
{
    public class WaypointTracker
    {
        private List<Vector2> waypointList;
        private IEntity entity;

        private Vector2 waypoint = Vector2.Zero;
        private int index = 0;

        public WaypointTracker(List<Vector2> waypointList, IEntity entity)
        {
            this.waypointList = waypointList;
            this.entity = entity;


            if (waypointList != null && waypointList.Count > 0)
            {
                this.waypoint = waypointList[0];
            }

        }

        public void Update()
        {
            if (index < waypointList.Count)
            {
                if (entity.Position.X > waypoint.X)
                {
                    waypoint = waypointList[++index];
                }
            }
        }

        public Vector2 getWaypoint()
        {
            return waypoint;
        }

        public void setEntity(IEntity entity)
        {
            this.entity = entity;
        }
    }
}

