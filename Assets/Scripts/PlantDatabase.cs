using UnityEngine;
using System.Collections.Generic;

public class PlantDatabase : MonoBehaviour
{
    // Singleton pattern so we can access this easily from anywhere
    public static PlantDatabase Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Initialize list if empty
        if (plantLibrary == null) plantLibrary = new List<PlantData>();
        plantLibrary.Clear(); // Clear existing to avoid duplicates when restarting

        // --- Plant 1: Monstera (Healthy) ---
        plantLibrary.Add(new PlantData
        {
            id = "monstera",
            displayName = "Monstera Deliciosa",
            waterLevel = 75,
            lastWateredDate = "3 Days Ago",
            nextWateringDate = "In 4 Days",
            nutrientStatus = "Healthy",
            fertilizerRecommendation = "None needed",
            sunStatus = "Perfect",
            optimalLux = "Indirect Bright"
        });

        // --- Plant 2: Snake Plant (Needs Water) ---
        plantLibrary.Add(new PlantData
        {
            id = "snake_plant",
            displayName = "Snake Plant",
            waterLevel = 10,
            lastWateredDate = "2 Weeks Ago",
            nextWateringDate = "Today!",
            nutrientStatus = "Good",
            fertilizerRecommendation = "Spring only",
            sunStatus = "Good",
            optimalLux = "Low to Bright"
        });

        // --- Plant 3: Fiddle Leaf Fig (Needs Sun) ---
        plantLibrary.Add(new PlantData
        {
            id = "fiddleleaf_fig",
            displayName = "Fiddle Leaf Fig",
            waterLevel = 50,
            lastWateredDate = "Yesterday",
            nextWateringDate = "Next Week",
            nutrientStatus = "Yellowing Leaves",
            fertilizerRecommendation = "Add Nitrogen",
            sunStatus = "Too Dark",
            optimalLux = "Needs Window Spot"
        });

        // --- Plant 4: Pothos ---
        plantLibrary.Add(new PlantData
        {
            id = "pothos",
            displayName = "Pothos",
            waterLevel = 30,
            lastWateredDate = "1 Month Ago",
            nextWateringDate = "When soil is dry",
            nutrientStatus = "Low Potassium",
            fertilizerRecommendation = "Succulent Food",
            sunStatus = "Perfect",
            optimalLux = "Direct Sun"
        });

        // --- Plant 5: Orchids ---
        plantLibrary.Add(new PlantData
        {
            id = "orchids",
            displayName = "Orchids",
            waterLevel = 90,
            lastWateredDate = "This Morning",
            nextWateringDate = "Tomorrow",
            nutrientStatus = "Healthy",
            fertilizerRecommendation = "None",
            sunStatus = "Perfect",
            optimalLux = "Full Sun"
        });

        // --- Plant 6: Hydrangeas ---
        plantLibrary.Add(new PlantData
        {
            id = "hydrangeas",
            displayName = "Hydrangeas",
            waterLevel = 90,
            lastWateredDate = "This Morning",
            nextWateringDate = "Tomorrow",
            nutrientStatus = "Healthy",
            fertilizerRecommendation = "None",
            sunStatus = "Perfect",
            optimalLux = "Full Sun"
        });

        // --- Plant: Monstera dry ---
        plantLibrary.Add(new PlantData
        {
            id = "leaf_bad",              
            displayName = "Monestera - Dry leaves",

            // DRY SETTINGS
            waterLevel = 0,             
            lastWateredDate = "35 Days Ago",
            nextWateringDate = "RIGHT NOW!",

            nutrientStatus = "Stable",
            fertilizerRecommendation = "None needed yet",

            sunStatus = "Perfect",
            optimalLux = "Direct Sunlight"
        });
    }

    // 1. Define the Data Structure
    [System.Serializable]
    public class PlantData
    {
        public string id; // e.g. "wheat" or "monsterra"
        public string displayName;

        [Header("Water Data")]
        public int waterLevel; // 0 to 100
        public string lastWateredDate;
        public string nextWateringDate;

        [Header("Nutrient Data")]
        public string nutrientStatus; // "Healthy", "Low Nitrogen"
        public string fertilizerRecommendation;

        [Header("Sun Data")]
        public string sunStatus; // "Too Dry", "Perfect"
        public string optimalLux;
    }

    // 2. The Array of Predefined Objects
    public List<PlantData> plantLibrary;

    // 3. Helper to find a plant by name
    public PlantData GetPlantData(string nameKey)
    {
        // Simple search (case insensitive)
        return plantLibrary.Find(p => nameKey.ToLower().Contains(p.id.ToLower()));
    }
}