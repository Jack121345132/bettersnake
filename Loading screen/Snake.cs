using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Snake
{
    class Snake
    {
        // const int updateInterval = 33;
        public Rectangle Head => new Rectangle((int)pos.X, (int)pos.Y, size, size); //chatgpt hjälpte mig här det är ormens huvud hitbox som är skit vitkgit för mat, väggar och annat
        int size; //size av snake
        Vector2 dir; //riktning
        float speed; //hastiget
        public Vector2 pos;//postion
        List<Vector2> tailList; //lista för tail ju mer mat ju längre tail
        Texture2D texture; //ormens ``
    public Snake(GraphicsDevice graphics, SpriteBatch spriteBatch, int size) //Konstruktor för orm med hjälp av jonas
        {
            this.size = size;
            dir = new Vector2(1, 0); // rör sig i höger
            speed = size * 0.2f;

            //Jonas hjälpte mig här hade skit konstigt problem som va skit svår att lösa: texutre:
            texture = new Texture2D(graphics, size, size);
            Color[] c = new Color[size * size];
            for (int i = 0; i < c.Length; i++) c[i] = Color.White;
            texture.SetData(c);

            int posX = graphics.Viewport.Width / 2;
            int posY = graphics.Viewport.Height / 2;
            pos = new Vector2(posX, posY);

            tailList = new List<Vector2>(); //ormens svans lista
        }

        public void Update(GameTime gameTime, KeyboardState ks) //knappar vad händer om trycker höger,vänster, ner och up
        {
            if (ks.IsKeyDown(Keys.Up) && dir.Y == 0)
            {
                dir = new Vector2(0, -1);//up
            }
            if (ks.IsKeyDown(Keys.Down) && dir.Y == 0)
            {
                dir = new Vector2(0, 1);//ner
            }
            if (ks.IsKeyDown(Keys.Left) && dir.X == 0)
            {
                dir = new Vector2(-1, 0);//vänster
            }
            if (ks.IsKeyDown(Keys.Right) && dir.X == 0)
            {
                dir = new Vector2(1, 0);//höger
            }

            // Flytta kroppen
            tailList.Insert(0, pos); //här fick hjälp av ai postion för svansen
            pos += dir * speed;//postion i huvuud

            // Ta bort sista delen om den inte växer
            if (tailList.Count > 5) // ai
            {
                tailList.RemoveAt(tailList.Count - 1); //ai han sa till ortem äter
            }
        }

        public bool CheckCollision()  //lite från breakoutn och ai. collision med sig själv
        {
            for (int i = 1; i < tailList.Count; i++)
            {
                if (pos == tailList[i])
                {
                    return true; //med sig själv krokar
                }
            }
            return false;
        }

        public bool OutOfbounds(int gameWidth, int gameHeight) //här för gameover om ormen är utanför
        {
            return pos.X < 0 || pos.Y < 0 || pos.X >= gameWidth || pos.Y >= gameHeight;
        }

        public void Grow()//mer mat längre orm
        {
            tailList.Add(pos); //  till ny del med tailist alltså i listan
        }

        public void Draw(GameTime gameTime, SpriteBatch sb) //draw nya parts
        {
            sb.Draw(texture, pos, Color.Red);
            foreach (var part in tailList)
            {
                sb.Draw(texture, part, Color.White);
            }
        }
    }
}