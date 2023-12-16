using UnityEngine;

public class DifficultyAdjustments : MonoBehaviour
{
    private GameObject Player;

    /* Difficulties:
     * 0 - Normal
     * 1 - Hard
     * 2 - Impossible
     */
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStateMachine PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();
        Health PlayerHealth = Player.GetComponent<Health>();
        HealthConsumable HealthConsumable = Player.GetComponent<HealthConsumable>();
        ShieldController ShieldController = Player.GetComponent<ShieldController>();
        switch (PlayerPrefs.GetInt("SelectedDifficulty"))
        {
            case 0:
                // Player health adjustments
                PlayerHealth.SetHealth(400);
                HealthConsumable.DifficultyAdjustment(400, 5);
                // Player memory attack and shield adjustments
                MemoryAttack.memAtkCost = 30;
                ShieldCollisions.SetParryRates(12, 15, 4);
                ShieldController.SetShieldRates(1, 30, 10, 0.3f);
                // Player mobility adjustment
                PlayerStateMachine.SetMovementSpeed(5f);
                // Summon up to 10 units, summon every 12 seconds, can 1-shot with memory attack immediately.
                EnemyPortal.DifficultyAdjustment(100, 10, 10);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 500;
                TormentedSoulStateMachine.BossHP = 500;
                MagicStateMachine.BossHP = 500;
                DragonStateMachine.BossHP = 1200;
                // Dragon specific adjustments
                DragonStateMachine.BomberCount = 2;
                break;
            case 1:
                // Player health adjustments 
                PlayerHealth.SetHealth(200);
                HealthConsumable.DifficultyAdjustment(150, 4);
                // Player memory attack and shield adjustments
                MemoryAttack.memAtkCost = 50;
                ShieldCollisions.SetParryRates(8, 12, 3);
                ShieldController.SetShieldRates(1.5f, 25, 15, 0.6f);
                // Player mobility adjustment
                PlayerStateMachine.SetMovementSpeed(4.5f);
                // Summon up to 14 units, summon every 10 seconds.
                EnemyPortal.DifficultyAdjustment(140, 10, 6);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 700;
                TormentedSoulStateMachine.BossHP = 700;
                MagicStateMachine.BossHP = 700;
                DragonStateMachine.BossHP = 1600;
                // Dragon specific adjustments
                DragonStateMachine.BomberCount = 3;
                break;
            case 2:
                // Player health adjustments
                PlayerHealth.SetHealth(100);
                HealthConsumable.DifficultyAdjustment(50, 3);
                // Player memory attack and shield adjustments
                MemoryAttack.memAtkCost = 80;
                ShieldCollisions.SetParryRates(5, 10, 2);
                ShieldController.SetShieldRates(2, 20, 20, 1);
                // Player mobility adjustment
                PlayerStateMachine.SetMovementSpeed(4f);
                // Summon up to 18 units, summon every 7 seconds.
                EnemyPortal.DifficultyAdjustment(180, 10, 6);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 1000;
                TormentedSoulStateMachine.BossHP = 1000;
                MagicStateMachine.BossHP = 1000;
                DragonStateMachine.BossHP = 2000;
                // Dragon specific adjustments
                DragonStateMachine.BomberCount = 4;
                break;
        }
    }
}
