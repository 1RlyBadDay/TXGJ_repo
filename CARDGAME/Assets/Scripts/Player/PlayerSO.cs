using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "BattleGame/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public int MAX_HEALTH;
    public int STARTING_HEALTH;
    public float SPEED;
    public Sprite sprite;
    public GameObject prefab;


}
