using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDisappearController : MonoBehaviour
{
    public float Speed = 2f;
    [SerializeField,Range(0,1)]
    private float _process = 0f;
    public float Process
    {
        get => _process;
        set
        {
            _process = Mathf.Clamp01(value);
            _material.SetFloat("_Process", _process);
        }
    }

    private Material _material;

    private void OnValidate()
    {
        if (_material == null) _material = GetComponent<Image>().material;
        Process = _process;
    }

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Image>().material;
    }

    private void OnEnable()
    {
        Process = 0;
    }

    private void Update()
    {
        Process += Time.deltaTime * Speed;
    }
}
