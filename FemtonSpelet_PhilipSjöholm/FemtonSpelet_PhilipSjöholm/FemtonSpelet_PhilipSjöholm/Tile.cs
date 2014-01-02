using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;  
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace FemtonSpelet_PhilipSjöholm
{
   
    class Tile
    { 
        Texture2D _tileTexture;
        Rectangle _rectangle;
        public Vector2 pos;
        public int targetPosX;
        public int targetPosY;
        public int tile_width;
        public int tile_height;
        public int tile_index;
        
        int _speed = 2; 
       
        Vector2 _offset = new Vector2(50, 50);
        Vector2 _startpoint;
        Video _videoMovie;
        VideoPlayer _videoPlayer;

        public Tile(Texture2D texture1 ,Vector2 firstpos, Rectangle rectangle, int tileIndex, Video vid, VideoPlayer videoPlayer )
        {
            tile_index = tileIndex;
            _rectangle = rectangle;
            _tileTexture = texture1;
            _videoPlayer = videoPlayer;
            pos = firstpos;
            tile_width = _rectangle.Width;
            tile_height = _rectangle.Height;
            _videoMovie = vid;
           // _rectangle.Width  = _videoMovie.Width / 4;
           // _rectangle.Height = _videoMovie.Height / 4;
            _startpoint = new Vector2(-175, -175);
           
            targetPosX = (int)pos.X;
            targetPosY = (int)pos.Y;
            // makes the tiles start outside border then update makes them move to their correct position 
            pos = _startpoint;
            
        }

        public void Draw(SpriteBatch spritebatch)
        {
           // Texture2D tempTexture = _videoPlayer.GetTexture();
            
            spritebatch.Begin();
            spritebatch.Draw(_tileTexture,pos + _offset , _rectangle, Color.White);
            spritebatch.End(); 
        }

        public void Update()
        {
            
            if (pos.X < targetPosX)
            {
                pos.X = pos.X + _speed;
            }
            if (pos.X > targetPosX)
            {
                pos.X = pos.X - _speed;
            }
            if (pos.Y < targetPosY)
            {
                pos.Y = pos.Y + _speed;
            }
            if (pos.Y > targetPosY)
            {
                pos.Y = pos.Y - _speed;
            }
        }
                
    }
}
