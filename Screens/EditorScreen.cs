using System.Numerics;
using ImGuiNET;
using Raylib_cs;

namespace ImGuiDemo
{
    class EditorScreen
    {
        private bool showMetricsWindow = false;
        private bool showStyleEditor = false;
        private bool showAboutWindow = false;
        private bool showDemoWindow = true;
        public ImFontPtr font1;

        private TestScreen test = new TestScreen();

        public Vector3 clearColor = new Vector3(0.45f, 0.55f, 0.6f);

        public Color GetClearColor()
        {
            return new Color((byte)(clearColor.X * 255), (byte)(clearColor.Y * 255), (byte)(clearColor.Z * 255), (byte)255);
        }

        public void Load()
        {

        }

        public void Unload()
        {

        }

        public void Update(float dt)
        {
            ImGui.PushFont(font1);

            ShowTools(dt);

            if (ImGui.Begin("Editor", ImGuiWindowFlags.MenuBar))
            {
                // File menu for quick access
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("File"))
                    {
                        if (ImGui.MenuItem("Exit", "Alt+F4"))
                        {

                        }
                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Tools"))
                    {
                        if (ImGui.BeginMenu("ImGui"))
                        {
                            ImGui.MenuItem("Metrics", "", ref showMetricsWindow);
                            ImGui.MenuItem("Style Editor", "", ref showStyleEditor);
                            ImGui.MenuItem("About", "", ref showAboutWindow);
                            ImGui.MenuItem("Demo", "", ref showDemoWindow);
                            ImGui.EndMenu();
                        }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMenuBar();
                }

                if (ImGui.BeginTabBar("EditorTabs", ImGuiTabBarFlags.Reorderable))
                {
                    if (ImGui.BeginTabItem("Test"))
                    {
                        test.Update(this, dt);
                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }

                ImGui.PopFont();
                ImGui.End();
            }
        }

        /// <summary>
        /// Show enabled tool windows
        /// </summary>
        private void ShowTools(float dt)
        {
            if (showMetricsWindow)
            {
                ImGui.ShowMetricsWindow(ref showMetricsWindow);
            }

            ImGuiStylePtr style = ImGui.GetStyle();
            if (showStyleEditor)
            {
                if (ImGui.Begin("Dear ImGui Style Editor", ref showStyleEditor))
                {
                    ImGui.ShowStyleEditor(style);
                }
                ImGui.End();
            }

            if (showAboutWindow)
            {
                ImGui.ShowAboutWindow(ref showAboutWindow);
            }

            if (showDemoWindow)
            {
                // Normally user code doesn't need/want to call this because positions are saved in .ini file anyway.
                // Here we just want to make the demo initial state a bit more friendly!
                ImGui.SetNextWindowPos(new Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref showDemoWindow);
            }
        }
    }
}
