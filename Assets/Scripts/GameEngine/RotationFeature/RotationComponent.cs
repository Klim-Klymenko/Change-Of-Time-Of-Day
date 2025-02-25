using Application.GameCycleFeature;
using UnityEngine;

namespace GameEngine.RotationFeature
{
    public sealed class RotationComponent : MonoBehaviour, IUpdatable
    {
        private float _angle;
        
        private float _speed;
        private float _initialYRotation;
        
        public void Construct(float speed)
        {
            _speed = speed;
            _initialYRotation = transform.rotation.eulerAngles.y;
        }
        
        public void ResetRotation()
        {
            _angle = 0;
        }
        
        void IUpdatable.OnUpdate()
        {
            _angle -= _speed * Time.deltaTime;
            
            Vector3 rotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, _initialYRotation + _angle, rotation.z);
        }
    }
}