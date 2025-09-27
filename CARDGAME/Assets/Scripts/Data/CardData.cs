using UnityEngine;

[CreateAssetMenu(menuName = "BattleGame/Card Data", fileName = "NewCardData")]
public class CardData : ScriptableObject
{
    public UnitData unit;        // the unit this card spawns
    public Sprite cardArt;       // artwork for the card background
    public string description;   // flavor text or tooltip
}
