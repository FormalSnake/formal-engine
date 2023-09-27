using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Examples.Rlights;
using Examples;
using ImGuiDemo;
using ImGuiNET;

namespace HelloWorld;

class Program
{
    public static unsafe void Main()
    {
        const int screenWidth = 1280;
        const int screenHeight = 720;
        Raylib.SetTraceLogCallback(&Logging.LogConsole);
        Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
        Raylib.InitWindow(screenWidth, screenHeight, "Hello World");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.InitAudioDevice();

ImguiController controller = new ImguiController();
            TestScreen testScreen = new TestScreen();

            controller.Load(screenWidth, screenHeight);
            // testScreen.Load();

        Camera3D camera = new Camera3D();
        camera.position = new Vector3(5.0f, 5.0f, 5.0f); // Camera position
        camera.target = new Vector3(0.0f, 2.0f, 0.0f); // Camera looking at point
        camera.up = new Vector3(0.0f, 1.0f, 0.0f); // Camera up vector (rotation towards target)
        camera.fovy = 45.0f; // Camera field-of-view Y
        camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

        Mesh planeMesh = Raylib.GenMeshPlane(10.0f, 10.0f, 3, 3);
        Model planeModel = Raylib.LoadModelFromMesh(planeMesh);
        Model model = Raylib.LoadModel("resources/models/gltf/robot.glb");

        Shader shader = LoadShader(
            "resources/shaders/glsl330/lighting.vs",
            "resources/shaders/glsl330/lighting.fs"
        );

        shader.locs[(int)ShaderLocationIndex.SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(
            shader,
            "viewPos"
        );

        int ambientLoc = GetShaderLocation(shader, "ambient");
        float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
        Raylib.SetShaderValue(
            shader,
            ambientLoc,
            ambient,
            ShaderUniformDataType.SHADER_UNIFORM_VEC4
        );
        for (int i = 0; i < model.materialCount; i++)
        {
            // Set the shader for each material
            model.materials[i].shader = shader;
        }
        // model.materials[0].shader = shader;
        planeModel.materials[0].shader = shader;

        Light[] lights = new Light[4];
        lights[0] = CreateLight(
            0,
            Examples.LightType.Point,
            new Vector3(-2, 1, -2),
            Vector3.Zero,
            Color.YELLOW,
            shader
        );
        lights[1] = CreateLight(
            1,
            Examples.LightType.Point,
            new Vector3(2, 1, 2),
            Vector3.Zero,
            Color.RED,
            shader
        );
        lights[2] = CreateLight(
            2,
            Examples.LightType.Point,
            new Vector3(-2, 1, 2),
            Vector3.Zero,
            Color.GREEN,
            shader
        );
        lights[3] = CreateLight(
            3,
            Examples.LightType.Point,
            new Vector3(2, 1, -2),
            Vector3.Zero,
            Color.BLUE,
            shader
        );

        uint animsCount = 0;
        uint animIndex = 0;
        uint animCurrentFrame = 0;

        ReadOnlySpan<ModelAnimation> modelAnimationsSpan = Raylib.LoadModelAnimations(
            "resources/models/gltf/robot.glb",
            ref animsCount
        );
        ModelAnimation[] modelAnimations = modelAnimationsSpan.ToArray();

        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        Raylib.DisableCursor();
        Raylib.SetTargetFPS(75);

        while (!Raylib.WindowShouldClose())
        {
float dt = Raylib.GetFrameTime();

                // Feed the input events to our ImGui controller, which passes them through to ImGui.
                controller.Update(dt);
            Raylib.UpdateCamera(ref camera, CameraMode.CAMERA_THIRD_PERSON);

            if (IsKeyPressed(KeyboardKey.KEY_Y))
            {
                animCurrentFrame = 0;
                animIndex++;
                if (animIndex >= animsCount) // Check if animIndex has reached or exceeded the maximum index
                {
                    animIndex = 0; // Reset animIndex to zero
                }
            }

            UpdateLightValues(shader, lights[0]);
            UpdateLightValues(shader, lights[1]);
            UpdateLightValues(shader, lights[2]);
            UpdateLightValues(shader, lights[3]);

            // Update the light shader with the camera view position
            Raylib.SetShaderValue(
                shader,
                shader.locs[(int)ShaderLocationIndex.SHADER_LOC_VECTOR_VIEW],
                camera.position,
                ShaderUniformDataType.SHADER_UNIFORM_VEC3
            );

            ModelAnimation anim = modelAnimations[(int)animIndex]; // Explicitly cast animIndex to int
            animCurrentFrame = (uint)((animCurrentFrame + 1) % anim.frameCount); // Explicitly cast to uint
            Raylib.UpdateModelAnimation(model, anim, (int)animCurrentFrame); // Explicitly cast animCurrentFrame to int

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.RAYWHITE);

            Raylib.BeginMode3D(camera);

            Raylib.DrawModel(model, position, 1f, Color.WHITE);
            DrawModel(planeModel, Vector3.Zero, 1.0f, Color.WHITE);

            Raylib.DrawGrid(10, 1.0f);

            Raylib.EndMode3D();

            DrawFPS(10, 10);

            // Raylib.DrawText("Kakske", 12, 12, 20, Color.BLACK);

            Raylib.EndDrawing();
        }
	            controller.Dispose();
        Raylib.UnloadModel(model);
        Raylib.UnloadModel(planeModel);

        Raylib.CloseWindow();
    }
}
