using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipes
{
    public List<IngredientSO> RecipeList = new List<IngredientSO>();
    public string RecipeName;
    public float WaitTime;
}

[CreateAssetMenu(fileName = "Recipe", menuName = "Kitchen/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<Recipes> recipes = new List<Recipes>();
}
