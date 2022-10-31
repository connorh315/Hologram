using ModLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSC.GSCWriter.NU20
{
    public class NU205C : NU2057
    {
        protected override int Version => 0x5C;

        protected override bool ReadNTBL()
        {
            bool result = base.ReadNTBL();
            file.Seek(-4, System.IO.SeekOrigin.Current);
            return result;
        }
    }
}
