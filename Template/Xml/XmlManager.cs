using System.Xml;
using System.Xml.Linq;
using ImGuiNET;
using NativeFileDialogSharp;
using SkiaTemplate.Entities.UI;
using SkiaTemplate.Objects;

namespace SkiaTemplate.Xml;

public class XmlManager : ImGuiDrawable
{
    private const string LAST_SESSION_PATH = "Defaults.xml";

    public XmlManager() => Instance = this;
    public static XmlManager? Instance { get; private set; }

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

    public static event Action<XDocument>? OnLoadSession;
    public static event Action<XmlWriter>? OnSaveSession;

    /// <summary>
    ///     Saves the settings for the last session.
    /// </summary>
    private static void SaveSession(string path = LAST_SESSION_PATH)
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

        var mPath = path == LAST_SESSION_PATH ? Application.RunningPath : "";
        MessageHandler.SendMessage($"Saved to {mPath}{path}");
    }

    /// <summary>
    ///     Loads the last session.
    /// </summary>
    /// <returns></returns>
    private static void LoadSession(string path = LAST_SESSION_PATH)
    {
        OnLoadSession?.Invoke(XDocument.Load(path));
        
        var mPath = path == LAST_SESSION_PATH ? Application.RunningPath : "";
        MessageHandler.SendMessage($"Loaded {mPath}{path}");
    }
}