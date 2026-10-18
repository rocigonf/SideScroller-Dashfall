using UnityEngine;

public enum PowerUpType { Dash };

[CreateAssetMenu(fileName = "PowerUpScriptableObject", menuName = "Power Ups/Power Up Data")]
public class PowerUpScriptableObject : ScriptableObject
{
    [SerializeField] float powerUpValue;
    [SerializeField] float timeLimit;

    public PowerUpType type;
    public float PowerUpValue { get => powerUpValue; set => powerUpValue = value; }
    public float TimeLimit { get => timeLimit; set => timeLimit = value; }
}
