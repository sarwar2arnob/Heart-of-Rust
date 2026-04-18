using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private GameObject recipeButtonPrefab;

    private CraftingSystem currentCraftingSystem;
    private Toolbox currentToolbox;
    private InventorySystem inventory;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
    }

    public void Open(Toolbox toolbox, CraftingSystem craftingSystem)
    {
        currentToolbox = toolbox;
        currentCraftingSystem = craftingSystem;

        panel.SetActive(true);

        BuildRecipeList();
    }

    public void Close()
    {
        panel.SetActive(false);

        // 🔥 Force player back to normal state
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }

    void BuildRecipeList()
    {
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in currentCraftingSystem.recipes)
        {
            GameObject obj = Instantiate(recipeButtonPrefab, recipeContainer);

            CraftingRecipeUI ui = obj.GetComponent<CraftingRecipeUI>();
            ui.Setup(recipe, this, inventory, currentCraftingSystem);
        }
    }

    public void TryCraft(RecipeData recipe)
    {
        if (currentCraftingSystem.TryCraft(recipe, inventory))
        {
            Debug.Log("Craft success!");
            BuildRecipeList(); // 🔥 refresh (for availability)
        }
    }
}