using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
public class AddressablesManager : MonoBehaviour
{
    [SerializeField]
    private AssetReference _playerReference;

    [SerializeField]
    private AudioReference _audioReference;

    [SerializeField]
    private AssetReferenceTexture2D _textureReference;

    [SerializeField]
    private CinemachineVirtualCamera _camera;

    [SerializeField]
    private RawImage _logo;

    private GameObject playerIstance;

    private void Start()
    {
        Addressables.InitializeAsync().Completed += OnAddressablesInitCompleted;
    }

    private void OnAddressablesInitCompleted(AsyncOperationHandle<IResourceLocator> loctator)
    {
        _playerReference.InstantiateAsync().Completed += (obj) =>
        {
            playerIstance = obj.Result;
            _camera.Follow = obj.Result.transform.Find("PlayerCameraRoot");
        };
        _audioReference.LoadAssetAsync<AudioClip>().Completed += (audio) =>
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = audio.Result;
            source.playOnAwake = false;
            source.loop = true;
            source.Play();
        };

        _textureReference.LoadAssetAsync().Completed += (texture) =>
        {
            _logo.texture = texture.Result as Texture2D;
            _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, 1f);
        };
    }

    private void OnDestroy()
    {
        _playerReference.ReleaseInstance(playerIstance);
        _textureReference.ReleaseAsset();
        _audioReference.ReleaseAsset();
    }
}

[Serializable]
public class AudioReference : AssetReferenceT<AudioClip>
{
    public AudioReference(string guid) : base(guid) { }
}