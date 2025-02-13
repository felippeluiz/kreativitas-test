using UnityEngine;

namespace Gameflow
{
    [CreateAssetMenu(fileName = "LevelList", menuName = "LevelList")]
    public class LevelList : ScriptableObject
    {
        [field: SerializeField] public LevelEnemies[] Levels;
    }
}
