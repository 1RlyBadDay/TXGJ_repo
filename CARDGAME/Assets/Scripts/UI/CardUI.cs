using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  //! for TextMeshProUGUI


//! UI component for one card instance in the player's hand.
//? Displays art, name, type, effect summary, sequence, and a lifetime bar.
//* Fires OnClicked when player clicks card; fires OnExpired when lifetime runs out.
public class CardUI : MonoBehaviour
{
    [Header("References (assign in prefab)")]
    public Image artImage;
    public TextMeshProUGUI  nameText;
    public TextMeshProUGUI  typeText;
    public TextMeshProUGUI  effectText;       // generic effect line: "Damage: 15" / "Heal: 20"
    public TextMeshProUGUI  sequenceText;     // shows full sequence for preview in hand
    public Button playButton;
    public Image lifetimeBar;     // filled image to show remaining hand lifetime

    private CardData cardData;
    private float lifeTimer;
    private bool lifetimePaused = false;

    public CardData CardData => cardData;

    //! Events
    public event Action<CardUI> OnClicked;
    public event Action<CardUI> OnExpired;

    //! Initialize UI from CardData
    public void SetData(CardData data)
    {
        cardData = data;
        lifeTimer = data.handLifetime;
        lifetimePaused = false;

        if (artImage && data.cardArt) artImage.sprite = data.cardArt;
        if (nameText) nameText.text = data.displayName;
        if (typeText) typeText.text = data.cardType.ToString();
        if (effectText) effectText.text = BuildEffectText(data);
        if (sequenceText) sequenceText.text = SequenceToString(data.inputSequence);

        if (playButton)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayButton);
        }

        // initial lifetime visual
        if (lifetimeBar) lifetimeBar.fillAmount = 1f;
    }

    private void Update()
    {
        if (cardData == null) return;
        if (lifetimePaused) return;

        lifeTimer -= Time.deltaTime;

        if (lifetimeBar && cardData.handLifetime > 0f)
            lifetimeBar.fillAmount = Mathf.Clamp01(lifeTimer / cardData.handLifetime);

        if (lifeTimer <= 0f)
        {
            //! Let the manager handle removal/destruction (raise event)
            OnExpired?.Invoke(this);
        }
    }

    //! Called by the Button
    private void OnPlayButton()
    {
        OnClicked?.Invoke(this);
    }

    //! Called by manager to pause lifetime while the player attempts the mini-game
    public void SetLifetimePaused(bool paused)
    {
        lifetimePaused = paused;
    }

    //! Helper: show effect text based on type
    private string BuildEffectText(CardData d)
    {
        switch (d.cardType)
        {
            case CardType.Attack: return $"Damage: {d.damageAmount}";
            case CardType.Heal:   return $"Heal: {d.healAmount}";
            case CardType.Buff:   return $"Buff: +{d.buffAmount}% ({d.buffDuration}s)";
            default: return "";
        }
    }

    private string SequenceToString(InputKey[] seq)
    {
        if (seq == null || seq.Length == 0) return "";
        var sb = new StringBuilder();
        foreach (var k in seq) sb.Append(KeyToSymbol(k)).Append(" ");
        return sb.ToString().Trim();
    }

    private string KeyToSymbol(InputKey k) =>
        k switch
        {
            InputKey.Left => "<",
            InputKey.Right => ">",
            InputKey.Up => "^",
            InputKey.Down => "v",
            _ => k.ToString()
        };
}
