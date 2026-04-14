using UnityEngine;

[System.Serializable]
public class CraftResult
{
    public ResultType type;

    public ModuleData module;
    public PartType part;
}

public enum ResultType
{
    Module,
    Part
}