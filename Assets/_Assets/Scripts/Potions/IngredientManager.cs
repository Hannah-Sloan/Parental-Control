using System.Collections.Generic;

public class IngredientManager : Singleton<IngredientManager>
{
    private List<PotionItemPickup> ingredients = new List<PotionItemPickup>();

    public void Register(PotionItemPickup ingredient)
    {
        if (ingredient == null) return;
        if (ingredients.Contains(ingredient)) return;
        ingredients.Add(ingredient);
    }

    public void Remove(PotionItemPickup ingredient)
    {
        ingredients.Remove(ingredient);
    }

    public bool HarvestStatus() 
    {
        foreach (var ingredient in ingredients)
        {
            if (ingredient.harvesting) return true;
        }
        return false;
    }
}
