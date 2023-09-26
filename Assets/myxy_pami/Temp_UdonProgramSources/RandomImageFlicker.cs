
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RandomImageFlicker : UdonSharpBehaviour
{
    private Material _material;
    [SerializeField]
    private Texture[] _textureList;
    private float _lastFlickTime = 0f;
    private float _accumulatedTime = 0f;
    [SerializeField, Range(0.01f, 0.1f)]
    private float _flickerPeriod = 0.1f;
    private void Start()
    {
        _material = this.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        _accumulatedTime += Time.unscaledDeltaTime;
        if (_accumulatedTime - _lastFlickTime > _flickerPeriod)
        {
            var randomIndex = Random.Range(0, _textureList.Length); 
            _material.SetTexture("_MainTex", _textureList[randomIndex]);
            _lastFlickTime = _accumulatedTime;
        }
    }
}
