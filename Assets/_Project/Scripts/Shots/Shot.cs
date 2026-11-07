using System;
using Game.Level;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Shots
{
    public class Shot : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private GameObject _visual;
        [SerializeField] private LayerMask _obstacleMask;

        private BallConfig _config;
        private Vector3 _velocity;

        public event Action Finished;

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

            _velocity = Vector3.zero;
            _collider.enabled = false;
            _visual.SetActive(false);

            Destroy(gameObject, InfectObstacles());
        }

        private void OnDestroy()
        {
            Finished?.Invoke();
        }

        public void Grow(float amount)
        {
            Size += amount;
            transform.localScale = Vector3.one * Size;
        }

        public void Launch(Vector3 direction, float pathWidth)
        {
            _velocity = direction * _config.ShotSpeed;
            _collider.radius = pathWidth / 2f / Size;
            _collider.enabled = true;
            Destroy(gameObject, _config.ShotLifetime);
        }

        private float InfectObstacles()
        {
            float radius = Size * _config.InfectionRadiusPerSize;
            float duration = 0f;

            foreach (Collider hit in Physics.OverlapSphere(transform.position, radius, _obstacleMask))
            {
                if (!hit.TryGetComponent(out Obstacle obstacle))
                    continue;

                float delay = Vector3.Distance(transform.position, hit.transform.position) / _config.InfectionSpeed;
                obstacle.Infect(delay);
                duration = Mathf.Max(duration, delay + obstacle.ExplodeDelay);
            }

            return duration;
        }

        public class Factory : PlaceholderFactory<Shot>
        {
        }
    }
}
