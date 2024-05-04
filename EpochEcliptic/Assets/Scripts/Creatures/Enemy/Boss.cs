
public class Boss : Enemy {

    public string title = "";

    protected override void Awake() {
        base.Awake();

        if (title == "") Util.Error(name, "Missing Title.");

        Refs.boss = this;
        Refs.bossOverlay.SetTitle(title);
        Refs.bossOverlay.Show();
    }

    public override void Damage(int amount) {
        base.Damage(amount);
        Refs.bossOverlay.RefreshHealth();
    }

    protected override void Die() {
        Refs.bossOverlay.Hide();
        transform.parent.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        base.Die();
    }
}