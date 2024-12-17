using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace Snake
{
    public class SnakeGame : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public int _score; //spelarens score
        private bool _gameOver = false; //om game over
        public SpriteFont _font; //spelets font
        Snake snake;
        Food food;

    public Texture2D _Picture; //background för spelet
        public Vector2 _screenOffset = Vector2.Zero; //screenshake från min gamla breakout spel

        const int WIDTH = 1400; //width och height för spelet (X)
        const int HEIGHT = 840;//Y
        Random rand = new Random(); //random

        const int SnakeSize = 10; //tjocklekn av snake
        const int gameHeight = 50; //antal blocks i höjjd
        const int gameWidth = 100; //antal blocks i bred
        private SoundEffect eatSound;
        private SoundEffect loseSound;  

        //private Song song;
        //Texture2D heartTexture;

        public SnakeGame()//Konstruktor
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        LoadData loadingScreen;
        bool _loading = true;
        public class LoadData
        {
            public void LoadMethod(out bool loading)
            {
                Thread.Sleep(3000);
                loading = false;
            }
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = gameHeight * SnakeSize; //snake size är 10 här och gameheight är 50 så 10x50=500

            _graphics.PreferredBackBufferWidth = gameWidth * SnakeSize;//10x100=1000

            base.Initialize();
        }

        protected override void LoadContent() //laddar ner innehål för spelet
        {
            snake = new Snake(GraphicsDevice, _spriteBatch, SnakeSize); // skapar ny orm

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("SpaceFont"); //här loads font
            food = new Food(Texture2D.FromFile(GraphicsDevice, "../../images/Food.png"), rand.Next(0, gameWidth) * SnakeSize, rand.Next(0, gameHeight) * SnakeSize); //spelets food alltso rock
            _Picture = Texture2D.FromFile(GraphicsDevice, "../../images/Darkgal2.jpg"); //background i jpg
                                                                                        //eatSound = Content.Load<SoundEffect>("eat");

            //Breakout för music:
            //Load Song:
            //song = Content.Load<Song>("Music/Sonic music");
            //MediaPlayer.Play(song); //background song
            //MediaPlayer.IsRepeating = true; //repeat song

            loadingScreen = new LoadData();
            Thread t = new Thread(() => loadingScreen.LoadMethod(out _loading));
            t.Start();
        }

        protected override void Update(GameTime gameTime) //ai
        {
            if (!_loading)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (!_gameOver)
                {
                    KeyboardState ks = Keyboard.GetState();
                    snake.Update(gameTime, ks);

                    // Beräkna radien för ormens huvud (halva SnakeSize)
                    int snakeRadius = SnakeSize / 2;

                    // Kontrollera om maten blir uppäten med hjälp av cirkelkollision
                    if (food.IsEaten(snake.pos, snakeRadius))
                    {
                        _score++;// Ökar poäng när ormen äter maten
                        snake.Grow();  // Gör ormen längre
                                       //eatSound.Play(); // sound when the snake eats the food
                        food.Respawn(
                            rand.Next(0, GraphicsDevice.Viewport.Width / SnakeSize) * SnakeSize,
                            rand.Next(0, GraphicsDevice.Viewport.Height / SnakeSize) * SnakeSize,
                            GraphicsDevice.Viewport.Width,
                            GraphicsDevice.Viewport.Height
                        );
                        // Hoppa över resten av Update denna gång eftersom mat har ätits
                        return;
                    }

                    // Kontr inte är en kollision med maten
                    if (snake.CheckCollision() || snake.OutOfbounds(gameWidth * SnakeSize, gameHeight * SnakeSize))
                    {
                        _gameOver = true;
                    }
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(); //allting börjar här med drawing i SB

            if (_loading)
            {
                GraphicsDevice.Clear(Color.White); //behövs inte har redan en background men för test
                _spriteBatch.DrawString(_font, "Loading...", new Vector2(300, 150), Color.Red, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
                // snake.Draw(gameTime, _spriteBatch); rita ut något i loading screen
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue); //behövs inte har redan en background men för test

                _spriteBatch.Draw(_Picture, new Rectangle((int)_screenOffset.X, (int)_screenOffset.Y, 1400, 800), Color.White);
                snake.Draw(gameTime, _spriteBatch);
                food.Draw(_spriteBatch);

                _spriteBatch.DrawString(_font, "Score: " + _score, new Vector2(10, 10), Color.White); //score i vänstra hörnan

                if (_gameOver) //om game over: når en väg då:
                {
                    _spriteBatch.Draw(_Picture, new Rectangle(0, 0, WIDTH, HEIGHT), Color.Black); //För att inte visa spelet utan bara texten

                    string gameOverText = "GAME OVER\\\\nPRESS ESC"; //Förlora då Game over
                    Vector2 gameOverSize = _font.MeasureString(gameOverText) * 0.5f; //text storlek
                    Vector2 gameOverPosition = new Vector2((WIDTH - gameOverSize.X) / 10, (HEIGHT - gameOverSize.Y) / 5); // Center the text
                    _spriteBatch.DrawString(_font, gameOverText, gameOverPosition, Color.Red, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
                }
            }
            _spriteBatch.End(); //slutet av SP allting efter ritas inte samma med begin allting innan ritas inte.

            base.Draw(gameTime);
        }
    }
}