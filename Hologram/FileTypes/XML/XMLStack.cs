using ModLib;

namespace Hologram.FileTypes.XML;

public class XMLStack
{
    public ModFile File;

    public Stack<string> Tags = new(16);

    internal void Open(string title, bool shouldNewline, bool selfClose, XMLAttribute[]? attributes)
    {
        File.WritePadding(2 * Tags.Count, (byte)' ');
        File.WriteByte((byte)'<');
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
        
        if (selfClose) File.WriteByte((byte)'/');
        File.WriteByte((byte)'>');

        if (shouldNewline) File.WriteByte((byte)'\n');
    }

    internal void WriteClose(string title, bool shouldIndent)
    {
        if (shouldIndent) File.WritePadding(2 * Tags.Count, (byte)' ');
        File.WriteString("</");
        File.WriteString(title);
        File.WriteString(">\n");
    }

    /// <summary>
    /// Pops most-recent tag from stack and closes it
    /// </summary>
    /// <returns>this</returns>
    public XMLStack Close()
    {
        WriteClose(Tags.Pop(), true);

        return this;
    }

    private void WriteChild(string title, XMLAttribute[]? attributes = null, string? content = null)
    {
        if (content != null)
        {
            Open(title, false, false, attributes);
            File.WriteString(content);
            WriteClose(title, false);
        }
        else
        {
            Open(title, true, true, attributes);
        }
    }

    /// <summary>
    /// Writes a child and advances the stack
    /// </summary>
    /// <param name="title"></param>
    /// <param name="attributes"></param>
    /// <returns>this</returns>
    public XMLStack CreateChild(string title, XMLAttribute[]? attributes = null)
    {
        Open(title, true, false, attributes);
        
        Tags.Push(title);

        return this;
    }

    /// <summary>
    /// Writes a child without pushing to the stack
    /// </summary>
    /// <param name="title"></param>
    /// <param name="attributes"></param>
    /// <param name="content"></param>
    /// <returns>this</returns>
    public XMLStack CreateSibling(string title, XMLAttribute[]? attributes = null, string? content = null)
    {
        WriteChild(title, attributes, content);
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