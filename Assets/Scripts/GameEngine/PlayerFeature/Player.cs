using UnityEngine;

namespace GameEngine.PlayerFeature
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float _forwardSpeed = 2;
        [SerializeField]
        private float _rotationSpeed = 90;
        [SerializeField]
        private float _jumpForce = 5;
        private Vector2 _input;
        private float _rotationInput;
        private bool _jumpInput;
        private Vector3 _forces;
    
        private Animator _animator;
        private CharacterController _controller;
    
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<CharacterController>();
        }
    
        private void Update()
        {
            _input = new Vector2(0, Input.GetAxis("Vertical"));
            _rotationInput = Input.GetAxis("Horizontal");
            _jumpInput = Input.GetKey(KeyCode.Space);
    
            _animator.SetFloat("Movement", _controller.velocity.magnitude / _forwardSpeed);
            _animator.SetInteger("Air", _controller.isGrounded ? 0 : (_forces.y >= 0 ? 1 : 2));
        }
    
        private void FixedUpdate()
        {
            Vector3 movement = transform.forward * _input.y * Time.deltaTime * _forwardSpeed;
            Vector3 gravity = Vector3.up * Physics.gravity.y * Time.deltaTime;
            _forces += gravity;
    
            if (_controller.isGrounded)
            {
                if (_jumpInput)
                {
                    _forces.y = _jumpForce;
                }
                else
                {
                    _forces.y = Physics.gravity.y * .5f;
                }
            }
    
            _controller.Move(movement + (_forces * Time.deltaTime));
            transform.Rotate(Vector3.up * _rotationInput * Time.deltaTime * _rotationSpeed);
        }
    }
}