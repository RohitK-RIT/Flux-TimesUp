namespace _Project.Scripts.Core.Enemy.FSM
{
    public enum EnemyState 
    {
        // Represents the state where the enemy randomly moves around
        Patrol,
        // Represents the state where the enemy searches for the player within detection range
        Detect,
        
        // Represents the state where the enemy follows the player within a certain distance
        Chase,
        
        // Represents the state where the enemy attacks the player once in close range
        Attack,
        
        // Represents the state where the enemy moves away from the player when its health is low
        Flee,
        
        // Represents the state only for boss enemy
        BossAttack,
    }
}