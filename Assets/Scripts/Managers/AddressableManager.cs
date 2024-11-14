using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager
{
    public static void LoadInstantiateAsync<T>(AssetReference asset) where T : Object
    {
        var assetLoadOperationHandle = Addressables.LoadAssetAsync<T>(asset);
        assetLoadOperationHandle.Completed += InstantiateOnLoadComplete;
    }

    private static void InstantiateOnLoadComplete<T>(AsyncOperationHandle<T> obj) where T : Object
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Object.Instantiate(obj.Result);
        }
        else
        {
            obj.Release();
        }
    }

    //https://docs.unity3d.com/Packages/com.unity.addressables@2.2/manual/AddressableAssetsAsyncOperationHandle.html
    internal class LoadWithEvent : MonoBehaviour
    {
        public string address;
        AsyncOperationHandle<GameObject> opHandle;

        void Start()
        {
            // Create operation
            opHandle = Addressables.LoadAssetAsync<GameObject>(address);
            // Add event handler
            opHandle.Completed += Operation_Completed;
        }

        private void Operation_Completed(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(obj.Result, transform);
            }
            else
            {
                obj.Release();
            }
        }

        void OnDestroy()
        {
            opHandle.Release();
        }
    }

    internal class LoadWithIEnumerator : MonoBehaviour
    {
        public string address;
        AsyncOperationHandle<GameObject> opHandle;

        public IEnumerator Start()
        {
            opHandle = Addressables.LoadAssetAsync<GameObject>(address);

            // yielding when already done still waits until the next frame
            // so don't yield if done.
            if (!opHandle.IsDone)
                yield return opHandle;

            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(opHandle.Result, transform);
            }
            else
            {
                opHandle.Release();
            }
        }

        void OnDestroy()
        {
            opHandle.Release();
        }
    }
}