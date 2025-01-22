using UniRx;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
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
   private Quaternion _targetRotation;
   
   private void Start()
   {
      _targetRotation = transform.rotation;
      
      _playerMover
         .OnMove
         .Subscribe(x=>
         {
            _animator.SetFloat("Speed", x.magnitude, 0.05f, Time.deltaTime);
            SetRotation(x);
         })
         .AddTo(this.gameObject);
   }

   private void SetRotation(Vector3 velocity)
   {
      if (velocity.magnitude > 0.5f)
      {
         _targetRotation = Quaternion.LookRotation(velocity,Vector3.up);  
      }
      
      var rotationSpeed = _rotationSpeed * Time.deltaTime;
      transform.rotation = Quaternion.RotateTowards(transform.rotation,_targetRotation,rotationSpeed);
   }
}
