using System.Collections.Generic;
using UnityEngine;

//! Global game state that persists across scenes.
//? Holds Gold, GlobalMultipliers, and shop / purchased data.
//* Attach as a GameObject in your first scene (MainMenuScene) and mark DontDestroyOnLoad.
public class GlobalGameState : MonoBehaviour
{
    public static GlobalGameState Instance { get; private set; }

    [Header("Global Values")]
    public int Gold = 0;                                 //! starts at 0
    public List<float> GlobalMultipliers = new();       //! will be seeded with 7 entries of 1f

    [Header("Shop / Purchases")]
    public List<bool> PurchasedItems = new();           //! index-based purchased flags (7 items)
    public List<CardData> PurchasedCardDatas = new();   //! ScriptableObjects queued to be added to decks

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Seed multipliers if necessary
        if (GlobalMultipliers.Count == 0)
        {
            for (int i = 0; i < 7; i++) GlobalMultipliers.Add(1f);
        }

        // Ensure PurchasedItems list has 7 entries (false)
        if (PurchasedItems.Count < 7)
        {
            int toAdd = 7 - PurchasedItems.Count;
            for (int i = 0; i < toAdd; i++) PurchasedItems.Add(false);
        }
    }

    //! Try to spend gold, returns true if successful (atomic)
    public bool TrySpendGold(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("[GlobalGameState] TrySpendGold called with negative amount.");
            return false;
        }

        if (Gold >= amount)
        {
            int before = Gold;
            Gold -= amount;
            Debug.Log($"[GlobalGameState] Spent {amount} gold. before={before}, now={Gold}");
            return true;
        }

        Debug.Log($"[GlobalGameState] Not enough gold to spend {amount}. current={Gold}");
        return false;
    }

    //! Mark a shop index as purchased (idempotent)
    public void MarkPurchased(int index)
    {
        if (index < 0) return;
        if (index >= PurchasedItems.Count)
        {
            // grow list (safety)
            for (int i = PurchasedItems.Count; i <= index; i++) PurchasedItems.Add(false);
        }
        PurchasedItems[index] = true;
        Debug.Log($"[GlobalGameState] Marked shop item {index} as purchased.");
    }

    public bool IsPurchased(int index)
    {
        return (index >= 0 && index < PurchasedItems.Count) && PurchasedItems[index];
    }

    //! Add a CardData to the queue that CardHandManager will insert into the deck when the cardgame scene loads
    public void AddPurchasedCard(CardData card)
    {
        if (card == null) return;
        PurchasedCardDatas.Add(card);
        Debug.Log($"[GlobalGameState] Added purchased card '{card.displayName}' to PurchasedCardDatas (count={PurchasedCardDatas.Count}).");
    }

    //! Set a global multiplier at index
    public void SetMultiplier(int idx, float value)
    {
        if (idx < 0) return;
        if (idx >= GlobalMultipliers.Count)
        {
            for (int i = GlobalMultipliers.Count; i <= idx; i++) GlobalMultipliers.Add(1f);
        }
        GlobalMultipliers[idx] = value;
        Debug.Log($"[GlobalGameState] GlobalMultipliers[{idx}] set to {value}");
    }

    //! Read multiplier
    public float GetMultiplier(int idx)
    {
        if (idx < 0 || idx >= GlobalMultipliers.Count) return 1f;
        return GlobalMultipliers[idx];
    }
}






/*
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
*/