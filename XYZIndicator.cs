using System.Numerics;
using Raylib_CsLo;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;

namespace FormalEngine;

public class XYZIndicator
{
    // private Vector3 position;
    private float size;

    public XYZIndicator( /* Vector3 position,  */
        float size = 1.0f
    )
    {
        // this.position = position;
        this.size = size;
    }

    public void Draw( /* Camera3D camera */
    )
    {
        // Raylib.BeginMode3D(camera);
        //
        //    Raylib.DrawGrid(10, 1.0f); // Draw a grid in the scene

        // Draw X, Y, Z axes
        Raylib.DrawLine3D(Vector3.Zero, new Vector3(10, 0, 0), RED);
        Raylib.DrawLine3D(Vector3.Zero, new Vector3(0, 10, 0), GREEN);
        Raylib.DrawLine3D(Vector3.Zero, new Vector3(0, 0, 10), BLUE);

        // Raylib.EndMode3D();
    }
}
