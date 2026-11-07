using UnityEngine;
using Zenject;

namespace Game.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private Obstacle _obstaclePrefab;
        [SerializeField] private Transform _obstaclesRoot;

        private LevelConfig _config;

        [Inject]
        private void Construct(LevelConfig config)
        {
            _config = config;
        }

        private void Start()
        {
            SpawnObstacles();
        }

        private void SpawnObstacles()
        {
            var random = new System.Random(_config.Seed);
            float spacing = _config.ObstacleSpacing;
            int rows = Mathf.FloorToInt(_config.FieldLength / spacing);
            int columns = Mathf.FloorToInt(_config.FieldWidth / spacing);
            float left = -_config.FieldWidth / 2f;

            for (int row = 0; row <= rows; row++)
            {
                float rowOffset = row % 2 == 0 ? 0f : spacing / 2f;

                for (int column = 0; column <= columns; column++)
                {
                    var position = new Vector3(left + column * spacing + rowOffset, 0f, row * spacing);
                    position.x += GetJitter(random);
                    position.z += GetJitter(random);

                    if (ShouldSpawn(position, random))
                        Instantiate(_obstaclePrefab, position, Quaternion.identity, _obstaclesRoot);
                }
            }
        }

        private float GetJitter(System.Random random)
        {
            return ((float)random.NextDouble() * 2f - 1f) * _config.ObstacleJitter;
        }

        private bool ShouldSpawn(Vector3 position, System.Random random)
        {
            Vector3 start = _config.StartPosition;
            Vector3 door = _config.DoorPosition;

            if (Vector3.Distance(position, start) < _config.ClearRadius)
                return false;

            if (Vector3.Distance(position, door) < _config.ClearRadius)
                return false;

            Vector3 path = door - start;
            float progress = Mathf.Clamp01(Vector3.Dot(position - start, path) / path.sqrMagnitude);
            float distanceToPath = Vector3.Distance(position, start + path * progress);

            if (distanceToPath > _config.PathHalfWidth)
                return true;

            return random.NextDouble() < _config.PathDensity.Evaluate(progress);
        }
    }
}
