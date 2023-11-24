using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BlockProperties : MonoBehaviour
{
    public ColorTag BlockColorTag;
    public int BlockScore=1;
    public int NeedHitTimes=1;
    [FormerlySerializedAs("_outLine")] public SpriteRenderer OutLine;
    
    private int _hitTimes;

    private void OnEnable()
    {
        if(OutLine is null)
            OutLine = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _hitTimes = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Tama"))
            return;
        
        var tamaProperties=other.gameObject.GetComponent<TamaProperties>();
        if (!tamaProperties.CompareColorTag(BlockColorTag))
            return;
        
        if (++_hitTimes >= NeedHitTimes)
        {
            ScoreCounter.Score += BlockScore;
            Debug.Log("Current Score"+ScoreCounter.Score);
            
            Destroy(gameObject);
        }
    }
    
    public void ChangeOutLineColor(Color color)
    {
        OutLine.color = color;
    }
}