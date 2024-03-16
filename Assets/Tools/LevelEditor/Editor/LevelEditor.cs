using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GetMikyled.LevelEditor
{
    public enum BuildMode
    {
        None,
        CreateWalls,
        PrefabPlacer,
        Eraser
    }

    public partial class LevelEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _tree;

        private BuildMode buildMode = BuildMode.None;
        protected Label currentModeText;

        private Transform mainToolObject = null;

        protected float gridSize = 1f;
        protected bool doGridSnapping = true;

        #region CreateGUI
        ///-////////////////////////////////////////////////
        ///
        /// CREATE THE WINDOW
        /// CLONE ELEMENTS
        /// 

        ///-////////////////////////////////////////////////
        ///
        [MenuItem("Tools/LevelEditor")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelEditor>();
        }

        ///-////////////////////////////////////////////////
        ///
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneCreateWalls;
            SceneView.duringSceneGui += OnScenePrefabPlacer;
            SceneView.duringSceneGui += OnSceneEraser;

            CreateLevel();

            mainToolObject = GameObject.Find("------- RoomConstructor -------")?.transform;
            if (mainToolObject == null)
            {
                mainToolObject = new GameObject().transform;
                mainToolObject.name = "------- RoomConstructor -------";
            }
        }

        ///-////////////////////////////////////////////////
        ///
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneCreateWalls;
            SceneView.duringSceneGui -= OnScenePrefabPlacer;
            SceneView.duringSceneGui -= OnSceneEraser;

            ResetTools();
            DestroyImmediate(mainToolObject.gameObject);
        }

        ///-////////////////////////////////////////////////
        ///
        private void CreateGUI()
        {
            VisualElement root = this.rootVisualElement;

            _tree.CloneTree(rootVisualElement);

            currentModeText = root.Q<Label>("CurrentMode");
            SetBuildMode(BuildMode.None);

            Button clearModeButton = root.Q<Button>(name: "ClearModeButton");
            clearModeButton.clicked += () => { SetBuildMode(BuildMode.None); };

            ConstructGridUI(root);
            ConstructCreateWallsUI(root);
            ConstructPrefabPlacerUI(root);
            ConstructEraserUI();

            CreateWallObjectPool();
        }
        #endregion

        #region Set Current Mode
        ///-////////////////////////////////////////////////
        ///
        private void SetBuildMode(BuildMode newMode)
        {
            ResetTools();
            buildMode = newMode;
            // MAKE IT UPDATE TO NAME OF ENUM
            currentModeText.text = "Current Mode: " + System.Enum.GetName(typeof(BuildMode), buildMode);
        }
        #endregion // Set Current Mode

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ConstructGridUI(VisualElement root)
        {
            Toggle gridSnappingToggle = root.Q<Toggle>(name: "GridSnapToggle");
            gridSnappingToggle.RegisterValueChangedCallback(ChangeGridProperties);
            FloatField gridSizeField = root.Q<FloatField>(name: "GridSizeField");
            gridSizeField.RegisterValueChangedCallback(ChangeGridProperties);
        }

        #region Change Properties
        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ChangeGridProperties(ChangeEvent<float> evt)
        {
            gridSize = evt.newValue;
        }

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ChangeGridProperties(ChangeEvent<bool> evt)
        {
            doGridSnapping = evt.newValue;
        }
        #endregion #region Change Properties

        ///-//////////////////////////////////////////////////////////////////
        ///
        private void ResetTools()
        {
            selectedPrefab = null;
            DestroyImmediate(previewPrefab);
            previewPrefab = null;
        }
    }
}
