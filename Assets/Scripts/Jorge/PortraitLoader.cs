using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PortraitLoader : MonoBehaviour
{
    [SerializeField]
    private Image portraitImage;

    public void LoadPortrait(AssetReferenceSprite portraitReference)
    {
        portraitReference.LoadAssetAsync<Sprite>().Completed += OnPortraitLoaded;
    }

    private void OnPortraitLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            portraitImage.sprite = handle.Result;
        }
    }
}
