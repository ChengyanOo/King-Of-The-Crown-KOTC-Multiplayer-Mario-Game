using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Sprint1.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprint1.Entities;
using System.Diagnostics;
using System.Threading;
using System.Linq;

namespace Sprint1.Scrolling
{
    public class Layer
    {
        private readonly Camera _camera;
        public Vector2 Parallax { get; set; }
        public Rectangle? Limits 
        {
            get 
            {
                return _camera.Limits;
            }
            set
            {
                _camera.Limits = value;
            }
        } 

        public List<ISprite> SpriteList { get; private set; }
        private List<ISprite> updateList;
        private Rectangle screenRectangle;
        private Rectangle originalScreenRectangle;
        public Layer(Camera camera, int screenWidth, int screenHeight, Vector2 parallax, Rectangle limits)
        {
            _camera = camera;
            Parallax = parallax;
            SpriteList = new List<ISprite>();
            Limits = limits;
            screenRectangle = getScreen(screenWidth, screenHeight);
            originalScreenRectangle = getScreen(screenWidth, screenHeight);

            updateList = new List<ISprite>();
        }

        private Rectangle getScreen(int screenWidth, int screenHeight)
        {
            return new Rectangle(_camera.Position.ToPoint(), (new Point((int)(screenWidth / Parallax.X), screenHeight)));
        }

        public void Draw(SpriteBatch spriteBatch, bool isColliderVisible)
        {
            //Parallax scrolling
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix(Parallax));
            
            //Only draw sprites that are on the screen
            for (int i = 0; i < SpriteList.Count; i++)
            {
                if (OnScreen(SpriteList[i]))
                {
                    if (isColliderVisible && SpriteList[i] is IEntity)
                    {
                        ((IEntity)SpriteList[i]).DrawCollider(spriteBatch);
                    }
                    SpriteList[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, Vector2 screenCenter)
        {
            /*
            if (screenCenter.X > _camera.Origin.X)
            {
                ScrollRight(screenCenter);
            }
            */
            UpdateUpdateList();
            //Only update sprites that are on the screen or have ever been on the screen
            int i = 0;
            while(i < updateList.Count)
            {
                updateList[i].Update(gameTime);
                i++;
            }
        }

        public void ScrollRight(Vector2 screenCenter)
        {
            _camera.LookAt(screenCenter);
            screenRectangle.X = (int)(_camera.Position.X * Parallax.X);
            screenRectangle.Y = (int)(_camera.Position.Y * Parallax.Y);
        }
        public bool OnScreen(ISprite sprite)
        {
            return screenRectangle.Contains(sprite.Position) || screenRectangle.Contains(sprite.RightEdge, sprite.Position.Y);
        }

        public void UpdateUpdateList()
        {
            for (int i = 0; i < SpriteList.Count; i++)
            {
                if (!updateList.Contains(SpriteList[i]) && OnScreen(SpriteList[i]))
                {
                    updateList.Add(SpriteList[i]);
                }
            }
            updateList = updateList.Intersect(SpriteList).ToList();
        }

        public void ResetCamera(Rectangle limits)
        {
            _camera.Limits = Limits;
            _camera.Position = limits.Location.ToVector2();
            screenRectangle = originalScreenRectangle;
        }
    }
}

