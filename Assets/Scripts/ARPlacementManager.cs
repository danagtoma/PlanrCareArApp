//using UnityEngine;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;
//using System.Collections.Generic;

//public class ARPlacementManager : MonoBehaviour
//{
//    public GameObject plantInfoPrefab;
//    public ARRaycastManager raycastManager;

//    // Reference to our database (assign in Inspector)
//    public PlantDatabase database;

//    private GameObject spawnedObject;
//    private PlantPanelController panelController;
//    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

//    public void ClearText()
//    {
//        if (spawnedObject != null)
//        {
//            Destroy(spawnedObject);
//            spawnedObject = null;
//        }
//    }

//    public void PlaceLabelAtLocation(Vector2 screenPosition, string detectedName)
//    {
//        // 1. Get Data
//        var data = database.GetPlantData(detectedName);
//        if (data == null)
//        {
//            Debug.LogWarning($"Plant '{detectedName}' not found in database. Creating dummy data.");
//            data = new PlantDatabase.PlantData { displayName = detectedName, waterLevel = 50, nutrientStatus = "Unknown", sunStatus = "Unknown" };
//        }

//        // 2. Spawn Logic
//        if (spawnedObject == null)
//        {
//            Pose hitPose;
//            if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
//            {
//                hitPose = hits[0].pose;
//                Debug.Log("Spawning on AR Plane.");
//            }
//            else
//            {
//                // Spawn 50cm in front of camera
//                Vector3 floatPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0.5f));
//                hitPose = new Pose(floatPos, Quaternion.identity);
//                Debug.Log("Spawning floating in air (No plane found).");
//            }

//            spawnedObject = Instantiate(plantInfoPrefab, hitPose.position, hitPose.rotation);
//        }

//        // 3. Controller Logic
//        // CRITICAL: We look for PlantPanelController, NOT InfoLabelController
//        panelController = spawnedObject.GetComponent<PlantPanelController>();

//        if (panelController != null)
//        {
//            Debug.Log("Controller found! Setting up data...");
//            panelController.Setup(data);

//            // Force the object to look at camera immediately
//            spawnedObject.transform.LookAt(Camera.main.transform);
//        }
//        else
//        {
//            Debug.LogError("ERROR: The spawned prefab does not have the 'PlantPanelController' script attached!");
//        }
//    }
//    // This is called by PlantIDAPI
//    public void UpdateText(string detectedName)
//    {
//        // Re-run the placement logic with the new name to fetch database info
//        if (spawnedObject != null)
//        {
//            var data = database.GetPlantData(detectedName);
//            if (data != null && panelController != null)
//            {
//                panelController.Setup(data);
//            }
//        }
//    }
//}

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlacementManager : MonoBehaviour
{
    public GameObject plantInfoPrefab;
    public ARRaycastManager raycastManager;
    public PlantDatabase database;

    private GameObject spawnedObject;
    private PlantPanelController panelController;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private int currentDemoIndex = -1;

    // Button calls this
    public void CycleToNextPlant()
    {
        if (database == null || database.plantLibrary.Count == 0) return;

        // 1. Next Index
        currentDemoIndex++;
        if (currentDemoIndex >= database.plantLibrary.Count) currentDemoIndex = 0;

        var nextPlant = database.plantLibrary[currentDemoIndex];

        // 2. Use Center of Screen for "Pointing"
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // 3. Call the placement logic
        PlaceLabelAtLocation(screenCenter, nextPlant.id);
    }

    public void PlaceLabelAtLocation(Vector2 screenPosition, string detectedName)
    {
        // A. Get Data
        var data = database.GetPlantData(detectedName);
        if (data == null) return;

        // B. ALWAYS Calculate the new position (Raycast)
        // We do this every time now, so the box moves to where you are looking.
        Pose targetPose;

        if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Hit a real surface
            targetPose = hits[0].pose;
        }
        else
        {
            // No surface found? Float 50cm in front of camera
            Vector3 floatPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0.5f));
            targetPose = new Pose(floatPos, Quaternion.identity);
        }

        // C. Spawn or Move
        if (spawnedObject == null)
        {
            // First time: Spawn it
            spawnedObject = Instantiate(plantInfoPrefab, targetPose.position, targetPose.rotation);
            panelController = spawnedObject.GetComponent<PlantPanelController>();
        }
        else
        {
            // It already exists: MOVE IT to the new spot
            spawnedObject.transform.position = targetPose.position;

            // Reset rotation so it faces the camera properly from the new spot
            spawnedObject.transform.LookAt(Camera.main.transform);
            // Fix the 180 flip if your text is backward
            // spawnedObject.transform.Rotate(0, 180, 0); 
        }

        // D. Update the Text
        if (panelController != null)
        {
            panelController.Setup(data);
        }
    }
}