using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JBObjects : EditorWindow
{
    public float range = 1;
    public float timeBetweenActions = 8;
    public float timeAttacking = 1;

    public GameObject tempObject;
    [MenuItem("JB Tools/Key Objects")]


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
        if (!GameObject.FindGameObjectWithTag("EntryDoor"))
        {
            GUILayout.Label("This level is lacking an entry point.");
        }
        if (!GameObject.FindGameObjectWithTag("ExitDoor"))
        {
            GUILayout.Label("This level is lacking an exit point.");
        }
        if (GameObject.FindGameObjectsWithTag("Points").Length ==0)
        {
            GUILayout.Label("This level is lacking point Pickups.");
        }
        GameObject temporarySprite = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Editor/TempObjects/TempObject.prefab", typeof(GameObject));
        GameObject bumperPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bumper.prefab", typeof(GameObject));
        GameObject hazardPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Hazard.prefab", typeof(GameObject));
        GameObject exitDoorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ExitDoor.prefab", typeof(GameObject));
        GameObject entryDoorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/EntryDoor.prefab", typeof(GameObject));
        tempObject = GameObject.Find("tempObject");
        if (!GameObject.Find("tempObject"))
        {
            tempObject = Instantiate(temporarySprite);
            tempObject.name = "tempObject";
            Selection.activeGameObject = tempObject;
        }
        GUILayout.Label("Object Creation Settings");
        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Range: " + range.ToString("0#.00"));
            range = GUILayout.HorizontalSlider(range, 0, 10);
        }
        GUILayout.Label("If hazard choose attack durations");
        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Time between attacks: " + timeBetweenActions.ToString("0#.00"));
            timeBetweenActions = GUILayout.HorizontalSlider(timeBetweenActions, 0, 10);
            
        }
        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Time attacking: " + timeAttacking.ToString("0#.00"));
            timeAttacking = GUILayout.HorizontalSlider(timeAttacking, 0, 10);
            
        }

        if (GUILayout.Button("Create Bumper"))
        {
            GameObject bumper = Instantiate(bumperPrefab);
            GetPosition(bumper);
            bumper.GetComponent<Bumper>().bumpPower = range;
        }
        if (GUILayout.Button("Create Hazard"))
        {
            GameObject hazard = Instantiate(hazardPrefab);
            GetPosition(hazard);
            Hazard hazardControls = hazard.GetComponent<Hazard>();
            hazardControls.attackRadius = range * 2;
            hazardControls.timeBetweenAttacks = timeBetweenActions;
            hazardControls.timeAttacking = timeAttacking;
        }
        if (GUILayout.Button("Create Exit Door"))
        {
            RaycastHit2D hit = Physics2D.Raycast(tempObject.transform.position, Vector2.down);
            if (hit.collider != null)
            {
                GameObject door = Instantiate(exitDoorPrefab, hit.point, Quaternion.identity);
            } else
            {
                Debug.Log("Place marker over valid surface");
            }
        }
        if (GUILayout.Button("Create Entry Door"))
        {
            RaycastHit2D hit = Physics2D.Raycast(tempObject.transform.position, Vector2.down);
            if (hit.collider != null)
            {
                GameObject door = Instantiate(entryDoorPrefab, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("Place marker over valid surface");
            }
        }
        GUILayout.FlexibleSpace();
    }

    private void GetPosition(GameObject temp)
    {
        temp.transform.position = tempObject.transform.position;
        temp.transform.rotation = tempObject.transform.rotation;
        temp.transform.localScale = tempObject.transform.localScale;
    }

    private void OnDestroy()
    {
        DestroyImmediate(GameObject.Find("tempObject"));
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }
    void OnDrawGizmos()
    {
    
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (tempObject != null)
        {
            Handles.Label(tempObject.transform.position + Vector3.up * (range + .5f), "Approximate range");
            Handles.color = Color.red;
            EditorGUI.BeginChangeCheck();
            float newBumpPower = (float)Mathf.Clamp(Handles.ScaleValueHandle(range, tempObject.transform.position + tempObject.transform.up * range, Quaternion.identity, 2, Handles.DotHandleCap, 2), 0, 10);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Changed moving platform positions");
                range = newBumpPower;
            }
        }

    }
}
