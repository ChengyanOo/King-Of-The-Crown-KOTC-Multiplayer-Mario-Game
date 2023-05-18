using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1.Collisions
{
    internal class Quadtree
    {
        private int maxColliders = 10;
        private int maxLevels = 5;

        private int level;
        private List<ICollidable> colliders;
        private Rectangle bounds;
        private Quadtree[] nodes;

        //Constructor
        public Quadtree(int level, Rectangle bounds)
        {
            this.level = level;
            this.bounds = bounds;
            colliders = new List<ICollidable>();
            nodes = new Quadtree[4];
        }

        //Clear the quadtree
        public void Clear()
        {
            colliders.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        //Split the node into 4 subnodes
        private void Split()
        {
            int subWidth = (int)(bounds.Width / 2);
            int subHeight = (int)(bounds.Height / 2);
            int x = (int)bounds.X;
            int y = (int)bounds.Y;

            nodes[0] = new Quadtree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new Quadtree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new Quadtree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new Quadtree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        //Determine which node the collider is a child of
        //Return -1 if collider cannot completely fit within a child node and is part of the parent node
        private int GetIndex(ICollidable collider)
        {
            int index = -1;
            float verticalMidpoint = bounds.X + (bounds.Width / 2);
            float horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            //check if collider fits in top 2 quadrants
            bool topQuadrant = (collider.Collider.Y < horizontalMidpoint && collider.Collider.Y + collider.Collider.Height < horizontalMidpoint);
            //check if collider fits in bottom 2 quadrants
            bool bottomQuadrant = (collider.Collider.Y > horizontalMidpoint);

            //check if collider fits in left 2 quadrants
            if (collider.Collider.X < verticalMidpoint && collider.Collider.X + collider.Collider.Width < verticalMidpoint)
            {
                if (topQuadrant)
                    index = 1;

                else if (bottomQuadrant)
                    index = 2;
            }
            
            //check if collider fits in right 2 quadrants
            else if (collider.Collider.X > verticalMidpoint)
            {
                if (topQuadrant)
                    index = 0;
                
                else if (bottomQuadrant)
                    index = 3;
            }

            return index;
        }

        //Insert the collider into the quadtree
        public void Insert(ICollidable collider)
        {
            if (nodes[0] != null)
            {
                int index = GetIndex(collider);

                if (index != -1)
                {
                    nodes[index].Insert(collider);
                    return;
                }
            }

            colliders.Add(collider);

            if (colliders.Count > maxColliders && level < maxLevels)
            {
                if (nodes[0] == null)
                    Split();

                int i = 0;
                while (i < colliders.Count)
                {
                    int index = GetIndex(colliders[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(colliders[i]);
                        colliders.RemoveAt(i);
                    }
                    else
                        i++;
                }
            }
        }

        //Return all colliders that could collide with the given collider
        public List<ICollidable> Retrieve(List<ICollidable> returnColliders, ICollidable collider)
        {
            int index = GetIndex(collider);
            if (index != -1 && nodes[0] != null)
                nodes[index].Retrieve(returnColliders, collider);

            returnColliders.AddRange(colliders);

            return returnColliders;
        }

        public void Remove(ICollidable collidable) 
        {
            int index = GetIndex(collidable);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].Remove(collidable);    
            }
            else
            {
                colliders.Remove(collidable);
            }
        }

    }
}
