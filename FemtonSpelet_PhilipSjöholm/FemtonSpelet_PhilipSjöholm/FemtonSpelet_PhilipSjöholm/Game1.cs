using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FemtonSpelet_PhilipSjöholm
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        // skapa en index som kollar var i arrayen den ska vara 
        // och sen när du loopar för att kolla checka emot den 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D _zeldaTexture;
        Texture2D _triforceTexture;
        Tile[,] tileArray = new Tile[4, 4];
        Vector2 _borderVector = new Vector2(50, 50);
        Random _rand = new Random();
        MouseState _mouseState;
        MouseState _oldMouseState;
        KeyboardState _oldKeyState;
        int whiteTileIndex_X;
        int whiteTileIndex_Y;
        int _numberOfClicks;
        int index = 0;
        bool lockGame;
        Video videoMovie;
        VideoPlayer _videoPlayer = new VideoPlayer();
        SpriteFont _spriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 700;
            IsMouseVisible = true;

        }


        protected override void Initialize()
        {
            base.Initialize();

            // creates new tiles and puts them in the 2dArray
            _oldKeyState = Keyboard.GetState();
            for (int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {
                    tileArray[i, j] = new Tile(_zeldaTexture, new Vector2(150 * i, 150 * j), new Rectangle(i * 150, j * 150, 150, 150), index, videoMovie, _videoPlayer);
                    index++;
                }
            }
            whiteTileIndex_X = 3;
            whiteTileIndex_Y = 3;

            ShuffleBoard();

        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _zeldaTexture = Content.Load<Texture2D>("zelda");
            _triforceTexture = Content.Load<Texture2D>("original");
            videoMovie = Content.Load<Video>("Wildlife");
            _spriteFont = Content.Load<SpriteFont>("SpriteFont1");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState _keystate = Keyboard.GetState();

            if (lockGame == true)
            {
                if (_keystate.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                {
                    Console.WriteLine("SPACE");
                    ShuffleBoard();
                    _numberOfClicks = 0;
                    lockGame = false;
                }
            }
            _oldKeyState = _keystate;

            base.Update(gameTime);
            _videoPlayer.Play(videoMovie);
            _videoPlayer.IsMuted = true;
            _videoPlayer.IsLooped = true;
            _oldMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            int indexX = (_mouseState.X - 50) / 150;
            int indexY = (_mouseState.Y - 50) / 150;

            Window.Title = "indexX: " + indexX + "indexY: " + indexY + " " + "Number of Moves: " + _numberOfClicks;

            if (lockGame == false)
            {
                if (_mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                {
                    /*  if (indexY == whiteTileIndex_Y)
                      {
                          if (indexX > whiteTileIndex_X)
                          {
                              int tempNrOfMoves = indexX - whiteTileIndex_X;
                              do
                              {
                                  SwapOut(whiteTileIndex_X, whiteTileIndex_Y, whiteTileIndex_X + 1, whiteTileIndex_Y);
                                  whiteTileIndex_X++;
                              } while (whiteTileIndex_X <= tempNrOfMoves);
                          }
                      }
                      else
                      {
                          int tempNrOfMoves = whiteTileIndex_X - indexX;
                          do
                          {
                              SwapOut(whiteTileIndex_X, whiteTileIndex_Y, whiteTileIndex_X - 1, whiteTileIndex_Y);
                              whiteTileIndex_X++;
                          } while (whiteTileIndex_X <= tempNrOfMoves);
                      }*/


                    // makes sure that you can only click the tiles next to the white tile
                    if (Math.Abs(indexX - whiteTileIndex_X) == 1 && Math.Abs(indexY - whiteTileIndex_Y) == 0 || Math.Abs(indexX - whiteTileIndex_X) == 0 && Math.Abs(indexY - whiteTileIndex_Y) == 1)
                    {
                        SwapOut(whiteTileIndex_X, whiteTileIndex_Y, indexX, indexY);
                        _numberOfClicks++;

                        CheckTiles();

                    }


                }
                // makes sure so that the tiles move when clicked 


            }
            foreach (Tile t in tileArray)
            {
                t.Update();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.DrawString(_spriteFont, "TILES CLICKED: " + _numberOfClicks, new Vector2(0, 0), Color.White);
            if (lockGame == true)
            {
                spriteBatch.DrawString(_spriteFont, "PRESS SPACE TO RESTART GAME", new Vector2(350, 0), Color.White);
            }
            spriteBatch.End();
            for (int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {   // ignores drawing the white tile 
                    if (i == whiteTileIndex_X && j == whiteTileIndex_Y)
                    {
                        continue;
                    }

                    tileArray[i, j].Draw(spriteBatch);
                }
            }
        }
        private void SwapOut(int posX1, int posY1, int posX2, int posY2)
        {
            // swaps out the white tile for the clicked drawn and change the position in 2dArray
            int tempX;
            int tempY;
            tempX = (int)tileArray[posX1, posY1].targetPosX;
            tempY = (int)tileArray[posX1, posY1].targetPosY;

            tileArray[posX1, posY1].targetPosX = (int)tileArray[posX2, posY2].targetPosX;
            tileArray[posX1, posY1].targetPosY = (int)tileArray[posX2, posY2].targetPosY;

            tileArray[posX2, posY2].targetPosX = tempX;
            tileArray[posX2, posY2].targetPosY = tempY;


            Tile tempTile;
            tempTile = tileArray[posX1, posY1];
            tileArray[posX1, posY1] = tileArray[posX2, posY2];
            tileArray[posX2, posY2] = tempTile;

            whiteTileIndex_X = posX2;
            whiteTileIndex_Y = posY2;


        }
        private void CheckTiles()
        {
            int loopCycle = 0;
            int tilesCorrect = 0;
            // checks tiles for their right position and then if all are correct locks the tiles from moving 
            for (int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {
                    if (tileArray[i, j].tile_index == loopCycle)
                    {
                        Console.WriteLine(loopCycle + "  " + tilesCorrect);
                        tilesCorrect++;
                        if (tilesCorrect == 15)
                        {
                            Console.WriteLine("YOU WIN !!!");
                            _videoPlayer.Pause();

                            lockGame = true;
                        }
                    }
                    loopCycle++;
                }
            }

        }

        private void ShuffleBoard()
        {
            for (int i = 0; i < 1000; i++)
            {
                int pos_X = _rand.Next(0, 4);
                int pos_Y = _rand.Next(0, 4);
                // only swaps the white tile to one next to it, making sure that it will only change the board in such a way that it is solveable 
                if ((pos_X == whiteTileIndex_X - 1) && (whiteTileIndex_Y == pos_Y) || (pos_X == whiteTileIndex_X + 1) && (whiteTileIndex_Y == pos_Y) || (pos_Y == whiteTileIndex_Y - 1) && (whiteTileIndex_X == pos_X) || (pos_Y == whiteTileIndex_Y + 1) && (whiteTileIndex_X == pos_X))
                {
                    SwapOut(whiteTileIndex_X, whiteTileIndex_Y, pos_X, pos_Y);
                }
            }
        }

    }
}
