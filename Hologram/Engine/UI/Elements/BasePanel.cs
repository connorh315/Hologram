using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI.Elements
{
    /// <summary>
    /// Offers a dummy UI element for other elements to attach to.
    /// </summary>
    public class BasePanel : UIElement
    {
        public override void DrawForHover(Color4 col)
        {
            throw new NotImplementedException();
        }
    }
}
