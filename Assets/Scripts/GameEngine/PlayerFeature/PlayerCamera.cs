using UnityEngine;

namespace GameEngine.PlayerFeature
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _target = default;
        [SerializeField]
        private Vector3 _offset = default;
        [SerializeField]
        private float _followSpeed = 5;
        [SerializeField]
        private float _rotationSpeed = 2;

        private Transform _parent;

        private void Start()
        {
            _parent = new GameObject(name).transform;
            _parent.position = transform.position;
            transform.SetParent(_parent);
        }

        private void LateUpdate()
        {
            Vector3 vel  = Vector3.zero;
            _parent.position = Vector3.SmoothDamp(_parent.position, _target.TransformPoint(_offset), ref vel, Time.deltaTime * _followSpeed);
            _parent.rotation = Quaternion.RotateTowards(_parent.rotation, Quaternion.LookRotation(_target.forward), _rotationSpeed * Time.deltaTime);
        }
    }
}