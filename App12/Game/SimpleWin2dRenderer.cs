using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win2DGameEngine;

namespace App12.Game
{
    public class SimpleWin2dRenderer : Win2DGameEngine.Win2dRenderer
    {
        public SimpleWin2dRenderer() : base(new SimpleGameEngine())
        {

        }
    }
}
