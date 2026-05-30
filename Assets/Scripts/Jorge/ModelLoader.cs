using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ModelLoader : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    private GameObject currentModel;

    public void LoadModel(AssetReferenceGameObject modelReference)
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        modelReference.InstantiateAsync(spawnPoint)
            .Completed += OnModelLoaded;
    }

    private void OnModelLoaded(
        AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            currentModel = handle.Result;
        }
    }
}
