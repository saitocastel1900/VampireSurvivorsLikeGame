using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

public class PlayerCharacterController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Rigidbody _rigidbody;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 _moveDirection;
    
    private void Start()
    {
        Observable
            .EveryFixedUpdate()
            .Subscribe(_ =>
            {
                _rigidbody.AddForce(_moveDirection,ForceMode.Acceleration);
                _rigidbody.velocity = _moveDirection;
            })
            .AddTo(this.gameObject);
    }

    public void Move(Vector3 direction)
    {
        _moveDirection = direction;
    }
}
