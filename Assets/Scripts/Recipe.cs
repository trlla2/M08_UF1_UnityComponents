using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Creafting/Recipe")]
public class Recipe : ScriptableObject
{
    public List<Ingredient> ingredients;
    public Ingredient result;
}
