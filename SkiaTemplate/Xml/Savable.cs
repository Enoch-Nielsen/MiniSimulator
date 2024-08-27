using System.Xml;
using System.Xml.Linq;

namespace SkiaTemplate.Xml;

[Serializable]
public abstract class Savable
{
    public Savable(bool useEvents)
    {
        if (!useEvents)
            return;
        
        XmlManager.OnLoadSession += LoadFromXml;
        XmlManager.OnSaveSession += SaveToXml;
    }
    
    public abstract void LoadFromXml(XDocument xml);

    public abstract void SaveToXml(XmlWriter xmlWriter);

    /// <summary>
    /// Writes a single value of type T to an XmlWriter instance.
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    protected static void ValueToXmlElement<T>(XmlWriter xmlWriter, T value, string name)
    {
        xmlWriter.WriteStartElement(name);
        xmlWriter.WriteValue(value);
        xmlWriter.WriteEndElement();
    }

    protected XElement GetRoot(XDocument xml)
    {
        XElement? rootElement = xml.Element(GetType().Name);
        
        if (rootElement == null)
            throw new Exception($"Root element {GetType().Name} not found in XML.");

        return rootElement;
    }
}