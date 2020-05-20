using System;
using System.Collections.Generic;
using System.Text;

namespace Asterisks.Sprites
{
    public interface ICollidable
    {
        void OnCollide(Sprite sprite);
    }
}
