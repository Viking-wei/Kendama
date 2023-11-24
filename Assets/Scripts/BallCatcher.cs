using System;
using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    [SerializeField] private SwordController _sword;
    public Vector3 Offset;
    public ColorTag CatcherColorTag=ColorTag.Red;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("ball caught");
        _sword.BallCaught = other.rigidbody;
        _sword.BallCaught.isKinematic = true;
        _sword.RelativePosition = transform.rotation * Offset + transform.position - _sword.transform.position;
        
        if (CompareTag("Top"))
            return;
        other.gameObject.GetComponent<TamaProperties>().SetColorTag(CatcherColorTag);
        _sword.ThrowBall();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.8f, 0.3f, 0.4f, 1f);
        Gizmos.DrawSphere(transform.position + transform.rotation * Offset, 0.4425f);
    }
}