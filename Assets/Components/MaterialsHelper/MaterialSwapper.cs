using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialSwapper : MonoBehaviour
{
    private MeshRenderer _renderer;

    [SerializeField] private Material _materialTolSwap;
     private Material _initialMaterial;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _initialMaterial = _renderer.material;
    }

    public void SetSecondaryMaterial()
    { 
        if(_materialTolSwap != null)
            _renderer.material = _materialTolSwap;
    }
    public void SetPrimaryMaterial()
    {
        if (_initialMaterial != null)
            _renderer.material = _initialMaterial;
    }

}
