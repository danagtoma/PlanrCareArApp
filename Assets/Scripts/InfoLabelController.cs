using UnityEngine;
using TMPro;

public class InfoLabelController : MonoBehaviour
{
    public TextMeshPro textComponent;
    public GameObject backgroundPanel;

    void Start()
    {
        // 1. Check if the link is missing immediately
        if (textComponent == null)
        {
            Debug.LogError("ERROR: The 'Text Component' slot is empty on the PlantLabelContainer prefab! Please drag the text object into the script slot.");

            // Try to find it automatically to save you
            textComponent = GetComponentInChildren<TextMeshPro>();
        }
    }

    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }

    public void SetText(string newText)
    {
        // 2. Log what we are trying to write
        Debug.Log("InfoLabelController: Trying to write text -> " + newText);

        if (textComponent != null)
        {
            textComponent.text = newText;
        }
        else
        {
            Debug.LogError("InfoLabelController: Cannot write text because textComponent is NULL.");
        }
    }
}