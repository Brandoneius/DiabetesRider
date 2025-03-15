using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVLoader : MonoBehaviour
{
    public LineRenderer lineRenderer; // Assign in Unity Inspector

    void Start()
    {
        string filePath = PlayerPrefs.GetString("CSVFilePath", "");
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("❌ No CSV file path found!");
            return;
        }

        Debug.Log($"📂 Loading CSV from: {filePath}");

        // Load and process CSV data
        List<Vector2> trackPoints = ReadCSV(filePath);
        if (trackPoints.Count > 0)
        {
            DrawTrack(trackPoints);
        }
    }

List<Vector2> ReadCSV(string path)
{
    List<Vector2> points = new List<Vector2>();

    if (!File.Exists(path))
    {
        Debug.LogError("❌ File does not exist: " + path);
        return points;
    }

    string[] lines = File.ReadAllLines(path);
    Debug.Log($"✅ Read {lines.Length} lines from CSV.");

    DateTime startTime = DateTime.MinValue;
    float SCALE_FACTOR = 0.01f;

    for (int i = 1; i < lines.Length; i++) // Skip header
    {
        string[] values = lines[i].Split(',');

        if (values.Length < 8) continue; // Ensure valid data

        string eventType = values[2].Trim().Replace("\"", "");
        string timestampStr = values[1].Trim().Replace("\"", "");
        string glucoseStr = values[7].Trim().Replace("\"", "");

        if (eventType != "EGV") continue;
        if (!float.TryParse(glucoseStr, out float glucoseValue)) continue;

        if (!DateTime.TryParseExact(timestampStr, "yyyy-MM-ddTHH:mm:ss", null, 
            System.Globalization.DateTimeStyles.None, out DateTime timestamp)) continue;

        if (startTime == DateTime.MinValue) startTime = timestamp;

        float timeSinceStart = (float)(timestamp - startTime).TotalSeconds * SCALE_FACTOR;

        // **Smoothing: Average with previous value**
        if (points.Count > 0)
        {
            glucoseValue = Mathf.Lerp(points[points.Count - 1].y, glucoseValue, 0.5f);
        }

        points.Add(new Vector2(timeSinceStart, glucoseValue));
    }

    Debug.Log($"🎢 Track Points Generated: {points.Count}");
    return points;
}

//End of ReadCSV


public GameObject riderPrefab; // Assign the Rider prefab in Inspector
public GameObject gummyPrefab; // Assign in Inspector

void DrawTrack(List<Vector2> points)
{
    if (lineRenderer == null)
    {
        Debug.LogError("❌ LineRenderer is not assigned!");
        return;
    }

    lineRenderer.positionCount = points.Count;

    for (int i = 0; i < points.Count; i++)
    {
        lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
    }

    Debug.Log("✅ Track successfully drawn!");

    //  Find highest point near the start 
    if (riderPrefab != null && points.Count > 0)
    {
        float spawnX = points[0].x;
        float highestY = points[0].y;

        int range = Mathf.Min(10, points.Count);
        for (int i = 0; i < range; i++)
        {
            if (points[i].y > highestY)
            {
                highestY = points[i].y;
                spawnX = points[i].x;
            }
        }

        // 🚨 Fail-safe: Ensure Rider never spawns at X = 0
        if (spawnX == 0)
        {
            spawnX = points[Mathf.Min(range, points.Count - 1)].x; // Pick a further point
            //highestY = points[Mathf.Min(range, points.Count - 1)].y;
            Debug.LogWarning($"⚠️ Highest point was at X=0, adjusted spawn to ({spawnX}, {highestY})");
        }

        Instantiate(riderPrefab, new Vector3(spawnX, highestY, 0), Quaternion.identity);
        Debug.Log($"🏎 Rider spawned at ({spawnX}, {highestY})");
    }


    // 🍬 Spawn Gummies randomly along the track 🍬
    if (gummyPrefab != null && points.Count > 10)
    {
        int gummyCount = Mathf.Max(5, points.Count / 25); // Adjust frequency
        for (int i = 0; i < gummyCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(5, points.Count - 5); // Avoid edges

            Vector2 gummyPosition = points[randomIndex];
            
            // Ensure it spawns a little above the track, not inside it
            Instantiate(gummyPrefab, new Vector3(gummyPosition.x, gummyPosition.y + 10, 0), Quaternion.identity);
        }
    }


    // 🎯 Add EdgeCollider2D for physics collisions 🎯
    EdgeCollider2D edgeCollider = lineRenderer.gameObject.GetComponent<EdgeCollider2D>();
    if (edgeCollider == null)
    {
        edgeCollider = lineRenderer.gameObject.AddComponent<EdgeCollider2D>();
    }

    Vector2[] colliderPoints = new Vector2[points.Count];
    for (int i = 0; i < points.Count; i++)
    {
        colliderPoints[i] = new Vector2(points[i].x, points[i].y);
    }
    
    edgeCollider.points = colliderPoints;

    Debug.Log("✅ EdgeCollider2D applied to track!");

    // 🎥 Automatically move camera to track center 🎥
    if (points.Count > 0)
    {
        float avgX = (points[0].x + points[points.Count - 1].x) / 2;
        float avgY = (points[0].y + points[points.Count - 1].y) / 2;

        Camera.main.transform.position = new Vector3(0, avgY, -10);
        Debug.Log($"🎥 Camera repositioned to ({avgX}, {avgY})");
    }
}



}
