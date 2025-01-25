using UniRx;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Rigidbody _rigidbody;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private PlayerAnimator _animator;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 _moveDirection;
    
    private void Start()
    {
        Observable
            .EveryFixedUpdate()
            .Where(_=>_animator.IsAttack.Value == false)
            .Subscribe(_ =>
            {
                _rigidbody.AddForce(_moveDirection,ForceMode.Acceleration);
                _rigidbody.velocity = _moveDirection;
            })
            .AddTo(this.gameObject);

        _animator
            .IsAttack
            .DistinctUntilChanged()
            .Where(x => x == true)
            .Subscribe(_=>_rigidbody.velocity = Vector3.zero)
            .AddTo(this.gameObject);
    }

    public void Move(Vector3 direction)
    {
        _moveDirection = direction;
    }
}
