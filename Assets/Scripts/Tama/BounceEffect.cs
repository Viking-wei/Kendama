using System;
using UnityEngine;
using UnityEngine.VFX;

public class BounceEffect : MonoBehaviour
{
    [HideInInspector]
    public BounceEffectSpawner BounceEffectSpawner;
    public VisualEffect VisualEffect;
    public float PlayTime = 1.5f;
    private float _currentPlayTime;

    private void Start()
    { 
        VisualEffect = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        _currentPlayTime = PlayTime;
        VisualEffect.Play();
    }

    private void Update()
    {
        _currentPlayTime -= Time.deltaTime;

        if (_currentPlayTime < 0f)
        {
            BounceEffectSpawner.ReturnEffect(VisualEffect);
        }
    }
}