using System;
using System.Collections.Generic;
using System.Text;

namespace WPFGame
{
    public class ConfigResolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}
