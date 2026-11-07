using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [field: Header("Field")]
        [field: SerializeField] public float FieldWidth { get; private set; } = 16f;
        [field: SerializeField] public float FieldLength { get; private set; } = 22f;

        [field: Header("Path")]
        [field: SerializeField] public Vector3 StartPosition { get; private set; } = new(-3.5f, 0f, 1f);
        [field: SerializeField] public Vector3 DoorPosition { get; private set; } = new(3.5f, 0f, 21f);
        [field: SerializeField] public float DoorOpenDistance { get; private set; } = 5f;
        [field: SerializeField] public float PathHalfWidth { get; private set; } = 2f;
        [field: SerializeField] public float ClearRadius { get; private set; } = 1.8f;
        [field: SerializeField] public AnimationCurve PathDensity { get; private set; } = new(
            new Keyframe(0f, 1f),
            new Keyframe(0.35f, 1f),
            new Keyframe(0.5f, 0.25f),
            new Keyframe(0.65f, 0.25f),
            new Keyframe(0.8f, 1f),
            new Keyframe(1f, 1f));

        [field: Header("Obstacles")]
        [field: SerializeField] public int Seed { get; private set; } = 7;
        [field: SerializeField] public float ObstacleSpacing { get; private set; } = 1.3f;
        [field: SerializeField] public float ObstacleJitter { get; private set; } = 0.15f;
    }
}
