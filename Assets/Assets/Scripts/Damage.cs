using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
    Dog,
    Proff
    }

    public EnemyType enemyType;

    // Example: when dog bites
    public void OnAttackPlayer()
    {
        if (enemyType == EnemyType.Dog)
            ScoreManager.Instance.DogBite();
    }

    // Example: when exploder dies
    public void OnExplode()
    {
        if (enemyType == EnemyType.Proff)
            ScoreManager.Instance.Explosion();
    }
}
