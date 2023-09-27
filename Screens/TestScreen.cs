using System;
using System.Linq;
using ImGuiNET;

namespace ImGuiDemo
{
    class TestScreen
    {
        private bool showMemoryEditor = false;
        private bool showAnotherWindow = false;
        private MemoryEditor memory = new MemoryEditor();

        private float f = 0.0f;
        private int counter = 0;
        private int dragInt = 0;
        private byte[] memoryEditorData;
        private uint tabBarFlags = (uint)ImGuiTabBarFlags.Reorderable;
        private bool[] opened = { true, true, true, true };
        private Random random = new Random();

        public TestScreen()
        {
            memoryEditorData = Enumerable.Range(0, 1024).Select(i => (byte)random.Next(255)).ToArray();
        }

        public void Update(EditorScreen editor, float dt)
        {
            if (showMemoryEditor)
            {
                memory.Update("Memory Editor", memoryEditorData, memoryEditorData.Length);
            }

            // Demo code adapted from the official Dear ImGui demo program:

            // 1. Show a simple window.
            // Tip: if we don't call ImGui.BeginWindow()/ImGui.EndWindow() the widgets automatically appears in a window called "Debug".
            {
                // Display some text (you can use a format string too)
                ImGui.Text("Hello, world!");

                // Edit 1 float using a slider from 0.0f to 1.0f
                ImGui.SliderFloat("float", ref f, 0, 1, f.ToString("0.000"));

                // Edit 3 floats representing a color
                ImGui.ColorEdit3("clear color", ref editor.clearColor);

                ImGui.Text($"Mouse position: {ImGui.GetMousePos()}");

                // Edit bools storing our windows open/close state
                ImGui.Checkbox("Another Window", ref showAnotherWindow);
                ImGui.Checkbox("Memory Editor", ref showMemoryEditor);

                // Buttons return true when clicked (NB: most widgets return true when edited/activated)
                if (ImGui.Button("Button"))
                {
                    counter++;
                }
                ImGui.SameLine(0, -1);
                ImGui.Text($"counter = {counter}");

                ImGui.DragInt("Draggable Int", ref dragInt);

                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");
            }

            // 2. Show another simple window. In most cases you will use an explicit Begin/End pair to name your windows.
            if (showAnotherWindow)
            {
                ImGui.Begin("Another Window", ref showAnotherWindow);
                ImGui.Text("Hello from another window!");
                if (ImGui.Button("Close Me"))
                {
                    showAnotherWindow = false;
                }
                ImGui.End();
            }
        }
    }
}
