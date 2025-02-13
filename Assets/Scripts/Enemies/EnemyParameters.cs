using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameters", menuName = "Parameters/EnemyParameters")]
public class EnemyParameters : ScriptableObject
{
    [field: SerializeField] public EnemySizes[] SizesParameters;
    [field: SerializeField] public Color LowLifeColor = Color.green; // Cor para vida baixa
    [field: SerializeField] public Color HighLifeColor = Color.red;
    [field: SerializeField] public Vector2 Speed = Vector2.one;
    [field: SerializeField] public float FloorY;
    [field: SerializeField] public AnimationCurve BounceCurve;
    [field: SerializeField] public float Size1BounceTime=2f;
    
    
    [Serializable]
    public struct EnemySizes
    {
        public float scale;
        public float jumpYHeight;
    }
}
