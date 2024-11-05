using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Snake
{
    internal class Food
    {
        Texture2D texture;
        Vector2 pos;
        public Vector2 Position => pos; // ai
    public Food(Texture2D tex, int x, int y)
        {
            pos = new Vector2(x, y); //respawn för food
            texture = tex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, Color.Wheat);
        }

        public void Respawn(int x, int y, int maxWidth, int maxHeight, int margin = 10) //ai
        {
            int foodWidth = texture.Width;
            int foodHeight = texture.Height;
            // Begränsa matens position inom spelramen med en marginal
            pos = new Vector2(
    Math.Clamp(x, margin, maxWidth - foodWidth - margin),
    Math.Clamp(y, margin, maxHeight - foodHeight - margin));
        }

        public bool IsEaten(Vector2 snakePos, int snakeRadius)
        {
            // Beräkna avstånd mellan ormen och maten
            float distance = Vector2.Distance(snakePos, pos);

            // Radien för maten är halva storleken på mat-texturen
            int foodRadius = texture.Width / 2;

            // Om avståndet är mindre än summan av radierna, returnera true (kollision)
            return distance <= snakeRadius + foodRadius;
        }

    }
}