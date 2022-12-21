using ModLib;

namespace Hologram.FileTypes.XML;

public partial class XML : XMLStack, IDisposable
{
    public void Dispose()
    {
        Close();
        File.Dispose();
    }
}