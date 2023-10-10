using System.Numerics;
using Raylib_CsLo;
// using raylibExtras;

using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;

namespace FormalEngine;

public class BuildingEditor
{
		private Image img;
		private Texture tx;
		private Mesh planeMesh;
		private Model planeModel;
		private Model model;
		private SelectableObject building;
		private Vector3 buildingPos;
		private UI vectorEditor;
		int screenWidth = 1280;
		int screenHeight = 720;
		Mesh buildingMesh;
		Model buildingModel;

		public unsafe void Initialize()
		{
				img = GenImageChecked(256, 256, 64, 64, LIGHTGRAY, WHITE);
				tx = LoadTextureFromImage(img);
				planeMesh = GenMeshPlane(10.0f, 10.0f, 3, 3);
				planeModel = LoadModelFromMesh(planeMesh);
				model = Raylib.LoadModel("resources/models/gltf/pillbox.glb");
				// Texture texture = LoadTexture("resources/models/gltf/pillbox/textures/concrete_dirty2.jpeg");
				// model.materials[0].maps[0].texture = texture;
				// SetMaterialTexture(model.materials, MATERIAL_MAP_DIFFUSE, texture);
				SetMaterialTexture(planeModel.materials, MATERIAL_MAP_DIFFUSE, tx);
				building = new SelectableObject();
				buildingPos = new Vector3(0.0f, 0.0f, 0.0f);
				building.Initialize("Barracks", buildingPos, model);
				vectorEditor = new UI();
				vectorEditor.Initialize();
				// Raylib.SetCameraMode(camera, CameraMode.CAMERA_THIRD_PERSON);
		}

		public unsafe void Loop(Camera3D camera)
		{
				screenWidth = Raylib.GetScreenWidth();
				screenHeight = Raylib.GetScreenHeight();
				building.RuntimeBD(camera, buildingPos);

				// Raylib.BeginDrawing();

				ClearBackground(SKYBLUE);

				Raylib.BeginMode3D(camera);

				DrawModel(planeModel, Vector3.Zero, 1.0f, WHITE);
				Raylib.DrawGrid(10, 1.0f);
				// DrawCube(cubePosition, 2.0f, 2.0f, 2.0f, RED);
				// DrawCubeWires(cubePosition, 2.0f, 2.0f, 2.0f, MAROON);

				building.RuntimeAD(camera);
				Raylib.EndMode3D();

				DrawFPS(GetScreenWidth() - 80, 3);

				Raylib.DrawText(
					"Camera position: " + camera.position,
					12,
					GetScreenHeight() - 30,
					20,
					BLACK
				);
				vectorEditor.XYZEditor();
				buildingPos = vectorEditor.pos;
				// Create a Vector3 editor using RayGUI

				// Raylib.EndDrawing();
		}

		public unsafe void Unload()
		{
				Raylib.UnloadModel(planeModel);
		}
}
