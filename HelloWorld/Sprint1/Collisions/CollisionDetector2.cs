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
using Sprint1.Factories.SpriteFactories;
using Sprint1.Entities.ItemEntities;

namespace Sprint1.Collisions
{
    public class CollisionDetector2
    {
        private Quadtree quadtree;
        private List<ICollidable> collidableList;
        private Dictionary<IEntity, List<ICollidable>> collidingDict;
        public bool isColliderVisible;

        private int count =0;

        public CollisionDetector2(int level, Rectangle bounds)
        {
            quadtree = new Quadtree(level, bounds);
            collidableList = new List<ICollidable>();
            collidingDict = new Dictionary<IEntity, List<ICollidable>>();
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

        public bool containsCollidable(ICollidable collidableObject)
        {
            return collidableList.Contains(collidableObject);
        }

        public void clearCollidable()
        {
            collidableList.Clear();
        }

        public void insertMoving(IEntity movingObject)
        {
            if (!collidingDict.ContainsKey(movingObject))
            {
                collidingDict.Add(movingObject, new List<ICollidable>());
            }
        }

        public void removeMoving(IEntity movingObject)
        {
            if (collidingDict.ContainsKey(movingObject))
            {
                collidingDict.Remove(movingObject);
            }
        }

        public bool containsMoving(IEntity movingObject)
        {
            return collidingDict.ContainsKey(movingObject);
        }

        public void clearMoving()
        {
            collidingDict.Clear();
        }

        public void toggleVisibility()
        {
            isColliderVisible = !isColliderVisible;
        }

        public void Update()
        {
            updateQuadtree();
            checkCollisions();
            //Debug.WriteLine(count++);
        }

        private void updateQuadtree()
        {
            quadtree.Clear();

            int collidableCount = collidableList.Count;
            for (int i = 0; i < collidableCount; i++)
            {
                quadtree.Insert(collidableList[i]);
            }
        }

        private void checkCollisions()
        {
            float totalRatio = 0;
            float elapsedRatio = 0;
            Dictionary<IEntity, Rectangle> sweptColliders = getSweptColliders(collidingDict.Keys.ToList(), totalRatio);

            List<KeyValuePair<IEntity, ICollidable>> collisions = getCollisions(collidingDict.Keys.ToList(), sweptColliders);
            List<KeyValuePair<IEntity, ICollidable>> sortedCollisions = sortCollisions(collisions, totalRatio, sweptColliders);
            if (sortedCollisions.Count > 0)
            {
                elapsedRatio = calculateElapsedRatio(sortedCollisions[0].Key, sortedCollisions[0].Value, totalRatio, sweptColliders);
                if (totalRatio + elapsedRatio > 1)
                {
                    elapsedRatio = 1-totalRatio;
                    totalRatio = 1;
                }
                else
                {
                    totalRatio += elapsedRatio;
                }             

                //Debug.WriteLine("ER: " + elapsedRatio);
                //Debug.WriteLine("TR: " + totalRatio);

                foreach(IEntity entity in collidingDict.Keys.ToList())
                {
                    moveEntity(entity, elapsedRatio);
                }
                sweptColliders = getSweptColliders(collidingDict.Keys.ToList(), totalRatio);

                List<IEntity> repeatCollisions = handleCollision(sortedCollisions[0].Key, sortedCollisions[0].Value, sweptColliders);
                sortedCollisions.RemoveAt(0);
                collisions = getCollisions(repeatCollisions, sweptColliders);
                foreach (KeyValuePair<IEntity, ICollidable> pair in collisions)
                {
                    if (!sortedCollisions.Contains(pair))
                    {
                        sortedCollisions.Add(pair);
                    }
                }
            }
            else
            {
                elapsedRatio = 1;
                totalRatio = 1;

                foreach(IEntity entity in collidingDict.Keys.ToList())
                {
                    moveEntity(entity, elapsedRatio);
                }
                sweptColliders = getSweptColliders(collidingDict.Keys.ToList(), totalRatio);
            }
            
            while (sortedCollisions.Count > 0)
            {
                sortedCollisions = sortCollisions(sortedCollisions, totalRatio, sweptColliders);
                
                elapsedRatio = calculateElapsedRatio(sortedCollisions[0].Key, sortedCollisions[0].Value, totalRatio, sweptColliders);
                if (totalRatio + elapsedRatio > 1)
                {
                    elapsedRatio = 1-totalRatio;
                    totalRatio = 1;
                }
                else
                {
                    totalRatio += elapsedRatio;
                }

                //Debug.WriteLine("ER: " + elapsedRatio);
                //Debug.WriteLine("TR: " + totalRatio);

                foreach (IEntity entity in collidingDict.Keys.ToList())
                {
                    moveEntity(entity, elapsedRatio);
                }
                sweptColliders = getSweptColliders(collidingDict.Keys.ToList(), totalRatio);
                
                
                List<IEntity> repeatCollisions = handleCollision(sortedCollisions[0].Key, sortedCollisions[0].Value, sweptColliders);
                sortedCollisions.RemoveAt(0);
                collisions = getCollisions(repeatCollisions, sweptColliders);
                foreach (KeyValuePair<IEntity, ICollidable> pair in collisions)
                {
                    if (!sortedCollisions.Contains(pair))
                    {
                        sortedCollisions.Add(pair);
                    }   
                }
            }

            elapsedRatio = 1-totalRatio;
            totalRatio = 1;
            foreach(IEntity entity in collidingDict.Keys.ToList())
            {
                moveEntity(entity, elapsedRatio);
            }
            //checkGround();
        }

        /*
        private static Dictionary<IEntity, Vector2> getMovement(List<IEntity> movingList)
        {
            Dictionary<IEntity, Vector2> remainingMovement = new Dictionary<IEntity, Vector2>();
            foreach (IEntity entity in movingList)
            {
                Vector2 movement;
                if (entity is Entity)
                {
                    if (((Entity)entity).rigidbody != null)
                    {
                        movement = entity.rigidbody.velocity;
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

                remainingMovement.Add(entity, movement);
            }
            return remainingMovement;
        }
        */

        private static Dictionary<IEntity, Rectangle> getSweptColliders(List<IEntity> movingList, float totalRatio)
        {
            Dictionary<IEntity, Rectangle> sweptColliders = new Dictionary<IEntity, Rectangle>();
            foreach(IEntity entity in movingList)
            {
                if (entity.rigidbody != null)
                {
                    Rectangle sweepingBox = entity.game.GetCollider(entity.spriteType, entity.Position);
                    sweepingBox.Width += Math.Abs((int)((1 - totalRatio) * entity.rigidbody.velocity.X));
                    sweepingBox.Height += Math.Abs((int)((1 - totalRatio) * entity.rigidbody.velocity.Y));

                    if (entity.rigidbody.velocity.X < 0)
                    {
                        sweepingBox.X += (int)((1 - totalRatio) * entity.rigidbody.velocity.X);
                    }
                    if (entity.rigidbody.velocity.Y < 0)
                    {
                        sweepingBox.Y += (int)((1 - totalRatio) * entity.rigidbody.velocity.X);
                    }

                    sweptColliders.Add(entity, sweepingBox);
                } 
                else
                {
                    sweptColliders.Add(entity, entity.Collider);
                }
                
            }
            return sweptColliders;
        }

        private List<KeyValuePair<IEntity, ICollidable>> getCollisions(List<IEntity> movingList, Dictionary<IEntity, Rectangle> sweptColliders)
        {
            List<KeyValuePair<IEntity, ICollidable>> collisions = new List<KeyValuePair<IEntity, ICollidable>>();

            int movingCount = movingList.Count();
            for (int i = 0; i < movingCount; i++)
            {
                List<ICollidable> possibleCollisions = quadtree.Retrieve(new List<ICollidable>(), (ICollidable)movingList[i]);
                List<ICollidable> currentCollisions = new List<ICollidable>();
                List<ICollidable> newCollisions = new List<ICollidable>();
                foreach (ICollidable collidee in possibleCollisions)
                {
                    Rectangle entityCollider = sweptColliders[movingList[i]];
                    Rectangle collideeCollider = (collidee is IEntity && sweptColliders.ContainsKey((IEntity)collidee)) ? sweptColliders[(IEntity)collidee] : collidee.Collider;

                    if (movingList[i] != collidee && entityCollider.Intersects(collideeCollider))
                    {
                        if (collidingDict.ContainsKey(movingList[i]))
                        {
                            if ((collidingDict[movingList[i]]).Contains(collidee))
                            {
                                currentCollisions.Add(collidee);
                            }
                            else
                            {
                                newCollisions.Add(collidee);
                            }
                        }

                        if (movingList[i] is PlayerEntity)
                        {
                            turnRedCollisions(movingList[i], collidee);
                        }
                    }
                }

                if (movingList[i].rigidbody != null)
                {
                    movingList[i].rigidbody.CheckGround(movingList[i], possibleCollisions);
                }
                
                collidingDict[movingList[i]] = currentCollisions;

                foreach(ICollidable collidee in newCollisions)
                {
                    if (!(
                        collisions.Contains(new KeyValuePair<IEntity, ICollidable>(movingList[i], collidee)) || 
                        (collidee is IEntity && collisions.Contains(new KeyValuePair<IEntity, ICollidable>((IEntity)collidee, (ICollidable)movingList[i])))
                    ))
                    collisions.Add(new KeyValuePair<IEntity, ICollidable>(movingList[i], collidee));
                }
            }
            return collisions;
        }

        private List<KeyValuePair<IEntity, ICollidable>> sortCollisions(List<KeyValuePair<IEntity, ICollidable>> collisions, float totalRatio, Dictionary<IEntity, Rectangle> sweptColliders)
        {
            List<KeyValuePair<IEntity, ICollidable>> sortedCollisions = new List<KeyValuePair<IEntity, ICollidable>>();

            for (int i = 0; i < collisions.Count; i++)
            {
                float distance = getDistance(collisions[i].Key, collisions[i].Value, totalRatio, sweptColliders);
                int index = 0;
                for (int j = 0; j < sortedCollisions.Count; j++)
                { 
                    if (distance > getDistance(sortedCollisions[j].Key, sortedCollisions[j].Value, totalRatio, sweptColliders))
                    {
                        index++;
                    }
                }
                sortedCollisions.Insert(index, collisions[i]);
            }           
            return sortedCollisions;
        }

        private float getDistance(IEntity entity, ICollidable collidee, float totalRatio, Dictionary<IEntity, Rectangle> sweptColliders)
        {
            int direction = getDirection(entity, collidee, sweptColliders);
            float distance = 0;
            Rectangle entityCollider = entity.game.GetCollider(entity.spriteType, entity.Position);
            IEntity collideeEntity = (IEntity)collidee;
            Rectangle collideeCollider = collideeEntity.game.GetCollider(collideeEntity.spriteType, collideeEntity.Position);
            switch (direction)
            {
                case 0:
                    distance = Math.Abs(entityCollider.Top - collideeCollider.Bottom);
                    break;
                case 1:
                    distance = Math.Abs(entityCollider.Right - collideeCollider.Left);
                    break;
                case 2:
                    distance = Math.Abs(entityCollider.Bottom - collideeCollider.Top);
                    break;
                case 3:
                    distance = Math.Abs(entityCollider.Left - collideeCollider.Right);
                    break;
            }
            return distance;
        }

        private float calculateElapsedRatio(IEntity entity, ICollidable collidee, float totalRatio, Dictionary<IEntity, Rectangle> sweptColliders)
        {
            float elapsedRatio = 0;
            float distance = getDistance(entity, collidee, totalRatio, sweptColliders);
            int direction = getDirection(entity, collidee, sweptColliders);
            if (entity.rigidbody != null)
            {
                switch (direction)
                {
                    case 0:
                        if ((entity.rigidbody.velocity.Y) == 0) { elapsedRatio = (1-totalRatio); }
                        else { elapsedRatio = Math.Min(distance / Math.Abs(entity.rigidbody.velocity.Y), 1); }
                        break;
                    case 1:
                        if ((entity.rigidbody.velocity.X) == 0) { elapsedRatio = (1 - totalRatio); }
                        else { elapsedRatio = Math.Min(distance / Math.Abs(entity.rigidbody.velocity.X), 1); }
                        break;
                    case 2:
                        if ((entity.rigidbody.velocity.Y) == 0) { elapsedRatio = (1 - totalRatio); }
                        else { elapsedRatio = Math.Min(distance / Math.Abs(entity.rigidbody.velocity.Y), 1); }
                        break;
                    case 3:
                        if ((entity.rigidbody.velocity.X) == 0) { elapsedRatio = (1 - totalRatio); }
                        else { elapsedRatio = Math.Min(distance / Math.Abs(entity.rigidbody.velocity.X), 1); }
                        break;
                }
            }
            
            return elapsedRatio;
        }

        private void moveEntity(IEntity entity, float elapsedRatio)
        {
            if (entity.rigidbody != null)
            {
                Vector2 movement = elapsedRatio * entity.rigidbody.velocity;
                entity.Position += movement;
            }
        }

        private List<IEntity> handleCollision(IEntity entity, ICollidable collidee, Dictionary<IEntity, Rectangle> sweptColliders)
        {
            List<IEntity> repeatCollisions = new List<IEntity>();
            Rectangle entityCollider = (sweptColliders.ContainsKey(entity)) ? sweptColliders[entity] : entity.Collider;
            Rectangle collideeCollider = (collidee is IEntity && sweptColliders.ContainsKey((IEntity)collidee)) ? sweptColliders[(IEntity)collidee] : collidee.Collider;
            
            if (entityCollider.Intersects(collideeCollider))
            {
                
                /*
                if (
                    !(collidee is IEntity && ((IEntity)collidee).rigidbody != null &&
                    !(
                        (Math.Sign((int)entity.rigidbody.velocity.Y) == -1 && (entityCollider.Center.Y > collideeCollider.Center.Y)) |
                        (Math.Sign((int)entity.rigidbody.velocity.Y) == 1 && (entityCollider.Center.Y <= collideeCollider.Center.Y)) |
                        (Math.Sign((int)entity.rigidbody.velocity.X) == -1 && (entityCollider.Center.X > collideeCollider.Center.X)) |
                        (Math.Sign((int)entity.rigidbody.velocity.X) == 1 && (entityCollider.Center.X <= collideeCollider.Center.X)) |

                        (Math.Sign((int)((IEntity)collidee).rigidbody.velocity.Y) == -1 && (collideeCollider.Center.Y > entityCollider.Center.Y)) |
                        (Math.Sign((int)((IEntity)collidee).rigidbody.velocity.Y) == 1 && (collideeCollider.Center.Y <= entityCollider.Center.Y)) |
                        (Math.Sign((int)((IEntity)collidee).rigidbody.velocity.X) == -1 && (collideeCollider.Center.X > entityCollider.Center.X)) |
                        (Math.Sign((int)((IEntity)collidee).rigidbody.velocity.X) == 1 && (collideeCollider.Center.X <= entityCollider.Center.X))
                    ))
                )
                
                {
                */
                int direction = getDirection(entity, collidee, sweptColliders);
                if (
                    (collidingDict.ContainsKey(entity) && !(collidingDict[entity]).Contains(collidee)) && 
                    !(collidee is IEntity && collidee is IEntity && collidingDict.ContainsKey((IEntity)collidee) && (collidingDict[(IEntity)collidee]).Contains(entity))
                )
                {
                    (entity).OnCollisionEnter(collidee, direction);
                    (collidee).OnCollisionEnter((ICollidable)entity, oppositeDirection(direction));

                    if (collidingDict.ContainsKey(entity))
                    {
                        collidingDict[entity].Add(collidee);
                        repeatCollisions.Add((IEntity)entity);
                    }
                    if (collidee is IEntity && collidingDict.ContainsKey((IEntity)collidee))
                    {
                        repeatCollisions.Add((IEntity)collidee);
                    }                    
                    /*
                    quadtree.Remove(entity);
                    quadtree.Remove(collidee);
                    quadtree.Insert(entity);
                    quadtree.Insert(collidee);                       
                    */

                    //Debug.WriteLine("Direction: " + direction);
                    //Debug.WriteLine("Collision Handled");
                }                
            }
           
            //Debug.WriteLine("Entity: " + entity + " Collidee: " + collidee);
                        
            return repeatCollisions;
        }

        private void checkGround(Dictionary<IEntity, Rectangle> sweptColliders)
        {
            foreach(IEntity entity in collidingDict.Keys.ToList())
            {
                bool grounded = false;
                for (int i = 0; i < collidingDict[entity].Count; i++)
                {
                    if (!(collidingDict[entity][i] is BlockEntity && ((((BlockEntity)collidingDict[entity][i]).spriteType & SpriteEnum.allBlocks) == (SpriteEnum.block | SpriteEnum.hidden))))
                    {
                        if (getDirection(entity, collidingDict[entity][i], sweptColliders) == 2)
                        {
                            grounded = true;
                            break;
                        }
                    }
                }
                entity.rigidbody.isGrounded = grounded;
            }
        }

        public static int getDirection(ICollidable entity, ICollidable collider, Dictionary<IEntity, Rectangle> sweptColliders)
        {
            int direction = 0;
            Rectangle entityRectangle = (entity is IEntity) ? ((IEntity)entity).game.GetCollider(((IEntity)entity).spriteType, ((IEntity)entity).Position) : entity.Collider;
            Rectangle collideeRectangle = (entity is IEntity) ? ((IEntity)collider).game.GetCollider(((IEntity)collider).spriteType, ((IEntity)collider).Position) : collider.Collider;

            int xdistance;
            bool isRight;
            float xRatio;
            if (entityRectangle.X < collideeRectangle.X)
            {
                xdistance = collideeRectangle.Left - entityRectangle.Right;
                isRight = false;
            }
            else
            {
                xdistance = entityRectangle.Left - collideeRectangle.Right;
                isRight = true;
            }
            if ((((IEntity)entity).rigidbody.velocity.X) == 0) { xRatio = Int32.MinValue; }
            else { xRatio = xdistance / Math.Abs(((IEntity)entity).rigidbody.velocity.X); }
               
            int ydistance;
            bool isTop;
            float yRatio;
            if (entityRectangle.Y < collideeRectangle.Y)
            {
                ydistance = collideeRectangle.Top - entityRectangle.Bottom;
                isTop = false;
            }
            else
            {
                ydistance = entityRectangle.Top - collideeRectangle.Bottom;
                isTop = true;
            }
            if ((((IEntity)entity).rigidbody.velocity.Y) == 0) { yRatio = Int32.MinValue; }
            else { yRatio = ydistance / Math.Abs(((IEntity)entity).rigidbody.velocity.Y); }
        
            if (xRatio > yRatio)
            {
                if (isRight)
                {
                    direction = 1;
                }
                else
                {
                    direction = 3;
                }
            }
            else
            {
                if (isTop)
                {
                    direction = 0;
                }
                else
                {
                    direction = 2;
                }
            }
            
            return direction;


            /*
            int direction = 0;
            Rectangle entityCollider = (entity is IEntity && sweptColliders.ContainsKey((IEntity)entity)) ? sweptColliders[(IEntity)entity] : entity.Collider;
            Rectangle colliderCollider = (collider is IEntity && sweptColliders.ContainsKey((IEntity)collider)) ? sweptColliders[(IEntity)collider] : collider.Collider;
           
            Rectangle collisionRect = Rectangle.Intersect(entityCollider, colliderCollider);
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
            */
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