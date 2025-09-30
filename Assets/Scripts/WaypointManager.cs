using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public PlayerType playerType; // Assign PlayerA or PlayerB in Inspector

    public List<int> dangerZoneIndices = new List<int>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }

        // Assign danger zones based on player type
        if (playerType == PlayerType.PlayerA)
        {
            dangerZoneIndices = new List<int> { 4, 16, 22, 34, 36, 50, 57, 62 };
        }
        else if (playerType == PlayerType.PlayerB)
        {
            dangerZoneIndices = new List<int> { 1, 6, 10, 26, 28, 40, 46, 60 };
        }
    }

    public bool IsDangerZone(int index)
    {
        return dangerZoneIndices.Contains(index);
    }
}
