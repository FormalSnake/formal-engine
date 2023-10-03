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
        Raylib.SetConfigFlags(
            ConfigFlags.FLAG_MSAA_4X_HINT
                | ConfigFlags.FLAG_VSYNC_HINT
                | ConfigFlags.FLAG_WINDOW_UNDECORATED
        );
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

        Vector2 mousePosition = Vector2.Zero;
        Vector2 windowPosition = new Vector2(
            Raylib.GetMonitorWidth(GetCurrentMonitor()) / 2 - GetScreenWidth() / 2,
            Raylib.GetScreenHeight() / 2 - GetScreenHeight() / 4
        );
        Vector2 panOffset = mousePosition;
        bool dragWindow = false;
        SetWindowPosition((int)windowPosition.X, (int)windowPosition.Y);

        bool exitWindow = false;

        while (!exitWindow && !Raylib.WindowShouldClose())
        {
            mousePosition = GetMousePosition();
            screenWidth = Raylib.GetScreenWidth();
            screenHeight = Raylib.GetScreenHeight();

            if (IsMouseButtonDown(MOUSE_LEFT_BUTTON) && !dragWindow)
            {
                if (CheckCollisionPointRec(mousePosition, new Rectangle(0, 0, screenWidth, 20)))
                {
                    dragWindow = true;
                    panOffset = mousePosition;
                }
            }

            if (dragWindow)
            {
                windowPosition.X += (mousePosition.X - panOffset.X);
                windowPosition.Y += (mousePosition.Y - panOffset.Y);

                SetWindowPosition((int)windowPosition.X, (int)windowPosition.Y);

                if (IsMouseButtonReleased(MOUSE_LEFT_BUTTON))
                    dragWindow = false;
            }

            if (IsCursorHidden())
                UpdateCamera(&camera);

            if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                if (IsCursorHidden())
                    EnableCursor();
                else
                    DisableCursor();
            }
            BeginDrawing();
            exitWindow = GuiWindowBox(
                new Rectangle(0, 0, screenWidth, screenHeight),
                "FormalEngine"
            );
            if (buttonPressed)
            {
                buildingEditor.Loop(camera);
                if (GuiButton(new Rectangle(10, 33, 100, 40), "Exit"))
                {
                    // Exit the building editor and reset the buttonPressed flag
                    buttonPressed = false;
                    // Add any necessary logic to clean up the building editor here
                }
            }
            if (!buttonPressed)
            {
                // BeginDrawing();
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
                // EndDrawing();
            }

            EndDrawing();
        }
        buildingEditor.Unload();
        Raylib.CloseWindow();
    }
}
