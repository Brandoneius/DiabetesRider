using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVLoader : MonoBehaviour
{
    void Start()
    {
        // Get the saved file path from Scene 1
        string filePath = PlayerPrefs.GetString("CSVFilePath", "");
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError(" No CSV file path found!");
            return;
        }

        Debug.Log($"📂 Loading CSV from: {filePath}");

        // Read and print CSV content
        ReadCSV(filePath);
    }

void ReadCSV(string path)
{
    if (!File.Exists(path))
    {
        Debug.LogError("❌ File does not exist: " + path);
        return;
    }

    string[] lines = File.ReadAllLines(path);
    Debug.Log($"✅ Read {lines.Length} lines from CSV.");

    for (int i = 1; i < lines.Length; i++) // Skip header row
    {
        string[] values = lines[i].Split(',');

        if (values.Length < 8) // Ensure we have at least 8 columns
        {
            Debug.LogWarning($"⚠️ Skipping invalid line {i}: {lines[i]}");
            continue;
        }

        string eventType = values[2].Trim().Replace("\"", "");  // EKG 
        string timestamp = values[1];  // Column B (index 1)
        string glucoseStr = values[7].Trim().Replace("\"", ""); // Remove spaces & quotes


        Debug.Log($"🔍 Checking line {i}: EventType = '{eventType}' (Raw Data: {lines[i]})");

        if (eventType != "EGV")
        {
            Debug.Log($"⏩ Skipping line {i} (Event Type = {eventType})");
            continue;
        }

        if (string.IsNullOrEmpty(glucoseStr) || !float.TryParse(glucoseStr, out float glucoseValue))
        {
            Debug.LogWarning($"⚠️ Skipping line {i}: Invalid glucose value - {glucoseStr}");
            continue;
        }

        Debug.Log($"🔹 Line {i}: Time = {timestamp}, Glucose = {glucoseValue}");
    }
}


}
