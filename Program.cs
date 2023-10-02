using System.Numerics;
using Raylib_CsLo;
// using raylibExtras;

using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;

// using rlImGui_cs;
// using ImGuiNET;
// using static Raylib_CsLo.Camera3D;

namespace FormalEngine;

class Program
{
    public static unsafe void Main()
    {
        int screenWidth = 1280;
        int screenHeight = 720;
        // Raylib.SetTraceLogCallback(&Logging.LogConsole);
        Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
        Raylib.InitWindow(screenWidth, screenHeight, "Formal Engine");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        GuiLoadStyle("resources/ui-themes/cyber/style_cyber.rgs");
        Font uiFont = LoadFontEx("resources/ui-themes/cyber/Kyrou 7 Wide.ttf", 12, 0);
        GuiSetFont(uiFont);
        // DisableCursor();
        // rlImGui.Setup(true);
        BuildingEditor buildingEditor = new BuildingEditor();
        buildingEditor.Initialize();
        Camera3D camera = new Camera3D();
        camera.position = new Vector3(5.0f, 5.0f, 5.0f); // Camera position
        camera.target = new Vector3(0.0f, 2.0f, 0.0f); // Camera looking at point
        camera.up = new Vector3(0.0f, 1.0f, 0.0f); // Camera up vector (rotation towards target)
        camera.fovy = 45.0f; // Camera field-of-view Y
        camera.projection_ = CameraProjection.CAMERA_PERSPECTIVE;
        bool buttonPressed = false;

        while (!Raylib.WindowShouldClose())
        {
            screenWidth = Raylib.GetScreenWidth();
            screenHeight = Raylib.GetScreenHeight();
            if (IsCursorHidden())
                UpdateCamera(&camera);

            if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                if (IsCursorHidden())
                    EnableCursor();
                else
                    DisableCursor();
            }
            if (!buttonPressed)
            {
                BeginDrawing();
                GuiPanel(new Rectangle(0, 0, screenWidth, screenHeight), null);
                string text = "Formal Engine";
                int fontSize = 40;

                // Measure the text's width
                float textWidth = Raylib.MeasureText(text, fontSize);

                // Calculate the X-coordinate to center the text
                float centerX = (Raylib.GetScreenWidth() - textWidth) / 2;

                // Draw the centered text
                Raylib.DrawText(text, (int)centerX, 100, fontSize, WHITE);
                buttonPressed = GuiButton(
                    new Rectangle(screenWidth / 2 - 100, screenHeight / 2 - 50, 200, 100),
                    "Building editor"
                );
                EndDrawing();
            }
            else
            {
                buildingEditor.Loop(camera);
            }
        }
        buildingEditor.Unload();
        Raylib.CloseWindow();
    }
}
