using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeBook : ScriptableObject
{
    [SerializeField] public List<Recipe> recipes;
}

[Serializable]
public class Recipe
{
    //Shroom, Flower, Carrot
    [Tooltip("Represents number of Shrooms, Flowers, Carrots. Should always add up to 3.")]
    public Vector3 recipe;
    public Potion potion;
}

