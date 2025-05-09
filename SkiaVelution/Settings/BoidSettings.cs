using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using ImGuiNET;
using SkiaTemplate.Objects;
using SkiaTemplate.Xml;

namespace SkiaTemplate.Settings;

[Serializable]
public class BoidSettings : Savable, ImGuiDrawable
{
    public static float FinchSpeed = 10.0f;

    public static float VisionRange = 20f;
    
    public static float GroupVisionRange = 128f;
    public static float GroupAvoidanceFactor = 0.02f;
    
    public static float AvoidanceFactor = 0.03f;
    public static float MatchingFactor = 0.25f;
    public static float CenteringFactor = 0f;
    
    public static float TurnFactor = 4f;
    public static float MinimumSpeed = 3f;
    public static float MaximumSpeed = 7.5f;
    public static float XMargin = 100f;
    public static float YMargin = 100f;
    
    // FinchBoidBehaviour = new FinchBoidBehaviour()
    // {
    //     VisionRange = 20f,
    //     
    //     GroupVisionRange = 128f,
    //     GroupAvoidanceFactor = 0.02f,
    //     
    //     AvoidanceFactor = 0.03f,
    //     MatchingFactor = 0.25f,
    //     CenteringFactor = 0f,
    //     
    //     TurnFactor = 4f,
    //     MinimumSpeed = 3f,
    //     MaximumSpeed = 7.5f,
    //     XMargin =  100f,
    //     YMargin = 100f,
    // };
    
    public BoidSettings() : base(Instance == null)
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    public static BoidSettings? Instance { get; private set; }

    public void DrawImGui()
    {
        if (ImGui.CollapsingHeader("Boid Settings"))
        {
            ImGui.SliderFloat("Finch Speed", ref FinchSpeed, 0f, 100f);
            ImGui.SliderFloat("Vision Range", ref VisionRange, 0f, 600f);
            ImGui.SliderFloat("Group Vision Range", ref GroupVisionRange, 0f, 600f);
            ImGui.SliderFloat("Group Avoidance Factor", ref GroupAvoidanceFactor, 0f, 1f);
            ImGui.SliderFloat("Avoidance Factor", ref AvoidanceFactor, 0f, 1f);
            ImGui.SliderFloat("Matching Factor", ref MatchingFactor, 0f, 3f);
            ImGui.SliderFloat("Centering Factor", ref CenteringFactor, 0f, 3f);
            ImGui.SliderFloat("TurnFactor", ref TurnFactor, 0f, 30f);
            ImGui.SliderFloat("Minimum Speed", ref MinimumSpeed, 0f, MaximumSpeed);
            ImGui.SliderFloat("Maximum Speed", ref MaximumSpeed, MinimumSpeed, 15f);
            ImGui.SliderFloat("XMargin", ref XMargin, 0f, 1000f);
            ImGui.SliderFloat("YMargin", ref YMargin, 0f, 1000f);
        }
    }

    public override void LoadFromXml(XDocument xml)
    {
        XElement rootElement = GetTarget(xml);

        var visionRange = rootElement.Element(nameof(VisionRange))?.Value;
        var groupVisionRange = rootElement.Element(nameof(GroupVisionRange))?.Value;
        var groupAvoidanceFactor = rootElement.Element(nameof(GroupAvoidanceFactor))?.Value;
        var avoidanceFactor = rootElement.Element(nameof(AvoidanceFactor))?.Value;
        var matchingFactor = rootElement.Element(nameof(MatchingFactor))?.Value;
        var centeringFactor = rootElement.Element(nameof(CenteringFactor))?.Value;
        var turnFactor = rootElement.Element(nameof(TurnFactor))?.Value;
        var minimumSpeed = rootElement.Element(nameof(MinimumSpeed))?.Value;
        var maximumSpeed = rootElement.Element(nameof(MaximumSpeed))?.Value;
        var xMargin = rootElement.Element(nameof(XMargin))?.Value;
        var yMargin = rootElement.Element(nameof(YMargin))?.Value;

        // Parse string data.
        if (!double.TryParse(visionRange, out var _visionRange))
            throw new Exception($"Failed to parse {nameof(VisionRange)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(groupVisionRange, out var _groupVisionRange))
            throw new Exception($"Failed to parse {nameof(GroupVisionRange)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(groupAvoidanceFactor, out var _groupAvoidanceFactor))
            throw new Exception($"Failed to parse {nameof(GroupAvoidanceFactor)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(avoidanceFactor, out var _avoidanceFactor))
            throw new Exception($"Failed to parse {nameof(AvoidanceFactor)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(matchingFactor, out var _matchingFactor))
            throw new Exception($"Failed to parse {nameof(MatchingFactor)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(centeringFactor, out var _centeringFactor))
            throw new Exception($"Failed to parse {nameof(CenteringFactor)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(turnFactor, out var _turnFactor))
            throw new Exception($"Failed to parse {nameof(TurnFactor)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(minimumSpeed, out var _minimumSpeed))
            throw new Exception($"Failed to parse {nameof(MinimumSpeed)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(maximumSpeed, out var _maximumSpeed))
            throw new Exception($"Failed to parse {nameof(MaximumSpeed)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(xMargin, out var _xMargin))
            throw new Exception($"Failed to parse {nameof(XMargin)} in {GetType().Name}");// Parse string data.
        
        if (!double.TryParse(yMargin, out var _yMargin))
            throw new Exception($"Failed to parse {nameof(YMargin)} in {GetType().Name}");

        VisionRange = (float)_visionRange;
        GroupVisionRange = (float)_groupVisionRange;
        GroupAvoidanceFactor = (float)_groupAvoidanceFactor;
        AvoidanceFactor = (float)_avoidanceFactor;
        MatchingFactor = (float)_matchingFactor;
        CenteringFactor = (float)_centeringFactor;
        TurnFactor = (float)_turnFactor;
        MinimumSpeed = (float)_minimumSpeed;
        MaximumSpeed = (float)_maximumSpeed;
        XMargin = (float)_xMargin;
        YMargin = (float)_yMargin;
    }

    public override void SaveToXml(XmlWriter xmlWriter)
    {
        Console.WriteLine("Boid");
        xmlWriter.WriteStartElement(GetType().Name);
        ValueToXmlElement(xmlWriter, VisionRange, nameof(VisionRange));
        ValueToXmlElement(xmlWriter, GroupVisionRange, nameof(GroupVisionRange));
        ValueToXmlElement(xmlWriter, GroupAvoidanceFactor, nameof(GroupAvoidanceFactor));
        ValueToXmlElement(xmlWriter, AvoidanceFactor, nameof(AvoidanceFactor));
        ValueToXmlElement(xmlWriter, MatchingFactor, nameof(MatchingFactor));
        ValueToXmlElement(xmlWriter, CenteringFactor, nameof(CenteringFactor));
        ValueToXmlElement(xmlWriter, TurnFactor, nameof(TurnFactor));
        ValueToXmlElement(xmlWriter, MinimumSpeed, nameof(MinimumSpeed));
        ValueToXmlElement(xmlWriter, MaximumSpeed, nameof(MaximumSpeed));
        ValueToXmlElement(xmlWriter, XMargin, nameof(XMargin));
        ValueToXmlElement(xmlWriter, YMargin, nameof(YMargin));
        xmlWriter.WriteEndElement();
    }
}