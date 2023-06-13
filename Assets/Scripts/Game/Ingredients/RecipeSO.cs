using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Kitchen/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<IngredientSO> RecipeList = new List<IngredientSO>();
    public string RecipeName;
    public Sprite RecipeIcon;
}
