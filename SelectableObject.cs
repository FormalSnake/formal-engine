using System.Numerics;
using Raylib_CsLo;
// using raylibExtras;

using static Raylib_CsLo.Raylib;

namespace FormalEngine;

class SelectableObject
{
    private static Vector3 cubePosition;
    private static Vector2 cubeScreenPosition;
    private static Vector3 cubeSize;
    private static Ray ray;
    private static RayCollision collision;
    private static string buildingName;

    public unsafe void Initialize(string name, Vector3 position)
    {
        // Initialize the variables here
        cubePosition = position;
        cubeScreenPosition = new Vector2(0.0f, 0.0f);
        cubeSize = new Vector3(2.0f, 2.0f, 2.0f);

        ray = new Ray(); // Picking line ray
        collision = new RayCollision();
        buildingName = name;
    }

    public unsafe void RuntimeBD(Camera3D camera, Vector3 newPos, Model model)
    {
        cubePosition = newPos;
        if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            if (!collision.hit)
            {
                ray = GetMouseRay(GetMousePosition(), camera);

                // Check collision between ray and box
                collision = GetRayCollisionBox(
                    ray,
		    GetMeshBoundingBox(model.meshes[0])
                    // new BoundingBox(
                    //     new Vector3(
                    //         cubePosition.X - cubeSize.X / 2,
                    //         cubePosition.Y - cubeSize.Y / 2,
                    //         cubePosition.Z - cubeSize.Z / 2
                    //     ),
                    //     new Vector3(
                    //         cubePosition.X + cubeSize.X / 2,
                    //         cubePosition.Y + cubeSize.Y / 2,
                    //         cubePosition.Z + cubeSize.Z / 2
                    //     )
                    // )
                );
            }
            else
            {
                collision.hit = false;
            }
        }

        cubeScreenPosition = GetWorldToScreen(
            new Vector3(cubePosition.X, cubePosition.Y + 5.0f, cubePosition.Z),
            camera
        );
        if (collision.hit)
        {
            DrawText(
                $"{buildingName}: 100 / 100",
                (int)cubeScreenPosition.X - MeasureText($"{buildingName}: 100/100", 20) / 2,
                (int)cubeScreenPosition.Y,
                20,
                BLACK
            );
        }
    }

    public unsafe void RuntimeAD(Camera3D camera, Model model)
    {
	    BoundingBox bounds = 		    GetMeshBoundingBox(model.meshes[0]);
        if (collision.hit)
        {
            // DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, RED);
	    	    DrawModel(model, cubePosition, 0.1f, RED);
            // DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, MAROON);
            //
            // DrawCubeWires(
            //     cubePosition,
            //     cubeSize.X + 0.2f,
            //     cubeSize.Y + 0.2f,
            //     cubeSize.Z + 0.2f,
            //     GREEN
            // );
	    DrawBoundingBox(bounds, GREEN);
        }
        else
        {
            // DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, GRAY);
	    DrawModel(model, cubePosition, 0.1f, WHITE);
            // DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, DARKGRAY);
        }
    }
}
