using ModLib;

namespace Hologram.FileTypes.XML;

public partial class XML
{
    public static XML Create(string fileLocation, string rootTitle, XMLAttribute[] attributes = null)
    {
        XML xml = new XML()
        {
            File = ModFile.Create(fileLocation),
            Title = rootTitle
        };
        
        xml.File.WriteString("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");

        xml.Open(rootTitle, true, false, attributes);
        
        return xml;
    }
}