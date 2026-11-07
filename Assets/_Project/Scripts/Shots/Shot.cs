using Game.Level;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Shots
{
    public class Shot : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private LayerMask _obstacleMask;

        private BallConfig _config;
        private Vector3 _velocity;

        public float Size { get; private set; }

        [Inject]
        private void Construct(BallConfig config)
        {
            _config = config;
        }

        private void Update()
        {
            transform.position += _velocity * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Obstacle _))
                return;

            InfectObstacles();
            Destroy(gameObject);
        }

        public void Grow(float amount)
        {
            Size += amount;
            transform.localScale = Vector3.one * Size;
        }

        public void Launch(Vector3 direction)
        {
            _velocity = direction * _config.ShotSpeed;
            _collider.enabled = true;
            Destroy(gameObject, _config.ShotLifetime);
        }

        private void InfectObstacles()
        {
            float radius = Size * _config.InfectionRadiusPerSize;

            foreach (Collider hit in Physics.OverlapSphere(transform.position, radius, _obstacleMask))
            {
                if (!hit.TryGetComponent(out Obstacle obstacle))
                    continue;

                float distance = Vector3.Distance(transform.position, hit.transform.position);
                obstacle.Infect(distance / _config.InfectionSpeed);
            }
        }

        public class Factory : PlaceholderFactory<Shot>
        {
        }
    }
}
