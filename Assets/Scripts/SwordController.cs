using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Rigidbody2D BallCaught = null;
    public Vector3 RelativePosition;
    [SerializeField] private Vector3 _emitVelocity = new Vector3(0, 4f, 0);
    [SerializeField] private float moveSpeed = 0.1f;
    private float _currentSpeedProcess = 0f;

    #region Persistent Data

    public Vector3 MoveDirection;
    private Vector3 LastMoveDirection;
    public float TargetAngle;

    #endregion
    
    public void ThrowBall()
    {
        if (BallCaught == null) return;
        BallCaught.isKinematic = false;
        BallCaught.velocity = _emitVelocity + 0.4f*moveSpeed * MoveDirection;
        BallCaught = null;
    }

    public void RotateSword(float direction)
    {
        if (BallCaught != null) return;

        TargetAngle += direction * 90f;
    }

    private void FixedUpdate()
    {
        float velocity = 0f;
        if (MoveDirection != Vector3.zero)
        {
            _currentSpeedProcess = Mathf.SmoothDamp(_currentSpeedProcess, 1f, ref velocity, 0.05f);
            transform.position += MoveDirection * (moveSpeed * _currentSpeedProcess * Time.deltaTime);
            LastMoveDirection = MoveDirection;
        }
        else
        {
            _currentSpeedProcess = Mathf.SmoothDamp(_currentSpeedProcess, 0f, ref velocity, 0.025f);
            transform.position += LastMoveDirection * (moveSpeed * _currentSpeedProcess * Time.deltaTime);
        }

        if (BallCaught != null)
        {
            BallCaught.MovePosition(transform.position + RelativePosition);
        }


        if (TargetAngle < 0) TargetAngle += 360f; 
        if (TargetAngle > 360f) TargetAngle -= 360f; 
        //Debug.Log(Quaternion.Euler(0,0,TargetAngle).eulerAngles);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, TargetAngle), 0.9f);
    }
    
}