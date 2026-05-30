using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Characters/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterId;
    public string characterName;

    public CharacterType characterType;

    public AssetReferenceSprite portrait2D;

    public AssetReferenceGameObject model3D;
}
