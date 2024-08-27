using System.Xml;
using System.Xml.Linq;
using ImGuiNET;
using NativeFileDialogSharp;
using SkiaTemplate.Entities.UI;
using SkiaTemplate.Objects;

namespace SkiaTemplate.Xml;

public class XmlManager : ImGuiDrawable
{
    public static XmlManager? Instance { get; private set; }
    
    private const string LAST_SESSION_PATH = "Defaults.xml";

    public static event Action<XDocument>? OnLoadSession;
    public static event Action<XmlWriter>? OnSaveSession;

    public XmlManager() => Instance = this;

    /// <summary>
    /// Saves the settings for the last session.
    /// </summary>
    public static void SaveSession(string path = LAST_SESSION_PATH)
    {
        XmlWriterSettings writerSettings = new()
        {
            Indent = true,
            ConformanceLevel = ConformanceLevel.Auto
        };

        XmlWriter writer = XmlWriter.Create(path, writerSettings);
        
        // Do Root Setup
        writer.WriteStartDocument();
        
        OnSaveSession?.Invoke(writer);
        
        writer.WriteEndDocument();
        writer.Close();

        string mPath = path == LAST_SESSION_PATH ? Application.RunningPath : "";
        MessageHandler.SendMessage($"Saved to {mPath}{path}", 2f);
    }

    /// <summary>
    /// Loads the last session.
    /// </summary>
    /// <returns></returns>
    public static void LoadSession(string path = LAST_SESSION_PATH)
    {
        OnLoadSession?.Invoke(XDocument.Load(path));
        
        string mPath = path == LAST_SESSION_PATH ? Application.RunningPath : "";
        MessageHandler.SendMessage($"Loaded {mPath}{path}", 2f);
    }

    public void DrawImGui()
    {
        // Save and load settings.
        if (ImGui.CollapsingHeader("Save/Load"))
        {
            if (ImGui.Button("Load Defaults"))
                LoadSession();
            ImGui.SameLine();
            if (ImGui.Button("Save Defaults"))
                SaveSession();
        
            // Save and load files at specific locations.
            if (ImGui.Button("Load File"))
            {
                DialogResult dialog = Dialog.FileOpen("xml", Application.RunningPath);
                if (dialog.IsOk) LoadSession(dialog.Path);
            }
            ImGui.SameLine();
            if (ImGui.Button("Save File"))
            {
                DialogResult dialog = Dialog.FileSave("xml", Application.RunningPath);
                if (dialog.IsOk) SaveSession(dialog.Path);
            }
        }
    }
}