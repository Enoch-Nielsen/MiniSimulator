using System.Xml;
using System.Xml.Linq;
using ImGuiNET;
using SkiaTemplate.Objects;
using SkiaTemplate.Xml;

namespace SkiaTemplate.Settings;

[Serializable]
public class ProgramSettings : Savable, ImGuiDrawable
{
    public static ProgramSettings? Instance { get; private set; }
    public double RunSpeed = 1.0;
    
    public bool ShowFps = true;

    public ProgramSettings() : base(Instance == null)
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    public override void LoadFromXml(XDocument xml)
    {
        XElement rootElement = GetRoot(xml);

        string? runSpeed = rootElement.Element(nameof(RunSpeed))?.Value;
        string? showFps = rootElement.Element(nameof(ShowFps))?.Value;

        // Parse string data.
        if (!double.TryParse(runSpeed, out double _runSpeed))
            throw new Exception($"Failed to parse {nameof(RunSpeed)} in {GetType().Name}");
        
        if (!bool.TryParse(showFps, out bool _showFps))
            throw new Exception($"Failed to parse {nameof(ShowFps)} in {GetType().Name}");

        RunSpeed = _runSpeed;
        ShowFps = _showFps;
    }

    public override void SaveToXml(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement(GetType().Name);
            ValueToXmlElement(xmlWriter, RunSpeed, nameof(RunSpeed));
            ValueToXmlElement(xmlWriter, ShowFps, nameof(ShowFps)); 
        xmlWriter.WriteEndElement();
    }

    public void DrawImGui()
    {
        if (!ImGui.CollapsingHeader("Program Settings")) return;
        
        ImGui.Checkbox("Show Fps?", ref ShowFps);
        ImGui.InputDouble("Run Speed", ref RunSpeed, 0.1);
    }
}