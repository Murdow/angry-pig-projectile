using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LancamentoProjetil
{
    class Positions
    {
        public Vector2 position;
        public Texture2D texture;               

        public Positions(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }        
    }
}
