using UnityEngine;

public enum IngredientType
{
    Bread, Meat, Lettuce, Cheese, Tomato
}

[CreateAssetMenu (fileName = "Ingredient", menuName = "Kitchen/Ingredient")]
public class IngredientSO : ScriptableObject
{
    public IngredientType ingredientType;
    public string IgrendientName;
    public Sprite IngredientIcon;
    public GameObject IngredientPrefab;
}
