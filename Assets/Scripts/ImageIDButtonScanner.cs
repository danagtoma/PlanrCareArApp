using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageIDButtonScanner : MonoBehaviour
{
    // --- IF THESE LINES ARE MISSING, YOU WON'T SEE SLOTS ---
    [Header("Connections")]
    public ARTrackedImageManager imageManager;
    public PlantDatabase database;
    public GameObject plantInfoPrefab;
    // -------------------------------------------------------

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

        // Find Data
        var data = database.GetPlantData(id);
        if (data == null) return;

        // Position 10cm ABOVE the image (Fixes button clicks)
        Vector3 spawnPos = imageReference.transform.position + (imageReference.transform.up * 0.1f);

        currentPanel = Instantiate(plantInfoPrefab, spawnPos, imageReference.transform.rotation);

        // Face Camera
        currentPanel.transform.LookAt(Camera.main.transform);

        // Fill Text
        PlantPanelController controller = currentPanel.GetComponent<PlantPanelController>();
        if (controller != null)
        {
            controller.Setup(data);
        }
    }
}