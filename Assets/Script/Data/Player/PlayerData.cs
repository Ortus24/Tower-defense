using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public int maxHP;
    public float moveSpeed;
    public float damage;
}