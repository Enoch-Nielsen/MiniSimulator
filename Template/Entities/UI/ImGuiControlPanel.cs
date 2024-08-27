using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using SkiaTemplate.Settings;
using SkiaTemplate.Xml;

namespace SkiaTemplate.Entities.UI;

public class ImGuiControlPanel
{
    public ImGuiControlPanel()
    {
        ImGui.NewFrame();
        ImGui.StyleColorsDark();
    }
    
    public void RenderGUI(double delta, ImGuiController controller)
    {
        controller.Update((float)delta);

        ImGui.Begin("Control Panel");

        if (ImGui.Button("Close Application"))
            WindowManager.ActiveWindow?.Close();

        ImGui.Spacing();

        XmlManager.Instance?.DrawImGui();
        ProgramSettings.Instance?.DrawImGui();
        
        ImGui.End();
        ImGui.Render();

        controller.Render();
    }
}