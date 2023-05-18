using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Entities;
using System.Diagnostics;
using Sprint1.Sprites;
using System.Runtime.ExceptionServices;
using Sprint1.Transformations;
using Sprint1.Entities.ItemEntities.Fireball;

namespace Sprint1.Collisions
{
    public class CollisionDetector
    {
        private Quadtree quadtree;
        private List<ICollidable> collidableList;
        private Dictionary<IEntity, List<ICollidable>> movingDict;
        public bool isColliderVisible;

        private int count =0;

        public CollisionDetector(int level, Rectangle bounds)
        {
            quadtree = new Quadtree(level, bounds);
            collidableList = new List<ICollidable>();
            movingDict = new Dictionary<IEntity, List<ICollidable>>();
            isColliderVisible = false;
        }

        public void insertCollidable(ICollidable collidableObject)
        {
            if (!collidableList.Contains(collidableObject))
            {
                collidableList.Add(collidableObject);
            }
        }

        public void removeCollidable(ICollidable collidableObject)
        {
            if (collidableList.Contains(collidableObject))
            {
                collidableList.Remove(collidableObject);
            }
        }

        public void clearCollidable()
        {
            collidableList.Clear();
        }

        public void insertMoving(IEntity movingObject)
        {
            if (!movingDict.ContainsKey(movingObject))
            {
                movingDict.Add(movingObject, new List<ICollidable>());
            }
        }

        public void removeMoving(IEntity movingObject)
        {
            if (movingDict.ContainsKey(movingObject))
            {
                movingDict.Remove(movingObject);
            }
        }

        public void clearMoving()
        {
            movingDict.Clear();
        }

        public void toggleVisibility()
        {
            isColliderVisible = !isColliderVisible;
        }

        public void Update()
        {
            updateQuadtree();
            checkCollisions();
        }

        public void updateQuadtree()
        {
            quadtree.Clear();

            int collidableCount = collidableList.Count;
            for (int i = 0; i < collidableCount; i++)
            {
                quadtree.Insert(collidableList[i]);
            }
        }

        public void checkCollisions()
        {   
            Dictionary<IEntity, Vector2> movementDict = getMovement(movingDict.Keys.ToList());
            while (movementDict.Count > 0)
            {
                Dictionary<IEntity, Vector2> repeatDict = new Dictionary<IEntity, Vector2>();

                List<IEntity> movementList = movementDict.Keys.ToList();
                int movementCount = movementList.Count;
                for (int i = 0; i < movementCount; i++)
                {
                    if (movementList[i] is Entity)
                    {
                        //Debug.WriteLine(((Entity)movementList[i]).rigidbody);
                        if (((Entity)movementList[i]).rigidbody != null)
                        {
                            movementList[i].Position = ((Entity)movementList[i]).rigidbody.position;
                        }
                        else
                        {
                            movementList[i].Position = movementList[i].transformation(movementList[i].Position);
                        }
                    }
                    else
                    {
                        movementList[i].Position = movementList[i].transformation(movementList[i].Position);
                    }
                    
                    Dictionary<IEntity, Vector2> tempDict = callCollision(movementList[i], movementDict);

                    foreach(IEntity entity in tempDict.Keys)
                    {
                        if (!repeatDict.ContainsKey(entity))
                        {
                            repeatDict.Add(entity, tempDict[entity]);
                        }
                    }
                }
                movementDict = repeatDict;
            }
        }

        private static Dictionary<IEntity, Vector2> getMovement(List<IEntity> movingList)
        {
            Dictionary<IEntity, Vector2> movementDict = new Dictionary<IEntity, Vector2>();
            foreach (IEntity entity in movingList)
            {
                Vector2 movement;
                if (entity is Entity)
                {
                    if (((Entity)entity).rigidbody != null)
                    {
                        movement = ((Entity)entity).rigidbody.position - entity.Position;    
                    }
                    else
                    {
                        movement = entity.transformation(entity.Position) - entity.Position;
                    }
                }
                else
                {
                    movement = entity.transformation(entity.Position) - entity.Position;
                }

                movementDict.Add(entity, movement);
            }
            return movementDict;
        }

        private Dictionary<IEntity, Vector2> callCollision(IEntity entity, Dictionary<IEntity, Vector2> movementDict)
        {
            List<ICollidable> possibleCollisions = quadtree.Retrieve(new List<ICollidable>(), (ICollidable)entity);
            List<ICollidable> currentCollisions = new List<ICollidable>();
            List<ICollidable> newCollisions = new List<ICollidable>();
            foreach(ICollidable collidee in possibleCollisions)
            {    
                if (entity != collidee && entity.Collider.Intersects(collidee.Collider))
                {
                    if (movingDict.ContainsKey(entity))
                    {
                        if ((movingDict[entity]).Contains(collidee))
                        {
                            currentCollisions.Add(collidee);
                        }
                        else
                        {
                            newCollisions.Add(collidee);
                        }
                    }
                    turnRedCollisions(entity, collidee);
                }
                
            }
            
            if (entity is Entity && ((Entity)entity).rigidbody != null && entity is not FireballEntity)
            {
                ((Entity)entity).rigidbody.CheckGround((Entity)entity, possibleCollisions);
            }
            
            movingDict[entity] = currentCollisions;
            

            Dictionary<IEntity, Vector2> repeatDict = new Dictionary<IEntity, Vector2>();
            if (newCollisions.Count > 0)
            {
                Dictionary<ICollidable, int> firstCollisions = getFirstCollisions(entity, newCollisions);
                repeatDict = handleCollision(entity, movementDict, firstCollisions);
            }
            return repeatDict;
        }

        private static Dictionary<ICollidable, int> getFirstCollisions(IEntity entity, List<ICollidable> collidableList)
        {
            Dictionary<ICollidable, int> firstCollisions = new Dictionary<ICollidable, int>();
            firstCollisions.Add(collidableList[0], getDirection(entity, collidableList[0]));
            Vector2 distance = getDistance(entity, collidableList[0]);
            float minDistance = Math.Abs(distance.X) + Math.Abs(distance.Y);
            if (collidableList.Count > 1)
            {
                for (int i = 1; i < collidableList.Count; i++)
                {
                    distance = getDistance(entity, collidableList[i]);
                    if ((Math.Abs(distance.X) + Math.Abs(distance.Y)) == minDistance)
                    {
                        firstCollisions.Add(collidableList[i], getDirection(entity, collidableList[i]));
                    }
                    else if ((Math.Abs(distance.X) + Math.Abs(distance.Y)) < minDistance)
                    {
                        firstCollisions = new Dictionary<ICollidable, int>();
                        firstCollisions.Add(collidableList[i], getDirection(entity, collidableList[i]));
                        minDistance = Math.Abs(distance.X) + Math.Abs(distance.Y);
                    }
                }
            }
            return firstCollisions;
        }

        private Dictionary<IEntity, Vector2> handleCollision(IEntity entity, Dictionary<IEntity, Vector2> movementDict, Dictionary<ICollidable, int> collisionDict)
        {
            Dictionary<IEntity, Vector2> repeatDict = new Dictionary<IEntity, Vector2>();
   
            List<ICollidable> collisionList = collisionDict.Keys.ToList();
            int collisionCount = collisionList.Count;
            for (int i = 0; i < collisionCount; i++)
            {
                if (!(collisionList[i] is IEntity && movementDict.ContainsKey((IEntity)collisionList[i]) && !Vector2.Equals(movementDict[(IEntity)collisionList[i]], Vector2.Zero)))
                {
                    entity.OnCollisionEnter(collisionList[i], collisionDict[collisionList[i]]);
                    //Debug.WriteLine("Collision between: " + entity + " and " + collisionList[i] + " - With direction: " + collisionDict[collisionList[i]] + " test " + count++);

                    collisionList[i].OnCollisionEnter(entity, oppositeDirection(collisionDict[collisionList[i]]));
                    //Debug.WriteLine("Collision between: " + collisionList[i] + " and " + entity + " - With direction: " + oppositeDirection(collisionDict[collisionList[i]]));
                    
                    movementDict[entity] = Vector2.Zero;
                    if (movingDict.ContainsKey(entity))
                    {
                        movingDict[entity].Add(collisionList[i]);
                    }
                }
                else
                {
                    if (
                         
                        (Math.Sign(movementDict[entity].Y) == -1 && (entity.Collider.Center.Y > ((IEntity)collisionList[i]).Collider.Center.Y)) |
                        (Math.Sign(movementDict[entity].Y) == 1 && (entity.Collider.Center.Y <= ((IEntity)collisionList[i]).Collider.Center.Y)) |
                        (Math.Sign(movementDict[entity].X) == -1 && (entity.Collider.Center.X > ((IEntity)collisionList[i]).Collider.Center.X)) |
                        (Math.Sign(movementDict[entity].X) == 1 && (entity.Collider.Center.X <= ((IEntity)collisionList[i]).Collider.Center.X)) |

                        (Math.Sign(movementDict[((IEntity)collisionList[i])].Y) == -1 && (((IEntity)collisionList[i]).Collider.Center.Y > entity.Collider.Center.Y)) |
                        (Math.Sign(movementDict[((IEntity)collisionList[i])].Y) == 1 && (((IEntity)collisionList[i]).Collider.Center.Y <= entity.Collider.Center.Y)) |
                        (Math.Sign(movementDict[((IEntity)collisionList[i])].X) == -1 && (((IEntity)collisionList[i]).Collider.Center.X > entity.Collider.Center.X)) |
                        (Math.Sign(movementDict[((IEntity)collisionList[i])].X) == 1 && (((IEntity)collisionList[i]).Collider.Center.X <= entity.Collider.Center.X)) 
                    )
                    {
                        movementDict[entity] = Vector2.Zero;
                    } 
                    else
                    {
                        movementDict[entity] = correctPosition(entity, collisionList[i], movementDict[entity]);
                    }

                    if(!repeatDict.ContainsKey(entity))
                    {
                        repeatDict.Add(entity, movementDict[entity]);
                    }
                }
            }
            return repeatDict;
        }

        private static Vector2 correctPosition(IEntity entity, ICollidable collidee, Vector2 remainingMovement)
        {
            Vector2 distance = getDistance(entity, collidee);
            return Vector2.Add(remainingMovement, distance);
        }

        private static Vector2 getDistance(IEntity entity, ICollidable collidee)
        {
            int direction = getDirection(entity, collidee);
            Vector2 distance = new Vector2(0,0);
            Rectangle collisionRect = Rectangle.Intersect(entity.Collider, collidee.Collider);
            switch (direction)
            {
                case 0:
                    distance.Y = collisionRect.Height;
                    break;
                case 1:
                    distance.X = collisionRect.Width;
                    break;
                case 2:
                    distance.Y = -collisionRect.Height;
                    break;
                case 3:
                    distance.X = -collisionRect.Width;
                    break;
            }
            return distance;
        }

        public static int getDirection(ICollidable entity, ICollidable collider)
        {
            int direction = 0;
            Rectangle collisionRect = Rectangle.Intersect(entity.Collider, collider.Collider);

            if (collisionRect.Width >= collisionRect.Height)
            {
                if (collisionRect.Center.Y > entity.Collider.Center.Y)
                {
                    direction = 2;
                }
                else
                {
                    direction = 0;
                }
            }
            else
            {
                if (collisionRect.Center.X > entity.Collider.Center.X)
                {
                    direction = 1;
                }
                else
                {
                    direction = 3;
                }
            }

            return direction;
        }

        private static int oppositeDirection(int direction)
        {
            int oppositeDirection = direction;
            switch (direction)
            {
                case 0:
                    oppositeDirection = 2;
                    break;
                case 1:
                    oppositeDirection = 3;
                    break;
                case 2:
                    oppositeDirection = 0;
                    break;
                case 3:
                    oppositeDirection = 1;
                    break;
                default:
                    oppositeDirection = -1;
                    break;
            }
            return oppositeDirection;
        }

        private void turnRedCollisions(ICollidable collidee1, ICollidable collidee2)
        {
            if (isColliderVisible)
            {
                if (collidee1 is ISprite)
                {
                    ((ISprite)collidee1).color = Color.Red;
                }

                if (collidee2 is ISprite)
                {
                    ((ISprite)collidee2).color = Color.Red;
                }
            }
        }
    }
}