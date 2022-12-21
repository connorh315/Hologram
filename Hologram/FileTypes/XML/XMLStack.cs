using ModLib;

namespace Hologram.FileTypes.XML;

public class XMLStack
{
    public ModFile File;
    
    public XMLStack Parent;

    public string Title;

    internal void Open(string title, bool shouldNewline, bool selfClose, XMLAttribute[]? attributes)
    {
        File.WriteByte((byte)'<');
        if (selfClose) File.WriteByte((byte)'/');
        File.WriteString(title);
        if (attributes != null)
        {
            foreach (XMLAttribute attribute in attributes)
            {
                File.WriteByte((byte)' ');
                File.WriteString(attribute.Key);
                File.WriteByte((byte)'=');
                File.WriteByte((byte)'"');
                File.WriteString(attribute.Value);
                File.WriteByte((byte)'"');
            }
        }
        
        File.WriteByte((byte)'>');

        if (shouldNewline) File.WriteByte((byte)'\n');
    }

    internal void WriteClose(string title)
    {
        File.WriteString("</");
        File.WriteString(title);
        File.WriteString(">\n");
    }

    /// <summary>
    /// Closes this tag
    /// </summary>
    /// <returns>Parent tag</returns>
    public XMLStack Close()
    {
        WriteClose(Title);

        return Parent;
    }

    private void WriteChild(string title, XMLAttribute[]? attributes = null, string? content = null)
    {
        if (content != null)
        {
            Open(title, false, false, attributes);
            File.WriteString(content);
            WriteClose(title);
        }
        else
        {
            Open(title, true, true, attributes);
        }
    }

    public XMLStack CreateChild(string title, XMLAttribute[]? attributes = null)
    {
        XMLStack stack = new XMLStack()
        {
            File = File,
            Parent = this,
            Title = title,
        };

        stack.Open(title, true, false, attributes);
        
        return stack;
    }

    /// <summary>
    /// Writes a child and then returns the parent so you can chain multiple siblings together
    /// </summary>
    /// <param name="title"></param>
    /// <param name="attributes"></param>
    /// <param name="content"></param>
    /// <returns>Parent tag</returns>
    public XMLStack CreateSibling(string title, XMLAttribute[]? attributes = null, string? content = null)
    {
        Parent.WriteChild(title, attributes, content);
        return this;
    }
}

public struct XMLAttribute
{
    public readonly string Key;
    public readonly string Value;

    public XMLAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }
}