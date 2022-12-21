using Hologram.FileTypes.XML;
using Hologram.Objects;
using Hologram.Settings;

namespace Hologram.FileTypes.DAE;

public class DAE
{
    public static void Create(string fileLocation, Entity[] entities)
    {
        using (XML.XML file = XML.XML.Create(fileLocation, "COLLADA"))
        {
            string currentdatetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz",
                System.Globalization.CultureInfo.InvariantCulture);
            file.CreateChild("asset")
                .CreateChild("contributor")
                    .CreateSibling("author", null, HoloSettings.Author)
                    .CreateSibling("authoring_tool", null, "Hologram " + HoloSettings.Version)
                    .Close()
                .CreateSibling("created", null, currentdatetime)
                .CreateSibling("modified", null, currentdatetime)
                .CreateSibling("unit", new XMLAttribute[2] { new XMLAttribute("name", "meter"), new XMLAttribute("meter", "1") })
                .CreateSibling("up_axis", null, "Z_UP");

        }
    }
}