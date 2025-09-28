using UnityEngine;

//! Basic possible input keys used in card sequences
//? Add more keys if you want (Space, Enter, mouse, etc.)
public enum InputKey { Left, Right, Up, Down, A, S, D, W }

//! Types of card effects
public enum CardType { Attack, Heal, Buff }

//* ScriptableObject describing a single card and its gameplay values
[CreateAssetMenu(menuName = "BattleGame/Card Data", fileName = "NewCardData")]
public class CardData : ScriptableObject
{
    [Header("Visual / Flavor")]
    public Sprite cardArt;
    public string displayName;
    [TextArea] public string description;

    [Header("Gameplay")]
    public CardType cardType;          //! Attack / Heal / Buff
    public InputKey[] inputSequence;   //! Arrow sequence required
    public float timeLimit = 2f;       //! Seconds allowed once player starts the sequence

    [Header("Effect Values")]
    public int damageAmount;           //! Used if Attack
    public int healAmount;             //! Used if Heal
    public float buffAmount;           //! Used if Buff (interpreted by game logic)
    public float buffDuration;         //! How long buff lasts in seconds
    public AnimationClip attackAnimation;  //! Optional animation to play when card is played
    public float attackRange;        //! Used if Attack (for targeting, etc.)
    public float reachargeTime;     //! Used if Attack (for cooldowns, etc.)

    [Header("Hand Lifetime")]
    public float handLifetime = 8f;    //! How long card stays in hand before auto-discard
}




















//! OLDER VERSIONS, may discard soon:
/*
using UnityEngine;

//! Enum of all possible input keys a card can require
//? Extend this if you want more keys (e.g., spacebar, mouse clicks)
public enum InputKey { Left, Right, Up, Down, A, S, D, W } // extend as needed

//* ScriptableObject holding all gameplay data for a single card
//TODO: Add fields later for special effects (damage type, sound, etc.)
[CreateAssetMenu(menuName = "BattleGame/Card Data", fileName = "NewCardData")]
public class CardData : ScriptableObject
{
    //! Unit to spawn or affect when the card is successfully played
    public UnitData unit;

    //! Artwork shown on the card UI
    public Sprite cardArt;

    //? Optional flavor text or tooltip
    public string description;

    [Header("Gameplay")]
    //! Ordered key sequence the player must input (e.g., Left, Left, Up, Left)
    public InputKey[] inputSequence;

    //! Time in seconds allowed to complete the input sequence
    public float timeLimit = 2f;
}

*/


/*
using UnityEngine;

[CreateAssetMenu(menuName = "BattleGame/Card Data", fileName = "NewCardData")]
public class CardData : ScriptableObject
{
    public UnitData unit;        // the unit this card spawns
    public Sprite cardArt;       // artwork for the card background
    public string description;   // flavor text or tooltip
}
*/

/*
using UnityEngine;

public enum InputKey { Left, Right, Up, Down, A, S, D, W } // extend as needed

[CreateAssetMenu(menuName = "BattleGame/Card Data", fileName = "NewCardData")]
public class CardData : ScriptableObject
{
    public UnitData unit;           // optional: if the card spawns a unit
    public Sprite cardArt;
    public string description;

    [Header("Gameplay")]
    public InputKey[] inputSequence; // e.g. [Left, Left, Up, Left]
    public float timeLimit = 2f;     // seconds to finish the sequence
}

*/