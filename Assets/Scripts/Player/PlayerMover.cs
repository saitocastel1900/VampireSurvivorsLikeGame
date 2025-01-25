using System;
using UniRx;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public IObservable<Vector3> OnMove => _moveSubject;
    private readonly Subject<Vector3> _moveSubject = new Subject<Vector3>();
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private PlayerCharacterController _characterController;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]private float _walkSpeed = 1f;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]private float _runSpeed = 2f;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private InputProvider _inputProvider;
    
    /// <summary>
    /// 
    /// </summary>
    private Vector3 _moveDirection;
    
    /// <summary>
    /// 
    /// </summary>
    private float _moveSpeed = 0f;
    
    private void Start()
    {
        _inputProvider
            .IsRun
            .Subscribe(isRun => _moveSpeed = isRun ? _runSpeed : _walkSpeed)
            .AddTo(this.gameObject);
        
        _inputProvider
            .MoveDirection
            .Subscribe(x=>_moveDirection = new Vector3(x.x,0,x.y))
            .AddTo(this.gameObject);

        Observable
            .EveryFixedUpdate()
            .Subscribe(_ =>
            {
                var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        
                var velocity = horizontalRotation * _moveDirection.normalized;
                
                _characterController.Move(velocity * _moveSpeed);
                _moveSubject.OnNext(velocity * _moveSpeed);
            })
            .AddTo(this.gameObject);
    }
}
