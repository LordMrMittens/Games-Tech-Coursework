using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public enum activetool { bumper, hazard, door, movingPlatform, coin }
public class JBObjects : EditorWindow
{
    public Vector3 firstPosition;
    public Vector3 secondPosition;
    public float speed = .5f;
    public float range = 1;
    public float timeBetweenActions = 8;
    public float timeAttacking = 1;
    GameObject tempPosOne;
    GameObject tempPosTwo;
    public GameObject tempObject;
    bool explanation;
    activetool tool = activetool.bumper;
    public int coinValue = 5;
    [MenuItem("JB Tools/Key Objects %#o")]

    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(JBObjects));

    }
    private void OnEnable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }
    private void OnGUI()
    {
        SpawnTemporaryObject();
        using (var vertical = new GUILayout.VerticalScope())
        {
            if (!GameObject.FindGameObjectWithTag("EntryDoor"))
            {
                GUILayout.Label("This level is lacking an entry point.");
            }
            if (!GameObject.FindGameObjectWithTag("ExitDoor"))
            {
                GUILayout.Label("This level is lacking an exit point.");
            }
            if (GameObject.FindGameObjectsWithTag("Points").Length == 0)
            {
                GUILayout.Label("This level is lacking point Pickups.");
            }
            else
            {
                int points = 0;
                foreach (var Pickup in FindObjectsOfType<Pickup>())
                {
                    points += Pickup.value;
                }
                GUILayout.Label($"This level currently have {points} points");
            }
        }
        using (var horizontal = new GUILayout.HorizontalScope())
        {
            using (var vertical = new GUILayout.VerticalScope())
            {
                ChooseToolPanel();
            }
            using (var vertical = new GUILayout.VerticalScope())
            {

                GUILayout.Label("Object Creation Settings");
                switch (tool)
                {
                    case activetool.bumper:
                        using (var horizontalScope = new GUILayout.HorizontalScope())
                        {
                            range = Slider("Range: ", range);
                            tempObject.GetComponent<TempObject>().range = range;
                        }
                        if (GUILayout.Button("Create Bumper"))
                        {
                            GameObject bumper = SpawnObject("Assets/Prefabs/Bumper.prefab");
                            bumper.GetComponent<Bumper>().bumpPower = range;
                        }
                        break;

                    case activetool.hazard:
                        using (var horizontalScope = new GUILayout.HorizontalScope())
                        {
                            range = Slider("Range: ", range);
                            tempObject.GetComponent<TempObject>().range = range;
                        }
                        using (var horizontalScope = new GUILayout.HorizontalScope())
                        {
                            timeBetweenActions = Slider("Time between attacks: ", timeBetweenActions);
                        }
                        using (var horizontalScope = new GUILayout.HorizontalScope())
                        {
                            timeAttacking = Slider("Time attacking ", timeAttacking);
                        }
                        if (GUILayout.Button("Create Hazard"))
                        {
                            GameObject hazard = SpawnObject("Assets/Prefabs/Hazard.prefab");
                            Hazard hazardControls = hazard.GetComponent<Hazard>();
                            hazardControls.attackRadius = range * 2;
                            hazardControls.timeBetweenAttacks = timeBetweenActions;
                            hazardControls.timeAttacking = timeAttacking;
                        }
                        break;

                    case activetool.door:
                        using (var horizontalScope = new GUILayout.HorizontalScope())
                        {
                            RaycastHit2D hit = Physics2D.Raycast(tempObject.transform.position, Vector2.down);

                            if (GUILayout.Button("Create Exit Door"))
                            {

                                if (hit.collider != null)
                                {
                                    GameObject door = SpawnObject("Assets/Prefabs/ExitDoor.prefab");
                                    door.transform.position = hit.point;
                                }
                                else
                                {
                                    Debug.Log("Place marker over valid surface");
                                }
                            }
                            if (GUILayout.Button("Create Entry Door"))
                            {

                                if (hit.collider != null)
                                {
                                    GameObject door = SpawnObject("Assets/Prefabs/EntryDoor.prefab");
                                    door.transform.position = hit.point;
                                }
                                else
                                {
                                    Debug.Log("Place marker over valid surface");
                                }
                            }
                        }
                        break;

                    case activetool.movingPlatform:
                        if (tempPosOne == null)
                        {
                            tempPosOne = CreateTempMarker("TempMarkerOne", tempPosOne, Color.red);
                            tempPosOne.transform.position = firstPosition;
                        }
                        if (tempPosTwo == null)
                        {
                            tempPosTwo = CreateTempMarker("TempMarkerTwo", tempPosTwo, Color.green);
                            tempPosTwo.transform.position = secondPosition;
                        }
                        speed = EditorGUILayout.FloatField("Speed", speed);
                        firstPosition = tempPosOne.transform.position;
                        secondPosition = tempPosTwo.transform.position;
                        if (GUILayout.Button("Add component selected object"))
                        {
                            if (!Selection.activeGameObject)
                            {
                                Debug.Log("No Object Selected!");
                            }
                            else if (Selection.activeGameObject == tempObject)
                            {
                                Debug.Log("Cannot add moving platform to temporary objects");
                            }
                            else if (!Selection.activeGameObject.GetComponent<MoverOverTime>() && !Selection.activeGameObject.GetComponentInParent<MoverOverTime>())
                            {
                                CreateEmptyGameObjects();
                            }
                            else
                            {
                                TryAssignValues();
                            }
                        }
                        if (GUILayout.Button("Create Moving Object"))
                        {
                            GameObject movingObject = SpawnObject("Assets/Prefabs/MovingObject.prefab");
                            AssignTheValues(movingObject.GetComponent<MoverOverTime>());
                        }
                        break;
                    case activetool.coin:
                        using (var horizontalScope = new GUILayout.HorizontalScope())
                        {
                            coinValue = EditorGUILayout.IntField("Value: ", coinValue);
                        }
                        if (GUILayout.Button("Create Coin"))
                        {
                            GameObject coin = SpawnObject("Assets/Prefabs/Pickup.prefab");
                            coin.GetComponent<Pickup>().value = coinValue;
                        }
                        break;
                }
                GUILayout.FlexibleSpace();
            }
        }

        if (explanation)
        {
            GUILayout.Label("Instructions:");
            GUILayout.TextArea($"Choose which object to create using the buttons on the left of the screen. {Environment.NewLine}" +
                $"Choose where to spawn the object using the temporary object in the editor window.{Environment.NewLine}" +
                $"Different objects have different parameters available to them.{Environment.NewLine}" +
                $"Parameters can also be edited using the handles on the temporary object{Environment.NewLine}" +
                 $"The temporary object has a reference to the highest possible jump distance the player can make. {Environment.NewLine}" +
                $"Press the create button to spawn that object into the scene at the temporary objects position{ Environment.NewLine}{ Environment.NewLine}" +
                $"1 - Bumper only has a range setting{Environment.NewLine}" +
                $"2 - Hazards have settings for time between attacks and duration of the attack{Environment.NewLine}" +
                $"3 - Doors snap to the closest available surface below the temporary object{Environment.NewLine}" +
                $"4 - Moving platform tool has the option to create a new object or to make a selected object into a moving one{Environment.NewLine}" +
                $"4 - Coin Object tool has the option tadjust a coins value{Environment.NewLine}" +
                $"----- Each object has more options in their inspector once spawned -----{Environment.NewLine}" +
                $"A checklist has also been implemented into the top of the window, it displays a warning if some key elements of the level are missing such as doors."
               );
        }
        if (GUILayout.Button("Show Explanation", GUILayout.MinWidth(140), GUILayout.MaxWidth(140), GUILayout.Height(20)))
        {
            if (!explanation)
            {
                explanation = true;
            }
            else
            {
                explanation = false;
            }
        }
    }

    private void SpawnTemporaryObject()
    {
        tempObject = GameObject.Find("tempObject");
        if (!GameObject.Find("tempObject"))
        {
            tempObject = new GameObject();
            tempObject.AddComponent<TempObject>();
            tempObject.name = "tempObject";
            Selection.activeGameObject = tempObject;
        }
    }

    private void ChooseToolPanel()
    {
        if (GUILayout.Button("Bumper tool", GUILayout.MinWidth(150), GUILayout.MaxWidth(150), GUILayout.MinHeight(20), GUILayout.MaxHeight(20)))
        {
            DestroyTemporaryMarkersIfPresent();
            tool = activetool.bumper;
            tempObject.GetComponent<TempObject>().isDrawingRange = true;
        }
        if (GUILayout.Button("Hazard tool", GUILayout.MinWidth(150), GUILayout.MaxWidth(150), GUILayout.MinHeight(20), GUILayout.MaxHeight(20)))
        {
            DestroyTemporaryMarkersIfPresent();
            tool = activetool.hazard;
            tempObject.GetComponent<TempObject>().isDrawingRange = true;
        }
        if (GUILayout.Button("Door tool", GUILayout.MinWidth(150), GUILayout.MaxWidth(150), GUILayout.MinHeight(20), GUILayout.MaxHeight(20)))
        {
            DestroyTemporaryMarkersIfPresent();
            tool = activetool.door;
            tempObject.GetComponent<TempObject>().isDrawingRange = false;
        }
        if (GUILayout.Button("Moving platform tool", GUILayout.MinWidth(150), GUILayout.MaxWidth(150), GUILayout.MinHeight(20), GUILayout.MaxHeight(20)))
        {
            tool = activetool.movingPlatform;
            firstPosition = new Vector3(tempObject.transform.position.x + 1, tempObject.transform.position.y, tempObject.transform.position.z);
            secondPosition = new Vector3(tempObject.transform.position.x - 1, tempObject.transform.position.y, tempObject.transform.position.z);
            tempObject.GetComponent<TempObject>().isDrawingRange = false;
        }
        if (GUILayout.Button("Coin Tool", GUILayout.MinWidth(150), GUILayout.MaxWidth(150), GUILayout.MinHeight(20), GUILayout.MaxHeight(20)))
        {
            DestroyTemporaryMarkersIfPresent();
            tool = activetool.coin;
            tempObject.GetComponent<TempObject>().isDrawingRange = false;
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (tempObject != null)
        {
            if (tool == activetool.bumper || tool == activetool.hazard)
            {
                Handles.Label(tempObject.transform.position + Vector3.up * (range + .5f), "Approximate range");
                Handles.color = Color.red;
                EditorGUI.BeginChangeCheck();
                float newBumpPower = (float)Mathf.Clamp(Handles.ScaleValueHandle(range, tempObject.transform.position + tempObject.transform.up * range, Quaternion.identity, 1, Handles.DotHandleCap, 2), 0, 10);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Changed moving platform positions");
                    range = newBumpPower;
                    tempObject.GetComponent<TempObject>().range = range;
                }
            }
        }

    }
    private GameObject SpawnObject(string prefabLocation)
    {
        GameObject prefabToCreate = (GameObject)AssetDatabase.LoadAssetAtPath(prefabLocation, typeof(GameObject));
        GameObject prefab = Instantiate(prefabToCreate);
        GetPosition(prefab);
        prefab.transform.parent = GameObject.Find("Level Container").transform;
        prefab.transform.localScale = prefabToCreate.transform.localScale;
        Undo.RegisterCreatedObjectUndo(prefab, "Created object");
        return prefab;
    }

    private GameObject CreateTempMarker(string name, GameObject markerObject, Color markerColor)
    {
        if (!GameObject.Find(name))
        {
            markerObject = new GameObject();
            markerObject.name = name;
            markerObject.AddComponent<TempMarker>().color = markerColor;
            markerObject.transform.parent = tempObject.transform;

            return markerObject;
        }
        else
        {
            markerObject = GameObject.Find(name);
            return markerObject;
        }
    }

    private float Slider(string label, float variable)
    {
        GUILayout.Label(label + variable.ToString("0#.00"), GUILayout.MinWidth(50), GUILayout.MaxWidth(300), GUILayout.MinHeight(20), GUILayout.MaxHeight(20));
        variable = GUILayout.HorizontalSlider(variable, 0, 10, GUILayout.MinWidth(50), GUILayout.MaxWidth(300), GUILayout.MinHeight(20), GUILayout.MaxHeight(20));
        return variable;
    }

    private void GetPosition(GameObject instance)
    {
        instance.transform.position = tempObject.transform.position;
        instance.transform.rotation = tempObject.transform.rotation;
        instance.transform.localScale = tempObject.transform.localScale;
    }

    private void OnDestroy()
    {
        DestroyImmediate(GameObject.Find("tempObject"));
        DestroyTemporaryMarkersIfPresent();
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    private void DestroyTemporaryMarkersIfPresent()
    {
        if (tempPosOne != null)
        {
            DestroyImmediate(GameObject.Find("TempMarkerOne"));
        }
        if (tempPosTwo != null)
        {
            DestroyImmediate(GameObject.Find("TempMarkerTwo"));
        }
    }

    private void TryAssignValues()
    {
        MoverOverTime mover = Selection.activeGameObject.GetComponent<MoverOverTime>();
        if (mover == null)
        {
            mover = Selection.activeGameObject.GetComponentInParent<MoverOverTime>();
            Debug.Log("please make all adjustments from the Moving Object!");
            AssignTheValues(mover);
        }
        AssignTheValues(mover);
    }
    private void AssignTheValues(MoverOverTime mover)
    {
        if (mover != null)
        {
            mover.positionOne = firstPosition;
            mover.posOne.transform.position = firstPosition;
            mover.positionTwo = secondPosition;
            mover.posTwo.transform.position = secondPosition;
            mover.speed = speed;
            Selection.activeGameObject.transform.position = Vector3.Lerp(firstPosition, secondPosition, 0.5f);
        }
    }
    private void CreateEmptyGameObjects()
    {
        var master = new GameObject();
        master.name = "MovingObject";
        master.AddComponent<MoverOverTime>();
        master.AddComponent<BoxCollider2D>();
        master.transform.position = Selection.activeGameObject.transform.position;
        master.transform.localScale = Selection.activeGameObject.transform.localScale;
        Selection.activeGameObject.transform.parent = master.transform;
        MoverOverTime mover = master.GetComponent<MoverOverTime>();
        mover.movingObject = Selection.activeGameObject;
        mover.positionOne = firstPosition;
        mover.positionTwo = secondPosition;
        mover.speed = speed;
        var po = new GameObject();
        var pt = new GameObject();
        ApplyMoverDestinations(po.transform, master.transform, mover, "Position One");
        ApplyMoverDestinations(pt.transform, master.transform, mover, "Position Two");
        mover.posOne = po;
        mover.posTwo = pt;
        Selection.activeGameObject = master;
    }
    void ApplyMoverDestinations(Transform targetPosition, Transform parentObject, MoverOverTime mover, string name)
    {
        targetPosition.position = mover.positionOne;
        targetPosition.parent = parentObject;
        targetPosition.name = name;
    }
}
