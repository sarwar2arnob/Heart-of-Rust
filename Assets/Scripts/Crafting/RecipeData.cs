using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("Display")]
    public string recipeName;       // e.g. "Leg Actuator"
    public Sprite recipePageImage;  // the full recipe page art you show on interact
    public Sprite resultIcon;       // small icon shown after crafting

    public List<ItemAmount> inputs;
    public CraftResult result;

    [Header("Progression")]
    public bool unlockedByDefault = false;
}

[System.Serializable]
public struct ItemAmount
{
    public ItemData item;
    public int amount;
}