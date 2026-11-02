using UnityEngine;

public class SpiritTower : Tower
{
    [SerializeField] private float teleportCooldown = 5f;
    [SerializeField] private int teleportRange = 3;
    [SerializeField] private float rageDamageMultiplier = 2f;

    [SerializeField] private Color rageColor = new Color(1f, 0.3f, 0.3f);

    private float lastTeleport = -999f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        spriteRenderer.color =
            (HomeBase.Instance != null && HomeBase.Instance.RecentlyAttacked)
                ? rageColor
                : BaseColor;
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (Time.time - lastTeleport > teleportCooldown && HomeBase.Instance != null)
            TryRelocate();
    }

    protected override float CurrentDamage()
    {
        bool baseUnderSiege = HomeBase.Instance != null && HomeBase.Instance.RecentlyAttacked;
        return base.CurrentDamage() * (baseUnderSiege ? rageDamageMultiplier : 1f);
    }

    void TryRelocate()
    {
        Vector3Int? target = TowerPlacer.Instance.FindFreeTileNear(CurrentCell, teleportRange);
        if (target == null) return;
        lastTeleport = Time.time;
        TowerPlacer.Instance.MoveTower(CurrentCell, target.Value);
    }

}