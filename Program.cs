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

        Camera3D camera = new Camera3D();
        camera.position = new Vector3(5.0f, 5.0f, 5.0f); // Camera position
        camera.target = new Vector3(0.0f, 2.0f, 0.0f); // Camera looking at point
        camera.up = new Vector3(0.0f, 1.0f, 0.0f); // Camera up vector (rotation towards target)
        camera.fovy = 45.0f; // Camera field-of-view Y
        camera.projection_ = CameraProjection.CAMERA_PERSPECTIVE;

        Image img = GenImageChecked(256, 256, 64, 64, LIGHTGRAY, WHITE);
        Texture tx = LoadTextureFromImage(img);
        Mesh planeMesh = GenMeshPlane(10.0f, 10.0f, 3, 3);
        Model planeModel = LoadModelFromMesh(planeMesh);
        Model model = Raylib.LoadModel("resources/models/gltf/robot.glb");
        SetMaterialTexture(planeModel.materials, MATERIAL_MAP_DIFFUSE, tx);
        SelectableObject building = new SelectableObject();
        Vector3 buildingPos = new Vector3(0.0f, 1.0f, 0.0f);
        building.Initialize("Barracks", buildingPos);
        Rectangle vector3EditorRect = new Rectangle(20, 40, 300, 140);
float sliderSensitivity = 1.0f;
        // Raylib.SetCameraMode(camera, CameraMode.CAMERA_THIRD_PERSON);

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
            building.RuntimeBD(camera, buildingPos);

            Raylib.BeginDrawing();

            ClearBackground(SKYBLUE);

            Raylib.BeginMode3D(camera);

            DrawModel(planeModel, Vector3.Zero, 1.0f, WHITE);
            Raylib.DrawGrid(10, 1.0f);
            // DrawCube(cubePosition, 2.0f, 2.0f, 2.0f, RED);
            // DrawCubeWires(cubePosition, 2.0f, 2.0f, 2.0f, MAROON);

            building.RuntimeAD(camera);
            Raylib.EndMode3D();

            DrawFPS(GetScreenWidth() - 80, 12);

            Raylib.DrawText(
                "Camera position: " + camera.position,
                12,
                GetScreenHeight() - 30,
                20,
                BLACK
            );
            // Create a RayGUI window
            GuiGroupBox(vector3EditorRect, "Vector3 Editor");
	    GuiPanel(vector3EditorRect, null);
            buildingPos.X = GuiSliderBar(
                new Rectangle(vector3EditorRect.x + 90, vector3EditorRect.y + 30, 120, 20),
                "X",
                null,
                buildingPos.X,
                -1000.0f,
                1000.0f
            );
            buildingPos.Y = GuiSliderBar(
                new Rectangle(vector3EditorRect.x + 90, vector3EditorRect.y + 60, 120, 20),
                "Y",
                null,
                buildingPos.Y,
                -1000.0f,
                1000.0f
            );
            buildingPos.Z = GuiSliderBar(
                new Rectangle(vector3EditorRect.x + 90, vector3EditorRect.y + 90, 120, 20),
                "Z",
                null,
                buildingPos.Z,
                -1000.0f,
                1000.0f
            );
            DrawText(
                $"Vector3: ({buildingPos.X}, {buildingPos.Y}, {buildingPos.Z})",
                20,
                200,
                20,
                DARKGRAY
            );

            // Create a Vector3 editor using RayGUI

            Raylib.EndDrawing();
        }
        Raylib.UnloadModel(planeModel);
        Raylib.CloseWindow();
    }
}
