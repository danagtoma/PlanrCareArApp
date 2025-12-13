using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageIDButtonScanner : MonoBehaviour
{
    [Header("Connections")]
    public ARTrackedImageManager imageManager;
    public PlantDatabase database;

    [Header("Prefabs")]
    public GameObject plantInfoPrefab;   
    public GameObject healthAlertPrefab; 

    private GameObject currentPanel;

    public void OnDetectButtonPressed()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
            currentPanel = null;
        }

        ARTrackedImage targetImage = GetCurrentTrackedImage();

        if (targetImage != null)
        {
            SpawnUI(targetImage);
        }
        else
        {
            Debug.Log("Button pressed, but no valid image found.");
        }
    }

    private ARTrackedImage GetCurrentTrackedImage()
    {
        foreach (var trackable in imageManager.trackables)
        {
            if (trackable.trackingState == TrackingState.Tracking)
            {
                return trackable;
            }
        }
        return null;
    }

    private void SpawnUI(ARTrackedImage imageReference)
    {
        string id = imageReference.referenceImage.name;
        var data = database.GetPlantData(id);

        if (data == null) return;

        // --- ADD THESE DEBUG LINES ---
        Debug.Log("Found ID: " + data.id);
        Debug.Log("Type from Database: '" + data.type + "'");
        // -----------------------------

        // --- SELECT WHICH PREFAB TO USE ---
        GameObject prefabToSpawn;

        if (data.type == "Health")
        {
            prefabToSpawn = healthAlertPrefab; // Use RED box for diseases
        }
        else
        {
            prefabToSpawn = plantInfoPrefab;   // Use BLUE box for plants
        }
        // ----------------------------------

        Vector3 spawnPos = imageReference.transform.position + (imageReference.transform.up * 0.1f);

        // Instantiate the selected prefab
        currentPanel = Instantiate(prefabToSpawn, spawnPos, imageReference.transform.rotation);

        currentPanel.transform.LookAt(Camera.main.transform);

        PlantPanelController controller = currentPanel.GetComponent<PlantPanelController>();
        if (controller != null)
        {
            controller.Setup(data);
        }
    }
}