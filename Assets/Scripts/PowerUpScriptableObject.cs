using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpScriptableObject", menuName = "Power Ups/Power Up Data")]
public class PowerUpScriptableObject : ScriptableObject
{
    [SerializeField] string powerUpType;
    [SerializeField] float powerUpValue;
    [SerializeField] float timeLimit;

    public string PowerUpType { get => powerUpType; set => powerUpType = value; }
    public float PowerUpValue { get => powerUpValue; set => powerUpValue = value; }
    public float TimeLimit { get => timeLimit; set => timeLimit = value; }
}
