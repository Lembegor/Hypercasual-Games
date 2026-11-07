using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "BallConfig", menuName = "Game/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [field: Header("Size")]
        [field: SerializeField] public float StartSize { get; private set; } = 2.3f;
        [field: SerializeField] public float CriticalSize { get; private set; } = 0.5f;
        [field: SerializeField] public float PathWidthPerSize { get; private set; } = 1.25f;

        [field: Header("Charging")]
        [field: SerializeField] public float MinShotSize { get; private set; } = 0.15f;
        [field: SerializeField] public float ChargeSpeed { get; private set; } = 0.5f;

        [field: Header("Shot")]
        [field: SerializeField] public float ShotSpeed { get; private set; } = 10f;
        [field: SerializeField] public float ShotLifetime { get; private set; } = 4f;

        [field: Header("Infection")]
        [field: SerializeField] public float InfectionRadiusPerSize { get; private set; } = 8f;
        [field: SerializeField] public float InfectionSpeed { get; private set; } = 8f;

        [field: Header("Movement")]
        [field: SerializeField] public float HopLength { get; private set; } = 1.5f;
        [field: SerializeField] public float HopDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float HopHeight { get; private set; } = 0.6f;
        [field: SerializeField] public float MinHopLength { get; private set; } = 0.25f;
    }
}
