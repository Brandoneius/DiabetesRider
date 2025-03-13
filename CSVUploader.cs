using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using SFB;

public class CSVUploader : MonoBehaviour
{
    public Text filePathText;
    public Button startRideButton;

    private string filePath = "";

    void Start()
    {
        // Ensure UI starts in the correct state
        if (filePathText == null || startRideButton == null)
        {
            Debug.LogError("UI Elements are not assigned! Drag them in the Inspector.");
            return;
        }

        filePathText.text = "No file selected";
        startRideButton.interactable = false;
    }

    public void ChooseFile()
    {
        Debug.Log("ChooseFile button clicked!");

#if UNITY_EDITOR
        filePath = UnityEditor.EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
#else
        Debug.LogError("File dialog not available in built game! Use StandaloneFileBrowser.");
#endif

        if (!string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Selected file: " + filePath);


            string fileName = Path.GetFileName(filePath);
            Debug.Log("Extracted File Name: " + fileName);

            filePathText.text = "Selected: " + fileName;
            //filePathText.text = "Selected: " + Path.GetFileName(filePath);
            //This is an attempt at refreshing the UI
            filePathText.enabled = false;
            filePathText.enabled = true;


            startRideButton.interactable = true;
        }
        else
        {
            Debug.Log("No file selected.");
            filePathText.text = "No file selected";
            startRideButton.interactable = false;
        }
    }

    public void StartRide()
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            PlayerPrefs.SetString("CSVFilePath", filePath); // Save file path
            PlayerPrefs.Save(); // Ensure it's stored
            SceneManager.LoadScene("DiabetesRiderGamePlay"); // Load Scene 2
        }
    }
}
