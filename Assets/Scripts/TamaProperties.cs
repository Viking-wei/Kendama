using System;
using UnityEngine;

public class TamaProperties : MonoBehaviour
{
    public Camera MainCamera;
    public Rigidbody2D TamaRigidbody2D;
    public Transform SwordTransform;
    public BallCatcher TopBallCatcher;
    [SerializeField] private ColorTag _ballColorTag;
    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _ballColorTag=ColorTag.Red;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (ColorTagUtilities.CheckBorder(MainCamera,transform.position, 0.445f, out var normal))
        {
            if (normal == Vector2.up)
            {
                ResetTama();
                return;
            }
            TamaRigidbody2D.velocity = Vector2.Reflect(TamaRigidbody2D.velocity, normal);
        }
    }

    private void ResetTama()
    {
        SwordTransform.position=Vector3.zero;
        SwordTransform.rotation=Quaternion.identity;
        TamaRigidbody2D.velocity=Vector2.zero;
        transform.position = Vector3.zero + TopBallCatcher.Offset;
    }
    
    public void SetColorTag(ColorTag colorTag)
    {
        _ballColorTag = colorTag;
        _spriteRenderer.color = ColorTagUtilities.ColorTag2Color(colorTag);
    }
    
    public bool CompareColorTag(ColorTag colorTag)
    {
        return _ballColorTag == colorTag;
    }
}

public enum ColorTag
{
    Red,
    Green,
    Blue
}


public static class ColorTagUtilities
{
    public static Color ColorTag2Color(ColorTag colorTag)
    {
        switch (colorTag)
        {
            case ColorTag.Red:
                return new Color(0.8f, 0.3f, 0.4f, 1f);
            case ColorTag.Green:
                return new Color(0.3f, 0.8f, 0.4f, 1f);
            case ColorTag.Blue:
                return new Color(0.3f, 0.4f, 0.8f, 1f);
            default:
                return Color.white;
        }
    }
    
    public static bool CheckBorder(Camera camera,Vector3 position,float radius,out Vector2 normal)
    {
        var screenPos=camera.WorldToScreenPoint(position);
        if(screenPos.x>=Screen.width-radius)
        {
            normal=Vector2.left;
            return true;
        }
        
        if(screenPos.x<=radius)
        {
            normal=Vector2.right;
            return true;
        }
        
        if(screenPos.y>=Screen.height-radius)
        {
            normal=Vector2.down;
            return true;
        }
        
        if(screenPos.y<=radius)
        {
            normal=Vector2.up;
            return true;
        }
        
        normal=Vector2.zero;
        return false;
    }
}

