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
                PlayerHealth.SetHealth(400);
                MemoryAttack.memAtkCost = 30;
                ShieldController.SetShieldRates(1, 30, 30);
                HealthConsumable.DifficultyAdjustment(400, 5);
                // Summon up to 4 sets every 12 seconds, can 1-shot with memory attack immediately.
                EnemyPortal.DifficultyAdjustment(100, 25, 12);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 500;
                TormentedSoulStateMachine.BossHP = 500;
                MagicStateMachine.BossHP = 500;
                DragonStateMachine.BossHP = 1000;
                break;
            case 1:
                PlayerHealth.SetHealth(200);
                MemoryAttack.memAtkCost = 50;
                ShieldController.SetShieldRates(1.5f, 20, 30);
                HealthConsumable.DifficultyAdjustment(100, 4);
                // Summon up to 6 sets every 10 seconds, generally spawns 1 set before 1-shottable with memory attack.
                EnemyPortal.DifficultyAdjustment(120, 20, 10);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 800;
                TormentedSoulStateMachine.BossHP = 800;
                MagicStateMachine.BossHP = 800;
                DragonStateMachine.BossHP = 1600;
                break;
            case 2:
                PlayerHealth.SetHealth(100);
                MemoryAttack.memAtkCost = 100;
                ShieldController.SetShieldRates(2, 20, 20);
                HealthConsumable.DifficultyAdjustment(50, 3);
                // Summon up to 8 sets every 8 seconds, generally have to hit with memory attack twice to clear early.
                EnemyPortal.DifficultyAdjustment(160, 20, 8);
                // Boss health adjustments
                MeleeStateMachine.BossHP = 1200;
                TormentedSoulStateMachine.BossHP = 1200;
                MagicStateMachine.BossHP = 1200;
                DragonStateMachine.BossHP = 2400;
                break;
        }
    }
}
