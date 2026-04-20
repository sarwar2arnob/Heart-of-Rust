using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float amount;
    public DamageType type;
    public Vector2 sourcePosition;

    public DamageData(float amount, DamageType type, Vector2 sourcePosition)
    {
        this.amount = amount;
        this.type = type;
        this.sourcePosition = sourcePosition;
    }
}

public enum DamageType
{
    Physical,
    Fire,
    Electric,
    Gas
}