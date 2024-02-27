using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadAddresableOnPress : MonoBehaviour
{
    [SerializeField] AssetReference prefabReference;

    private void OnEnable()
    {
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> loadAsyncOperation = prefabReference.LoadAssetAsync<GameObject>();

        loadAsyncOperation.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {

    }

    private void OnDisable()
    {
        prefabReference.ReleaseAsset();
    }
    public void OnPress()
    {
        prefabReference.InstantiateAsync();
    }
}
