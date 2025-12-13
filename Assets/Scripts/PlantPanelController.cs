using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantPanelController : MonoBehaviour
{
    [Header("Panel Containers")]
    public GameObject mainInfoPanel;
    public GameObject detailsPanel;

    [Header("Main UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI nutrientText;
    public TextMeshProUGUI sunText;

    [Header("Detail Popup Elements")]
    public TextMeshProUGUI detailsTitleText;
    public TextMeshProUGUI detailsBodyText;

    private PlantDatabase.PlantData currentData;

    [Header("Health Diagnosis UI (For Red Box)")]
    // Assign these ONLY in the HealthAlertCanvas prefab
    public TextMeshProUGUI healthStatusText;      // e.g. "Critical"
    public TextMeshProUGUI conditionText;         // e.g. "Iron Deficiency"
    public TextMeshProUGUI recommendationText;    // e.g. "Use Iron..."

    void Start()
    {
        if (mainInfoPanel != null) mainInfoPanel.SetActive(true);
        if (detailsPanel != null) detailsPanel.SetActive(false);
    }

    void Update()
    {
        // Billboard: Always face the camera
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void Setup(PlantDatabase.PlantData data)
    {
        // 1. Set the Title (Used by both)
        if (nameText != null) nameText.text = data.displayName;

        // 2. LOGIC: Check which type of data this is
        if (data.type == "Health")
        {
            // --- FILL HEALTH UI ---
            if (healthStatusText != null)
            {
                healthStatusText.text = data.healthStatus;

                // Color Code: Critical = Red, Medium = Yellow
                if (data.healthStatus.ToLower() == "critical")
                    healthStatusText.color = Color.red;
                else if (data.healthStatus.ToLower() == "medium")
                    healthStatusText.color = Color.yellow;
            }

            if (conditionText != null)
                conditionText.text = "Condition: " + data.condition;

            if (recommendationText != null)
                recommendationText.text = "Steps: " + data.recomandedFurtherSteps;
        }
        else
        {
            // --- FILL NORMAL UI ---
            currentData = data;
            nameText.text = data.displayName;
            waterText.text = $"{data.waterLevel}%";
            nutrientText.text = $"{data.nutrientStatus}";
            sunText.text = $"{data.sunStatus}";
        }
    }

    // --- BUTTON FUNCTIONS ---

    public void OnWaterButtonClicked()
    {
        OpenDetails("Water Details",
            $"Last: {currentData.lastWateredDate}\nNext: {currentData.nextWateringDate}");
    }

    public void OnNutrientButtonClicked()
    {
        OpenDetails("Nutrient Details",
            $"Status: {currentData.nutrientStatus}\nRec: {currentData.fertilizerRecommendation}");
    }

    public void OnSunButtonClicked()
    {
        OpenDetails("Sun Exposure",
            $"Current: {currentData.sunStatus}\nTarget: {currentData.optimalLux}");
    }

    // --- DETAILS PANEL LOGIC ---

    private void OpenDetails(string title, string body)
    {
        detailsTitleText.text = title;
        detailsBodyText.text = body;

        if (mainInfoPanel != null) mainInfoPanel.SetActive(false);
        if (detailsPanel != null) detailsPanel.SetActive(true);
    }

    public void OnCloseDetailsClicked()
    {
        // Go back to Main View
        if (detailsPanel != null) detailsPanel.SetActive(false);
        if (mainInfoPanel != null) mainInfoPanel.SetActive(true);
    }

    // --- NEW: CLOSE THE ENTIRE BOX ---
    public void OnCloseMainPanelClicked()
    {
        // This deletes the entire Floating UI Object from the scene
        Destroy(gameObject);
    }
}