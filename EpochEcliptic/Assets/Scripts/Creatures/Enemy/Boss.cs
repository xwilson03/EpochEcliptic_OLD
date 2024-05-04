
public class Boss : Enemy {

    public string title = "";

    protected override void Awake() {
        base.Awake();

        if (title == "") Util.Error(name, "Missing Title.");

        Refs.boss = this;
        Refs.healthOverlay.ShowBossHealth();
    }

    public override void Damage(int amount) {
        base.Damage(amount);
        Refs.healthOverlay.RefreshBoss();
    }

    protected override void Die() {
        Refs.healthOverlay.HideBossHealth();
        transform.parent.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        base.Die();
    }
}