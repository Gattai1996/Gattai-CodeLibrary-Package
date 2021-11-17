using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class stores an array of <c>Sprites</c> to be easily accessible
/// without need of the Unity's folder <c>Resources</c>.
/// The Sprites on this array can be taken at runtime
/// by name using the static method GetSpriteByName(nameOfSprite).
/// The name of Sprite is case sensitive.
/// </summary>
public class SpritesDataBase : MonoBehaviour
{
    private static SpritesDataBase _instance; // Used to have a reference of this GameObject on static level internally
    [SerializeField, Tooltip("The Sprites on this array can be taken at runtime by name using the static " +
                             "method GetSpriteByName(nameOfSprite). The name of Sprite is case sensitive.")] 
    private Sprite[] sprites;

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// Searches a Sprite by name on the Sprites array and returns the first matching result.
    /// </summary>
    /// <param name="nameOfSprite">The name of the sprite that will be searched.</param>
    /// <returns>Sprite</returns>
    public static Sprite GetSpriteByName(string nameOfSprite)
    {
        // Query on the sprites array and filter by name
        var enumerable = _instance.sprites.Where(sprite => sprite.name == nameOfSprite);
        
        // Converts enumerable to array
        var array = enumerable as Sprite[] ?? enumerable.ToArray();
        
        // Check if have any results
        if (!array.Any())
        {
            throw new ArgumentException($"The Sprites array '{nameof(_instance.sprites)}' on Game Object " +
                                     $"{_instance.gameObject.name} has not Sprite named '{nameOfSprite}'. " +
                                     $"Verify the '{nameof(nameOfSprite)}' parameter  when calling the method " +
                                     $"'{nameof(GetSpriteByName)}()' and certify that have a Sprite " +
                                     $"with same name on the Sprites array '{nameof(_instance.sprites)}'.");
        }
        
        return array.First();
    }
}
