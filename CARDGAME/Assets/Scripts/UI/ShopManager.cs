using UnityEngine;
using UnityEngine.UI;

//! Central shop logic for LimboScene.
//? Attach this to an empty GameObject (e.g. "ShopManager") in LimboScene.
//* Inspector: wire 7 buttons, set costs, assign ShopCard_1..3 ScriptableObjects for slots 0..2.
public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    public Button[] purchaseButtons;      // size = 7, assign in inspector
    public int[] costs;                   // size = 7 -> e.g. {10,100,400, ... }

    [Header("Shop Cards (first 3 buttons)")]
    public CardData shopCard1;            // for slot 0 (cost 10)
    public CardData shopCard2;            // for slot 1 (cost 100)
    public CardData shopCard3;            // for slot 2 (cost 400)

    private void Start()
    {
        // Validate arrays
        if (purchaseButtons == null || purchaseButtons.Length < 7)
            Debug.LogError("[ShopManager] Please assign 7 purchaseButtons in the inspector.");
        if (costs == null || costs.Length < 7)
            Debug.LogError("[ShopManager] Please supply 7 costs in the inspector.");

        // Wire up inspector-friendly default actions: disable already purchased buttons
        for (int i = 0; i < purchaseButtons.Length; i++)
        {
            int idx = i; // capture for lambda use if you choose to add listeners in code later
            if (GlobalGameState.Instance != null && GlobalGameState.Instance.IsPurchased(idx))
            {
                if (purchaseButtons[i] != null) purchaseButtons[i].interactable = false;
            }
        }
    }

    //! Public: called from a button OnClick, pass the slot index (0..6)
    public void AttemptPurchase(int slotIndex)
    {
        if (GlobalGameState.Instance == null)
        {
            Debug.LogError("[ShopManager] GlobalGameState missing.");
            return;
        }

        if (slotIndex < 0 || slotIndex >= costs.Length)
        {
            Debug.LogError("[ShopManager] AttemptPurchase invalid slotIndex " + slotIndex);
            return;
        }

        if (GlobalGameState.Instance.IsPurchased(slotIndex))
        {
            Debug.Log($"[ShopManager] Slot {slotIndex} already purchased.");
            return;
        }

        int cost = costs[slotIndex];
        if (!GlobalGameState.Instance.TrySpendGold(cost))
        {
            Debug.Log($"[ShopManager] Not enough gold to buy slot {slotIndex} (cost {cost}).");
            return;
        }

        // Payment succeeded — mark purchased and perform effect
        GlobalGameState.Instance.MarkPurchased(slotIndex);

        // Disable the button
        if (purchaseButtons != null && slotIndex < purchaseButtons.Length && purchaseButtons[slotIndex] != null)
        {
            purchaseButtons[slotIndex].interactable = false;
        }

        // First three slots add shop cards to the deck (via GlobalGameState queue)
        if (slotIndex == 0 && shopCard1 != null)
            GlobalGameState.Instance.AddPurchasedCard(shopCard1);
        else if (slotIndex == 1 && shopCard2 != null)
            GlobalGameState.Instance.AddPurchasedCard(shopCard2);
        else if (slotIndex == 2 && shopCard3 != null)
            GlobalGameState.Instance.AddPurchasedCard(shopCard3);

        // Slots 3..6 modify multipliers index 0..3 to 1.5
        if (slotIndex >= 3 && slotIndex <= 6)
        {
            int multiplierIndex = slotIndex - 3; // 3->0, 4->1, 5->2, 6->3
            GlobalGameState.Instance.SetMultiplier(multiplierIndex, 1.5f);
        }

        Debug.Log($"[ShopManager] Purchase successful: slot={slotIndex}, cost={cost}");
    }
}
