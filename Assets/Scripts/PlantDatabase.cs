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
            type = "Normal",
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
            type = "Normal",
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
            type = "Normal",
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
            type = "Normal",
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
            type = "Normal",
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
            type = "Normal",
            displayName = "Hydrangeas",
            waterLevel = 90,
            lastWateredDate = "This Morning",
            nextWateringDate = "Tomorrow",
            nutrientStatus = "Healthy",
            fertilizerRecommendation = "None",
            sunStatus = "Perfect",
            optimalLux = "Full Sun"
        });

        // Leaf
        plantLibrary.Add(new PlantData
        {
            id = "f_f_bad_leaf",
            type = "Health",
            displayName = "Fiddleleaf Fig Leaves", // The Plant Name

            // YOUR NEW DATA
            healthStatus = "Critical",
            condition = "Iron Deficiency (Chlorosis)",
            recomandedFurtherSteps = "Apply Chelated Iron to soil immediately. Avoid overwatering."
        });

        // 1. POWDERY MILDEW (Fungal Infection)
        plantLibrary.Add(new PlantData
        {
            id = "s_p_bad_leaf",
            type = "Health",
            displayName = "Spider Plant Leaves",

            healthStatus = "Medium",
            condition = "Powdery Mildew (Fungus)",
            recomandedFurtherSteps = "Remove infected leaves. Spray with neem oil or baking soda mixture. Increase air circulation."
        });

        // 2. SPIDER MITES (Pest Infestation)
        plantLibrary.Add(new PlantData
        {
            id = "p_bad_leaf",
            type = "Health",
            displayName = "Pothos Leaves",

            healthStatus = "Critical",
            condition = "Spider Mite Infestation",
            recomandedFurtherSteps = "Isolate plant immediately! Wipe leaves with alcohol. Spray with miticide or insecticidal soap every 3 days."
        });

        // 3. ROOT ROT (Overwatering)
        plantLibrary.Add(new PlantData
        {
            id = "o_bad_leaf",
            type = "Health",
            displayName = "Orchids Leaves",

            healthStatus = "Critical",
            condition = "Root Rot (Overwatering)",
            recomandedFurtherSteps = "Stop watering. Repot into dry, fresh soil immediately. Trim mushy black roots. Ensure pot has drainage holes."
        });

        // 4. SUN SCORCH (Sunburn)
        plantLibrary.Add(new PlantData
        {
            id = "m_bad_leaf",
            type = "Health",
            displayName = "Monstera Leaves",

            healthStatus = "Medium",
            condition = "Sun Scorch (Bleaching)",
            recomandedFurtherSteps = "Move plant away from direct window light. The burnt spots will not heal, but new growth will be healthy."
        });

        // 5. NITROGEN DEFICIENCY (General Yellowing)
        plantLibrary.Add(new PlantData
        {
            id = "h_bad_leaf",
            type = "Health",
            displayName = "Hydrangeas Leaves",

            healthStatus = "Medium",
            condition = "Nitrogen Deficiency",
            recomandedFurtherSteps = "Apply a high-nitrogen liquid fertilizer (NPK 3-1-2). Check soil pH to ensure nutrient absorption."
        });
    }

    // 1. Define the Data Structure
    [System.Serializable]
    public class PlantData
    {
        public string id; // e.g. "wheat" or "monsterra"
        public string displayName;
        public string type; //Normal or health

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

        public string healthStatus; // "Critical", "Medium", "Good"
        public string condition;    // "Iron Deficiency", "Fungus"
        public string recomandedFurtherSteps; // "Use Chelated Iron"
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