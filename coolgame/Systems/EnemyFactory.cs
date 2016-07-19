﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coolgame
{
    public static class EnemyFactory
    {
        private static Enemy[] enemies = new Enemy[1];
        private static ContentManager content;

        public static void LoadContent(ContentManager Content)
        {
            content = Content;
        }

        public static Enemy CreateEnemy(string enemyType)
        {
            Enemy tempEnemy;
            switch(enemyType.ToLower())
            {
                case "steve":
                    {
                        return new Enemy1(content);
                    }
                case "crawler":
                    {
                        return new Crawler(content);
                    }
                default:
                    {
                        Debug.Log("Tried to create an invalid enemy type!");
                        break;
                    }
            }
            return null;
        }
    }
}