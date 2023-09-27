using System;
using System.Linq;
using ImGuiNET;

namespace ImGuiDemo
{
    class TestScreen
    {

        public TestScreen()
        {
		ImGui.Begin("My DearImGui Window");
		                ImGui.Text("Hello, world!");
		ImGui.End();
        }
    }

}
