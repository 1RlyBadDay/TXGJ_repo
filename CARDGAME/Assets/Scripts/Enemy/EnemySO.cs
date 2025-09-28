using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemySO", menuName = "BattleGame/EnemySO")]
public class EnemySO : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public float HEALTH;
    public float SPEED;


    //attacks removed for now
    //public List<Attack> attacks;
    [Header("Attack Settings")]
        public float damage;
        public float reachargeTime;
        public float attackRange;
        public AnimationClip attackAnimation;
    
}
