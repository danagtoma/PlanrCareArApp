using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageTrackingHandler : MonoBehaviour
{
    [Header("Connections")]
    public ARTrackedImageManager imageManager;
    public PlantDatabase database;
    public GameObject plantInfoPrefab;

    private Dictionary<string, GameObject> activePanels = new Dictionary<string, GameObject>();

    // --- CONFIGURATION ---
    // How high above the image should the UI float? (0.1 = 10cm)
    private float verticalOffset = 0.1f;

    void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnImageChanged);
    }

    void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnImageChanged);
    }

    void OnImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // 1. NEW IMAGE FOUND
        foreach (var newImage in eventArgs.added)
        {
            SpawnPanel(newImage);
        }

        // 2. IMAGE MOVED (The part that was breaking your buttons)
        foreach (var updatedImage in eventArgs.updated)
        {
            string name = updatedImage.referenceImage.name;
            if (activePanels.ContainsKey(name))
            {
                GameObject panel = activePanels[name];

                // If lost, hide it
                if (updatedImage.trackingState == TrackingState.None)
                {
                    panel.SetActive(false);
                }
                else
                {
                    panel.SetActive(true);

                    // --- FIX START: Apply Offset Every Frame ---
                    // Instead of snapping exactly to the image, we snap to Image + Up * Offset
                    Vector3 targetPosition = updatedImage.transform.position + (updatedImage.transform.up * verticalOffset);

                    panel.transform.position = targetPosition;
                    panel.transform.rotation = updatedImage.transform.rotation;

                    // Make it face the camera (Recommended for UI buttons)
                    panel.transform.LookAt(Camera.main.transform);
                    // --- FIX END ---
                }
            }
        }
    }

    void SpawnPanel(ARTrackedImage imageTarget)
    {
        string id = imageTarget.referenceImage.name;
        var data = database.GetPlantData(id);

        if (data != null)
        {
            // Calculate initial position with offset
            Vector3 spawnPos = imageTarget.transform.position + (imageTarget.transform.up * verticalOffset);

            GameObject newPanel = Instantiate(plantInfoPrefab, spawnPos, imageTarget.transform.rotation);

            PlantPanelController controller = newPanel.GetComponent<PlantPanelController>();
            if (controller != null)
            {
                controller.Setup(data);
            }

            activePanels.Add(id, newPanel);
        }
    }
}