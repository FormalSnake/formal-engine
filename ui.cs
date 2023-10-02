using Raylib_CsLo;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using System.Numerics;

namespace FormalEngine;

public class UI
{
    private Rectangle vector3EditorRect = new Rectangle(20, 40, 300, 140);
    private float sliderSensitivity = 1.0f;
    public Vector3 pos;

    public void Initialize()
    {
        vector3EditorRect = new Rectangle(10, 83, 300, 140);
        sliderSensitivity = 1.0f;
        pos = new Vector3(0.0f, 1.0f, 0.0f);
    }

    public unsafe void XYZEditor()
    {
        // Create a RayGUI window
        GuiGroupBox(vector3EditorRect, "");
        GuiPanel(vector3EditorRect, null);
        pos.X = GuiSliderBar(
            new Rectangle(vector3EditorRect.x + 90, vector3EditorRect.y + 30, 120, 20),
            "X",
            null,
            pos.X,
            -100.0f,
            100.0f
        );
        pos.Y = GuiSliderBar(
            new Rectangle(vector3EditorRect.x + 90, vector3EditorRect.y + 60, 120, 20),
            "Y",
            null,
            pos.Y,
            -100.0f,
            100.0f
        );
        pos.Z = GuiSliderBar(
            new Rectangle(vector3EditorRect.x + 90, vector3EditorRect.y + 90, 120, 20),
            "Z",
            null,
            pos.Z,
            -100.0f,
            100.0f
        );
        DrawText($"Vector3: ({pos.X}, {pos.Y}, {pos.Z})", 10, 233, 20, DARKGRAY);
    }
}
