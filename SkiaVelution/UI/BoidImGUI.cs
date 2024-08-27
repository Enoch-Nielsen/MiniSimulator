using ImGuiNET;
using Silk.NET.OpenGL.Extensions.ImGui;
using SkiaVelution.Boids;

namespace SkiaVelution.UI;

public class BoidImGUI
{
    private FinchBoidBehaviour _finchBoidBehaviour;
    
    public BoidImGUI()
    {
        ImGui.NewFrame();
        ImGui.StyleColorsDark();

        _finchBoidBehaviour = Model.FinchBoidBehaviour;
    }

    public void RenderGUI(double delta, ImGuiController controller)
    {
        controller.Update((float)delta);

        _finchBoidBehaviour.CenteringFactor *= 10000;

        ImGui.Begin("Control Panel");
                
        if (ImGui.TreeNode("Finch Boid Behaviour"))
        {
            ImGui.SliderFloat("Vision Range", ref _finchBoidBehaviour.VisionRange, 0f, 128f);
            
            ImGui.SliderFloat("Group Vision Range", ref _finchBoidBehaviour.GroupVisionRange, 0f, 256f);
            ImGui.SliderFloat("Group Avoidance Factor", ref _finchBoidBehaviour.GroupAvoidanceFactor, 0f, 1f);
            
            ImGui.SliderFloat("Avoidance Factor", ref _finchBoidBehaviour.AvoidanceFactor, 0f, 1f);
            ImGui.SliderFloat("Matching Factor", ref _finchBoidBehaviour.MatchingFactor, 0f, 1f);
            ImGui.SliderFloat("Centering Factor / 10000", ref _finchBoidBehaviour.CenteringFactor, 0f, 0.5f);
            
            ImGui.SliderFloat("Turn Factor", ref _finchBoidBehaviour.TurnFactor, 0f, 8f);
            ImGui.SliderFloat("Minimum Speed", ref _finchBoidBehaviour.MinimumSpeed, 0f, _finchBoidBehaviour.MaximumSpeed-0.01f);
            ImGui.SliderFloat("Maximum Speed", ref _finchBoidBehaviour.MaximumSpeed, _finchBoidBehaviour.MinimumSpeed, 20f);
            ImGui.SliderFloat("XMargin", ref _finchBoidBehaviour.XMargin, 0f, 300f);
            ImGui.SliderFloat("YMargin", ref _finchBoidBehaviour.YMargin, 0f, 300f);
            ImGui.TreePop();
        }
                
        ImGui.End();
        ImGui.Render();
                
        controller.Render();
        
        // Tweak Centering Factor
        _finchBoidBehaviour.CenteringFactor /= 10000f;
        
        Model.UpdateBoidBehaviourFromUI(_finchBoidBehaviour);
    }
}