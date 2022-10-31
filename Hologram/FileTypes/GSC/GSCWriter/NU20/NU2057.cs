using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSC.GSCWriter.NU20
{
    public class NU2057 : NU2053
    {
        protected override int Version => 0x57;

        protected override bool ReadINFO()
        {
            bool result = base.ReadINFO();
            file.Seek(1, System.IO.SeekOrigin.Current);
            return result;
        }
    }
}
