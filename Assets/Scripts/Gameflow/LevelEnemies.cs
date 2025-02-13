using System;
using UnityEngine;

namespace Gameflow
{
    [CreateAssetMenu(fileName = "Level", menuName = "Parameters/LevelParameters")]
    public class LevelEnemies : ScriptableObject
    {
        [field: SerializeField] public RandomizedEnemy[] Enemies;
        [Serializable]
        public struct RandomizedEnemy
        {
            public Vector2Int size;
            public Vector2Int life;
            public Vector2 timeToNext;
        }
    }
}
