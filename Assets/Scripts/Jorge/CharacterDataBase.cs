using UnityEngine;
using System.Collections.Generic;

public class CharacterDataBase : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> characters;

    private Dictionary<string, CharacterData> characterDictionary;

    private void Awake()
    {
        characterDictionary = new Dictionary<string, CharacterData>();

        foreach (CharacterData character in characters)
        {
            characterDictionary.Add(character.characterId, character);
        }
    }

    public CharacterData GetCharacter(string id)
    {
        return characterDictionary[id];
    }
}
