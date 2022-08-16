using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Extensions
{
    internal static class Float
    {
        public static float Deg2Rad(this float deg)
        {
            return (float)((Math.PI / 180) * deg);
        }
    }
}
