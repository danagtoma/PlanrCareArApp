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
        currentData = data;
        nameText.text = data.displayName;
        waterText.text = $"{data.waterLevel}%";
        nutrientText.text = $"{data.nutrientStatus}";
        sunText.text = $"{data.sunStatus}";
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