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
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz",
                System.Globalization.CultureInfo.InvariantCulture);
            file.CreateChild("asset")
                .CreateChild("contributor")
                    .CreateSibling("author", null, HoloSettings.Author)
                    .CreateSibling("authoring_tool", null, "Hologram " + HoloSettings.Version)
                    .Close()
                .CreateSibling("created", null, currentDateTime)
                .CreateSibling("modified", null, currentDateTime)
                .CreateSibling("unit", new XMLAttribute[2] { new("name", "meter"), new("meter", "1") })
                .CreateSibling("up_axis", null, "Z_UP")
                .Close();
                

        }
    }
}