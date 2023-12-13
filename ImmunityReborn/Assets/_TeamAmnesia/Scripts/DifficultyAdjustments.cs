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
                ShieldCollisions.SetParryRates(12, 15, 3);
                ShieldController.SetShieldRates(1, 30, 10, 0.2f);
                // Summon up to 10 units, summon every 12 seconds, can 1-shot with memory attack immediately.
                EnemyPortal.DifficultyAdjustment(100, 10, 12);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 500;
                TormentedSoulStateMachine.BossHP = 500;
                MagicStateMachine.BossHP = 500;
                DragonStateMachine.BossHP = 1500;
                // Dragon specific adjustments
                DragonStateMachine.BomberCount = 2;
                break;
            case 1:
                // Player health adjustments 
                PlayerHealth.SetHealth(200);
                HealthConsumable.DifficultyAdjustment(100, 4);
                // Player memory attack and shield adjustments
                MemoryAttack.memAtkCost = 50;
                ShieldCollisions.SetParryRates(8, 12, 2);
                ShieldController.SetShieldRates(1.5f, 25, 15, 0.5f);
                // Summon up to 15 units, summon every 10 seconds, generally spawns 1 set before 1-shottable with memory attack.
                EnemyPortal.DifficultyAdjustment(150, 10, 10);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 800;
                TormentedSoulStateMachine.BossHP = 800;
                MagicStateMachine.BossHP = 800;
                DragonStateMachine.BossHP = 2000;
                // Dragon specific adjustments
                DragonStateMachine.BomberCount = 3;
                break;
            case 2:
                // Player health adjustments
                PlayerHealth.SetHealth(100);
                HealthConsumable.DifficultyAdjustment(50, 3);
                // Player memory attack and shield adjustments
                MemoryAttack.memAtkCost = 80;
                ShieldCollisions.SetParryRates(5, 10, 1);
                ShieldController.SetShieldRates(2, 20, 20, 1);
                // Summon up to 20 units, summon every 8 seconds, generally have to hit with memory attack twice to clear early.
                EnemyPortal.DifficultyAdjustment(200, 10, 8);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 1200;
                TormentedSoulStateMachine.BossHP = 1200;
                MagicStateMachine.BossHP = 1200;
                DragonStateMachine.BossHP = 2500;
                // Dragon specific adjustments
                DragonStateMachine.BomberCount = 4;
                break;
        }
    }
}
