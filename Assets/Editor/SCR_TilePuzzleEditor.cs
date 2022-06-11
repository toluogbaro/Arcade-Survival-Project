// using UnityEditor;
//  using UnityEngine;
 
//  [CustomEditor(typeof(SCR_TilePuzzle)), CanEditMultipleObjects]
//  public class SCR_TilePuzzleEditor : Editor {
     
//     #region PuzzleSettingsFields
//     public SerializedProperty 
//         puzzleType_Prop,
//         sequenceMode_Prop,
//         comparisonMode_Prop,
//         debugBaseColor_Prop,
//         debugActiveColor_Prop,
//         debugFailedColor_Prop,
//         debugSolvedColor_Prop,
//         failThreshhold_Prop,
//         comparisonSolutionTrans_Prop,
//         puzzleSolution_Prop, //Can remove once completed
//         puzzleAttempt_Prop, //Can remove once completed
//         puzzleInterface_Prop;
//     #endregion
//     #region GenerationSettingsFields
//     public SerializedProperty 
//         gridZ_Prop,
//         gridX_Prop,
//         gridPadding_Prop,
//         tileSizeRef_Prop,
//         tilePrefab_Prop,
//         puzzleAnchorTrans_Prop,
//         tiles_Prop; //Can remove once completed
//     #endregion
     
//     void OnEnable () {
//         // Setup the SerializedProperties
//         puzzleType_Prop = serializedObject.FindProperty ("puzzleType");
//         sequenceMode_Prop = serializedObject.FindProperty("sequenceMode");
//         comparisonMode_Prop = serializedObject.FindProperty("comparisonMode");

//         debugBaseColor_Prop = serializedObject.FindProperty("debugBaseColor");
//         debugActiveColor_Prop = serializedObject.FindProperty("debugActiveColor");
//         debugFailedColor_Prop = serializedObject.FindProperty("debugFailedColor");
//         debugSolvedColor_Prop = serializedObject.FindProperty("debugSolvedColor");
//         failThreshhold_Prop = serializedObject.FindProperty("failThreshhold");
//         puzzleSolution_Prop = serializedObject.FindProperty("puzzleSolution");
//         puzzleAttempt_Prop = serializedObject.FindProperty("puzzleAttempt");
//         comparisonSolutionTrans_Prop = serializedObject.FindProperty("comparisonSolutionTrans");

//         gridZ_Prop = serializedObject.FindProperty("gridZ");
//         gridX_Prop = serializedObject.FindProperty("gridX");
//         gridPadding_Prop = serializedObject.FindProperty("gridPadding");
//         tileSizeRef_Prop = serializedObject.FindProperty("tileSizeRef");
//         tilePrefab_Prop = serializedObject.FindProperty("tilePrefab");
//         puzzleAnchorTrans_Prop = serializedObject.FindProperty("puzzleAnchorTrans");
//         tiles_Prop = serializedObject.FindProperty("tiles");
//         puzzleInterface_Prop = serializedObject.FindProperty("puzzleInterfaces");

//     }

//     public override void OnInspectorGUI() 
//     {
//     serializedObject.Update ();
//     //PUZZLE SETTINGS
//     EditorGUILayout.LabelField("Puzzle Settings", EditorStyles.boldLabel);
    
//     EditorGUILayout.PropertyField( puzzleType_Prop );
    
//     SCR_TilePuzzle.PuzzleType st = (SCR_TilePuzzle.PuzzleType)puzzleType_Prop.enumValueIndex;
    
//     //Puzzle type selection
//     switch( st ) {
//     case SCR_TilePuzzle.PuzzleType.SEQUENCE:
//         EditorGUILayout.PropertyField( sequenceMode_Prop, new GUIContent("Puzzle Mode") );
//         EditorGUILayout.PropertyField( failThreshhold_Prop, new GUIContent("Fail Threshold (RESET_AFTER_X)") );
//         break;

//     case SCR_TilePuzzle.PuzzleType.COMPARISON:
//         EditorGUILayout.PropertyField( comparisonMode_Prop, new GUIContent("Puzzle Mode") );
//         EditorGUILayout.PropertyField( comparisonSolutionTrans_Prop, new GUIContent("World-Space Solution Spawn Pos"));
//         break;    
//     }
//     //Other puzzle settings
//     EditorGUILayout.PropertyField( debugBaseColor_Prop, new GUIContent("Debug Base Color") );
//     EditorGUILayout.PropertyField( debugActiveColor_Prop, new GUIContent("Debug Active Color") );
//     EditorGUILayout.PropertyField( debugFailedColor_Prop, new GUIContent("Debug Failed Color") );
//     EditorGUILayout.PropertyField( debugSolvedColor_Prop, new GUIContent("Debug Solved Color") );
//     EditorGUILayout.PropertyField( puzzleSolution_Prop, new GUIContent("PuzzleSolution (Debug)") );
//     EditorGUILayout.PropertyField( puzzleAttempt_Prop, new GUIContent("Puzzle Attempt (Debug)"));
//     EditorGUILayout.PropertyField( puzzleInterface_Prop, new GUIContent("Puzzle Interface"));

//     //GENERATION SETTINGS
//     EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
//     EditorGUILayout.LabelField("Generation Settings", EditorStyles.boldLabel);

//     EditorGUILayout.PropertyField( gridZ_Prop, new GUIContent("Grid Size Z Axis") );
//     EditorGUILayout.PropertyField( gridX_Prop, new GUIContent("Grid Size X Axis") );
//     EditorGUILayout.PropertyField( gridPadding_Prop, new GUIContent("Grid Padding") );
//     EditorGUILayout.PropertyField( tileSizeRef_Prop, new GUIContent("Tile Size Reference") );
//     EditorGUILayout.PropertyField( tilePrefab_Prop, new GUIContent("Tile Prefab") );
//     EditorGUILayout.PropertyField( puzzleAnchorTrans_Prop, new GUIContent("Puzzle Spawn Pos") );
//     EditorGUILayout.PropertyField( tiles_Prop, new GUIContent("Spawned Tiles List (Debug)") );

//     serializedObject.ApplyModifiedProperties ();
//     }
// }
