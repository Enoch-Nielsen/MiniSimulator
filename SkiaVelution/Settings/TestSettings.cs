using System.Xml;
using System.Xml.Linq;
using SkiaTemplate.Xml;

namespace SkiaTemplate.Settings;

[Serializable]
public class TestSettings : Savable
{
    public double RunSpeed = 1.0;

    public bool ShowFps = true;

    public TestSettings(bool useEvents) : base(useEvents)
    {
    }

    public override void LoadFromXml(XDocument xml)
    {
    }

    public override void SaveToXml(XmlWriter xmlWriter)
    {
    }
}