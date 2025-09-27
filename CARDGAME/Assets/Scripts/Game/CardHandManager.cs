using System;
using System.Collections;   // needed for IEnumerator / StartCoroutine
using System.Collections.Generic;
using UnityEngine;

//! Controls deck/hand/discard: draws cards, handles click -> starts mini-game, handles expiration
public class CardHandManager : MonoBehaviour
{
    [Header("Deck / Prefabs")]
    public List<CardData> deck = new List<CardData>();   //! Fill with card assets in inspector
    public GameObject cardPrefab;                        //! Prefab that contains CardUI
    public RectTransform handParent;                     //! The CardHand RectTransform (HorizontalLayoutGroup)
    public int handSize = 5;
    [SerializeField] private float refillDelay = 2f;   // seconds to wait before drawing a new card


    //! Optional: a spawn point or references used for testing (not required)
    public Transform spawnPoint;

    //! Events
    public event Action<CardData> OnCardPlayed;  //! success
    public event Action<CardData> OnCardFailed;  //! failure (wrong input)

    //! Internal
    private List<CardUI> hand = new List<CardUI>();
    private List<CardData> discard = new List<CardData>();

    

    private void Start()
    {
        ShuffleDeck();
        DrawInitialHand();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int r = UnityEngine.Random.Range(i, deck.Count);
            var tmp = deck[i];
            deck[i] = deck[r];
            deck[r] = tmp;
        }
    }

    private void DrawInitialHand()
    {
        while (hand.Count < handSize)
        {
            if (!DrawCard()) break;
        }
    }

    public bool DrawCard()
    {
        if (deck.Count == 0) RefillDeckFromDiscard();
        if (deck.Count == 0) return false;

        CardData data = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);

        GameObject go = Instantiate(cardPrefab, handParent);
        CardUI ui = go.GetComponent<CardUI>();
        if (ui == null)
        {
            Debug.LogError("cardPrefab missing CardUI component.");
            Destroy(go);
            return false;
        }

        ui.SetData(data);
        ui.OnClicked += HandleCardClicked;
        ui.OnExpired += HandleCardExpired;
        hand.Add(ui);
        return true;
    }

    private void HandleCardClicked(CardUI ui)
    {
        // Pause lifetime while playing the mini-game
        ui.SetLifetimePaused(true);

        // Start input mini-game. On success/fail execute corresponding callbacks.
        CardInputManager.Instance.StartSequence(
            ui.CardData,
            onSuccessCallback: () => OnSequenceSuccess(ui),
            onFailureCallback: () => OnSequenceFail(ui)
        );
    }

    private void OnSequenceSuccess(CardUI ui)
    {
        // Notify listeners that card played successfully
        OnCardPlayed?.Invoke(ui.CardData);

        // For quick testing only: you may spawn or apply effects here.
        //TODO: Replace with your game's systems that actually apply damage/heal/buff
        if (ui.CardData.cardType == CardType.Attack)
        {
            Debug.Log($"[CardHandManager] PLAYED Attack card: {ui.CardData.displayName} -> {ui.CardData.damageAmount} dmg");
        }
        else if (ui.CardData.cardType == CardType.Heal)
        {
            Debug.Log($"[CardHandManager] PLAYED Heal card: {ui.CardData.displayName} -> heal {ui.CardData.healAmount}");
        }
        else if (ui.CardData.cardType == CardType.Buff)
        {
            Debug.Log($"[CardHandManager] PLAYED Buff card: {ui.CardData.displayName} -> buff {ui.CardData.buffAmount} for {ui.CardData.buffDuration}s");
        }

        // Move card data to discard and remove UI
        DiscardAndRemoveUI(ui);

        // Draw a replacement
        StartCoroutine(RefillAfterDelay());   // ← was DrawCard();
        // //DrawCard();
    }

    private void OnSequenceFail(CardUI ui)
    {
        // Notify fail listeners (if any)
        OnCardFailed?.Invoke(ui.CardData);

        // Move to discard and remove the UI (player failed/abandoned)
        DiscardAndRemoveUI(ui);

        // Draw a replacement
        StartCoroutine(RefillAfterDelay());   // ← was DrawCard();
        // //DrawCard();
    }

    private void HandleCardExpired(CardUI ui)
    {
        // card's life ran out before the player touched it
        Debug.Log($"[CardHandManager] Card expired: {ui.CardData.displayName}");

        // Move to discard and remove UI
        DiscardAndRemoveUI(ui);

        // Draw a replacement
        StartCoroutine(RefillAfterDelay());   // ← was DrawCard();
        // //DrawCard();
    }

    private void DiscardAndRemoveUI(CardUI ui)
    {
        if (ui == null) return;

        // Unsubscribe events
        ui.OnClicked -= HandleCardClicked;
        ui.OnExpired -= HandleCardExpired;

        // move data to discard
        discard.Add(ui.CardData);

        // remove from hand list (if present)
        hand.Remove(ui);

        // destroy UI GameObject
        if (ui.gameObject != null) Destroy(ui.gameObject);
    }

    public void RefillDeckFromDiscard()
    {
        if (discard.Count == 0) return;
        deck.AddRange(discard);
        discard.Clear();
        ShuffleDeck();
    }

    //! Public: change hand size at runtime
    public void SetHandSize(int newSize)
    {
        handSize = Mathf.Max(0, newSize);
        AdjustHandToSize();
    }

    private void AdjustHandToSize()
    {
        while (hand.Count < handSize)
        {
            if (!DrawCard()) break;
        }

        while (hand.Count > handSize)
        {
            var ui = hand[hand.Count - 1];
            DiscardAndRemoveUI(ui);
        }
    }

    private IEnumerator RefillAfterDelay()
    {
        yield return new WaitForSeconds(refillDelay);
        DrawCard();
    }

}
