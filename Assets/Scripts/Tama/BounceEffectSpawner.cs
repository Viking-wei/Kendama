using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class BounceEffectSpawner : MonoBehaviour
{
    public VisualEffect BounceEffect;
    private ObjectPool<VisualEffect> _bounceEffectPool;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _bounceEffectPool = new ObjectPool<VisualEffect>(
            () =>
            {
                var go = Instantiate(BounceEffect);
                go.gameObject.SetActive(false);
                var vfx = go.GetComponent<BounceEffect>();
                vfx.BounceEffectSpawner = this;
                vfx.VisualEffect = go.GetComponent<VisualEffect>();
                
                return go;
            },
            vfx =>
            {
                vfx.gameObject.SetActive(true);
            },
            vfx =>
            {
                vfx.gameObject.SetActive(false);
            },defaultCapacity:16,maxSize:32);
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Blocks"))
        {
            var vfx = _bounceEffectPool.Get();
            vfx.transform.position = other.GetContact(0).point;
            vfx.transform.up = other.GetContact(0).normal;
        }
    }

    public void ReturnEffect(VisualEffect vfx)
    {
        _bounceEffectPool.Release(vfx);
    }
}
