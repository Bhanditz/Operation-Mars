﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace coolgame
{
    public class MotherShip : Enemy
    {
        private Rectangle detectionBox;
        private float altitudeVariation;
        private float altitudeVariationModifier;

        public MotherShip(ContentManager Content) : base(Content)
        {
            SetTexture("mothership");
            Y = 50;
            detectionBox = new Rectangle();
            detectionBox.Y = (int)Y;
            detectionBox.Height = Game.GAME_HEIGHT - detectionBox.Y;
            detectionBox.Width = Width;

            //attackSound = "enemylaser";
            altitudeVariationModifier = (float)GameManager.RNG.NextDouble() / 2 + .5f;

            movingSpeed = 20;
            attackPower = 215;
            attackSpeed = 8f;
            healthBar.MaxHealth = 450000;

            hitSound = "metalrobothit";

        }

        public override double X
        {
            get
            {
                return base.X;
            }

            set
            {
                base.X = value;
                detectionBox.X = (int)value;
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            altitudeVariation += deltaTime;

            attackCooldown += deltaTime;

            if (target != null && !target.Alive)
                target = null;

            if (target == null)
                target = CollisionManager.CollidesWithTurret(detectionBox);

            if (target == null)
                target = CollisionManager.CollidesWithForcefield(detectionBox);

            if (target == null)
                target = CollisionManager.CollidesWithBuilding(detectionBox);

            else
            {
                if (attackCooldown >= 1000f / attackSpeed)
                {
                    double projectileX = X + Width / 2;
                    double projectileY = Y + Height;
                    float projectileDirection = (float)Math.PI / 2;
                    EnemyProjectile p1 = new EnemyProjectile(content, projectileX, projectileY, projectileDirection, attackPower, "ufoprojectile");
                    EnemyProjectile p2 = new EnemyProjectile(content, projectileX - 15, projectileY, projectileDirection, attackPower, "ufoprojectile");
                    EnemyProjectile p3 = new EnemyProjectile(content, projectileX - 30, projectileY, projectileDirection, attackPower, "ufoprojectile");
                    EnemyProjectile p4 = new EnemyProjectile(content, projectileX + 15, projectileY, projectileDirection, attackPower, "ufoprojectile");
                    EnemyProjectile p5 = new EnemyProjectile(content, projectileX + 30, projectileY, projectileDirection, attackPower, "ufoprojectile");
                    attackCooldown = 0;
                    if (attackSound != null)
                        SoundManager.PlayClip(attackSound);
                }
            }

            if (direction == EnemyDirection.ToLeft)
            {
                X -= movingSpeed / 100 * deltaTime;

                if (X + Width < 0)
                    direction = EnemyDirection.ToRight;
            }
            else
            {
                X += movingSpeed / 100 * deltaTime;

                if (X > Game.GAME_WIDTH)
                    direction = EnemyDirection.ToLeft;
            }
            Y = Y - altitudeVariationModifier * Math.Sin(altitudeVariation / 300) * deltaTime * 6 / 100;
        }
    }
}
