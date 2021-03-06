﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace coolgame
{
    public abstract class Entity
    {
        private double x;
        private double y;
        protected Texture2D texture;
        protected Vector2 scale;

        protected Rectangle collisionBox;
        public Rectangle CollisionBox
        {
            get { return collisionBox; }
        }

        protected ContentManager content;
        private Rectangle sourceRectangle;
        private Rectangle drawRectangle;
        private Vector2 origin;
        private int totalFramesX;
        private int totalFramesY;
        private int currentFrameX;
        private int currentFrameY;
        private float frameUpdateTime;
        private float rotation;
        private bool enableAnimation;
        private float animationSpeed = .025f;
        protected HealthBar healthBar;
        private bool enableHealthBar;
        private bool autoHideHealthBar = true;
        private bool alive = true;
        protected SpriteEffects spriteEffects;
        protected float layerDepth;
        protected Color color = Color.White;

        public virtual double X
        {
            get { return x; }
            set
            {
                x = value;
                drawRectangle.X = (int)(value + origin.X * scale.X);
                healthBar.X = (int)value + Width / 2;
                collisionBox.X = (int)value;
            }
        }

        public virtual double Y
        {
            get { return y; }
            set
            {
                y = value;
                drawRectangle.Y = (int)(value + origin.Y * scale.Y);
                healthBar.Y = (int)value - 20;
                collisionBox.Y = (int)value;
            }
        }

        public virtual int Width
        {
            get { return sourceRectangle.Width; }
            set
            {
                if (value > 0)
                {
                    sourceRectangle.Width = value;
                    totalFramesX = texture.Width / value;
                    healthBar.X = (int)X + value / 2;
                    origin.X = value / 2;
                    collisionBox.Width = value;
                    drawRectangle.Width = (int)(value * scale.X);
                }
            }
        }

        public virtual int Height
        {
            get { return sourceRectangle.Height; }
            set
            {
                sourceRectangle.Height = value;
                totalFramesY = texture.Height / value;
                origin.Y = value / 2;
                collisionBox.Height = value;
                drawRectangle.Height = (int)(value * scale.Y);
            }
        }

        public virtual Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                drawRectangle.X = (int)(X + origin.X * value.X);
                drawRectangle.Y = (int)(Y + origin.Y * value.Y);
                drawRectangle.Width = (int)(value.X * Width);
                drawRectangle.Height = (int)(value.Y * Height);
            }
        }

        public bool EnableAnimation
        {
            get { return enableAnimation; }
            set
            {
                enableAnimation = value;

                if (value == false)
                {
                    currentFrameX = 0;
                    currentFrameY = 0;
                    sourceRectangle.X = 0;
                    sourceRectangle.Y = 0;
                }
            }
        }

        public float AnimationSpeed
        {
            get { return animationSpeed; }
            set { animationSpeed = value; }
        }

        public bool EnableHealthBar
        {
            get { return enableHealthBar; }
            set { enableHealthBar = value; }
        }

        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        
        public virtual bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public SpriteEffects Effects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }

        public Entity(ContentManager content)
        {
            sourceRectangle = new Rectangle();
            totalFramesX = 1;
            totalFramesY = 1;
            healthBar = new HealthBar(content);
            X = Y = 0;
            this.content = content;
            scale = Vector2.One;
        }

        public bool Collides(Entity e)
        {
            if (collisionBox.Intersects(e.collisionBox))
                return true;
            return false;
        }

        public bool Collides(Rectangle r)
        {
            if (collisionBox.Intersects(r))
                return true;
            return false;
        }

        public virtual void Update(float deltaTime)
        {
            if (alive)
            {
                if (EnableAnimation && animationSpeed > 0)
                {
                    frameUpdateTime += deltaTime;
                    if (frameUpdateTime >= 1 / animationSpeed)
                    {
                        frameUpdateTime = 0;

                        currentFrameX++;
                        if (currentFrameX == totalFramesX)
                        {
                            currentFrameX = 0;
                            currentFrameY++;
                        }
                        if (currentFrameY == totalFramesY)
                        {
                            currentFrameY = 0;
                        }

                        sourceRectangle.X = currentFrameX * Width;
                        sourceRectangle.Y = currentFrameY * Height;
                    }
                }

                healthBar.Update(deltaTime);
            }
        }

        public void SetTexture(string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
            if (Width > 0)
            {
                totalFramesX = texture.Width / Width;
                totalFramesY = texture.Height / Height;
            }
            
            if (!enableAnimation)
            {
                Width = texture.Width;
                Height = texture.Height;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(texture, null, drawRectangle, sourceRectangle, origin, rotation, Vector2.One, color, spriteEffects, layerDepth);

                if (enableHealthBar)
                    healthBar.Draw(spriteBatch);
            }
        }

        public virtual void InflictDamage(int hitpoints)
        {
            healthBar.Health -= hitpoints;
            if (healthBar.Health <= 0)
            {
                Alive = false;
            }
        }
    }
}
