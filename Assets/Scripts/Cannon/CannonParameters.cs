using UnityEngine;

namespace Cannon
{
    [CreateAssetMenu(fileName = "CannonParameters", menuName = "Parameters/CannonParameters")]
    public class CannonParameters : ScriptableObject
    {
        [field: SerializeField] public float ShotsPerSecond { get; private set; } = 3;
    }
}
