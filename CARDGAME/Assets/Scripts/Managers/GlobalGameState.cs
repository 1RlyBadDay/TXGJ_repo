using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds game-wide variables like Gold and GlobalMultipliers.
/// Persists across all scenes.
/// </summary>
public class GlobalGameState : MonoBehaviour
{
    // Singleton instance
    public static GlobalGameState Instance { get; private set; }

    [Header("Global Values")]
    public int Gold = 0;                               // starts at 0
    public List<float> GlobalMultipliers = new();       // filled with 1s at startup

    private void Awake()
    {
        // Enforce single instance & persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Fill multipliers only once (if empty)
        if (GlobalMultipliers.Count == 0)
        {
            for (int i = 0; i < 7; i++)
                GlobalMultipliers.Add(1f);
        }
    }

    /// <summary>Add gold safely.</summary>
    public void AddGold(int amount)
    {
        Gold += amount;
        Debug.Log($"Gold is now {Gold}");
    }
}
