using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class RecipeData : ScriptableObject
{
    public List<ItemAmount> inputs;
    public CraftResult result;
}

[System.Serializable]
public struct ItemAmount
{
    public ItemData item;
    public int amount;
}