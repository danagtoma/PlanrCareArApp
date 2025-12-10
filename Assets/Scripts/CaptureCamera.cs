//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Barracuda;
//using System;

//public class CaptureCamera : MonoBehaviour
//{
//    // Make sure to assign these in the Inspector
//    public NNModel modelAsset;
//    public ARPlacementManager placementManager;

//    private Model runtimeModel;
//    private IWorker worker;

//    private const int INPUT_SIZE = 256;
//    private const float CONFIDENCE_THRESHOLD = 0.25f;

//    void Start()
//    {
//        if (modelAsset == null)
//        {
//            Debug.LogError("Please assign the ONNX model in the Inspector!");
//            return;
//        }
//        runtimeModel = ModelLoader.Load(modelAsset);
//        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
//    }

//    public void OnCaptureButtonPressed()
//    {
//        // 1. Clear any existing text from previous scans
//        if (placementManager != null)
//        {
//            placementManager.ClearText();
//        }

//        // 2. Start the new scan
//        StartCoroutine(CaptureAndInfer());
//    }

//    public IEnumerator CaptureAndInfer()
//    {
//        yield return new WaitForEndOfFrame();

//        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
//        screenTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
//        screenTex.Apply();

//        Texture2D processedTex = Letterbox(screenTex, INPUT_SIZE);

//        RunInference(processedTex, screenTex.width, screenTex.height);

//        Destroy(screenTex);
//        Destroy(processedTex);
//    }

//    void RunInference(Texture2D texture, int originalWidth, int originalHeight)
//    {
//        using (Tensor inputTensor = new Tensor(texture, channels: 3))
//        {
//            worker.Execute(inputTensor);
//            Tensor outputTensor = worker.PeekOutput();
//            ParseYOLOOutput(outputTensor, originalWidth, originalHeight);
//            outputTensor.Dispose();
//        }
//    }

//    void ParseYOLOOutput(Tensor output, int imgW, int imgH)
//    {
//        // ... (Your existing Output Shape detection logic remains here) ...
//        // For brevity, I am assuming the shape logic works as provided in your snippet
//        // Copy your dimension finding logic back here if it's not working, 
//        // but the key change is inside the detection loop below:

//        // --- Simplified Shape Logic for Context ---
//        int numPredictions = output.width > 100 ? output.width : output.height;
//        // (Ensure you keep your full dimension logic from the original script here)
//        // ------------------------------------------

//        bool detectionFound = false;

//        // Note: Make sure to include your Dimension Finding Logic from your original script here.
//        // I am simplifying the loop access for clarity on the *Placement* part.

//        // Assuming [1, 8400, 5] or [1, 5, 8400] logic is handled:
//        for (int i = 0; i < numPredictions; i++)
//        {
//            // *Use your GetValue helper from your original script*
//            // float conf = GetValue(4, i); 

//            // Placeholder for logic (REPLACE with your actual GetValue logic):
//            float conf = 0f;
//            if (output.channels == 5376) conf = output[0, 4, i, 0]; // Example for [1,5,5376]
//            else conf = output[0, 0, i, 4]; // Example for [1,5376,5]

//            if (conf > CONFIDENCE_THRESHOLD)
//            {
//                detectionFound = true;

//                // REPLACE with your actual GetValue logic
//                float cx = (output.channels == 5376) ? output[0, 0, i, 0] : output[0, 0, i, 0];
//                float cy = (output.channels == 5376) ? output[0, 1, i, 0] : output[0, 0, i, 1];
//                float w = (output.channels == 5376) ? output[0, 2, i, 0] : output[0, 0, i, 2];
//                float h = (output.channels == 5376) ? output[0, 3, i, 0] : output[0, 0, i, 3];

//                Rect screenRect = PostProcessCoordinates(cx, cy, w, h, imgW, imgH);

//                Debug.Log($"<color=green>DETECTED!</color> Conf: {conf:F2}");

//                // --- NEW LOGIC START ---
//                if (placementManager != null)
//                {
//                    // Pass the Center of the bounding box to the placement manager
//                    string label = $"Plant {conf:P0}";
//                    placementManager.PlaceLabelAtLocation(screenRect.center, label);
//                }
//                // --- NEW LOGIC END ---

//                break; // Stop after first detection
//            }
//        }

//        // If nothing found
//        if (!detectionFound)
//        {
//            Debug.Log("No detection above threshold.");
//        }
//    }

//    private Rect PostProcessCoordinates(float cx, float cy, float w, float h, int originalW, int originalH)
//    {
//        float scale = Mathf.Min((float)INPUT_SIZE / originalW, (float)INPUT_SIZE / originalH);
//        float padX = (INPUT_SIZE - originalW * scale) / 2f;
//        float padY = (INPUT_SIZE - originalH * scale) / 2f;
//        float unpadCX = cx - padX;
//        float unpadCY = cy - padY;
//        float finalCX = unpadCX / scale;
//        float finalCY = unpadCY / scale;
//        float finalW = w / scale;
//        float finalH = h / scale;

//        // Invert Y for UI if needed (Screen.height - y)
//        // But for AR Raycasting, usually standard Screen coords (Bottom-Left 0,0) or (Top-Left 0,0) depends on Unity version.
//        // Unity Input.mousePosition is Bottom-Left. YOLO is Top-Left.

//        float unityY = Screen.height - finalCY; // Invert YOLO Y to match Unity Screen Space

//        return new Rect(finalCX - (finalW / 2f), unityY - (finalH / 2f), finalW, finalH);
//    }

//    private Texture2D Letterbox(Texture2D source, int targetSize)
//    {
//        // (Keep your existing Letterbox code here)
//        Texture2D result = new Texture2D(targetSize, targetSize, TextureFormat.RGB24, false);
//        Color[] fill = new Color[targetSize * targetSize];
//        for (int i = 0; i < fill.Length; i++) fill[i] = Color.black;
//        result.SetPixels(fill);
//        float scale = Mathf.Min((float)targetSize / source.width, (float)targetSize / source.height);
//        int newWidth = (int)(source.width * scale);
//        int newHeight = (int)(source.height * scale);
//        int offsetX = (targetSize - newWidth) / 2;
//        int offsetY = (targetSize - newHeight) / 2;
//        RenderTexture rt = RenderTexture.GetTemporary(targetSize, targetSize);
//        RenderTexture.active = rt;
//        GL.Clear(true, true, Color.black);
//        Graphics.DrawTexture(new Rect(offsetX, offsetY, newWidth, newHeight), source);
//        result.ReadPixels(new Rect(0, 0, targetSize, targetSize), 0, 0);
//        result.Apply();
//        RenderTexture.active = null;
//        RenderTexture.ReleaseTemporary(rt);
//        return result;
//    }

//    void OnDestroy()
//    {
//        worker?.Dispose();
//    }
//}



using UnityEngine;

public class CaptureCamera : MonoBehaviour
{
    // Reference to the manager (Assign in Inspector)
    public ARPlacementManager placementManager;

    // We don't need Start() anymore because we aren't loading models.

    // This function is called by your Button
    public void OnCaptureButtonPressed()
    {
        if (placementManager != null)
        {
            // Instead of capturing an image, we just tell the manager 
            // to show the next item in the list.
            placementManager.CycleToNextPlant();
        }
        else
        {
            Debug.LogError("ARPlacementManager is not assigned in the CaptureCamera script!");
        }
    }
}