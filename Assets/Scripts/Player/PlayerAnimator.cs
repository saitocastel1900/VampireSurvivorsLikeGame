using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<bool> IsAttack => _isAttack;
    private readonly BoolReactiveProperty _isAttack = new BoolReactiveProperty();

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Animator _animator;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private PlayerMover _playerMover;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float _rotationSpeed = 600f;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private InputProvider _inputProvider;

    /// <summary>
    /// 
    /// </summary>
    private Quaternion _targetRotation;

    private void Start()
    {
        _targetRotation = transform.rotation;

        _playerMover
            .OnMove
            .Subscribe(x =>
            {
                _animator.SetFloat("Speed", x.magnitude, 0.05f, Time.deltaTime);

                if (_isAttack.Value == false)
                {
                    SetRotation(x);
                }
            })
            .AddTo(this.gameObject);

        _inputProvider
            .OnAttack
            .Subscribe(_ =>
            {
                _animator.SetInteger("AttackType", 0);
                _animator.SetTrigger("AttackTrigger");
            })
            .AddTo(this.gameObject);

        ObservableStateMachineTrigger trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();

        trigger
            .OnStateEnterAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;

                if (info.IsName("AttackStateMachine.WGS_attackA1"))
                {
                    _isAttack.Value = true;
                }

                if (info.IsName("WalkBlendTree"))
                {
                    _isAttack.Value = false;
                }
            }).AddTo(this);
    }

    private void SetRotation(Vector3 velocity)
    {
        if (velocity.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        var rotationSpeed = _rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed);
    }
}