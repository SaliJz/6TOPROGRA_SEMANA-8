using UnityEngine;

public class CharacterVisualManager : MonoBehaviour
{
    [SerializeField]
    private PortraitLoader portraitLoader;

    [SerializeField]
    private ModelLoader modelLoader;

    public void ShowCharacter(CharacterData character)
    {
        switch (character.characterType)
        {
            case CharacterType.Portrait2D:

                portraitLoader.LoadPortrait(
                    character.portrait2D);

                break;

            case CharacterType.Model3D:

                modelLoader.LoadModel(
                    character.model3D);

                break;
        }
    }
}
