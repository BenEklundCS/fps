namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class HealthBar : Control {
    private ColorRect _fill;
    private Label _label;

    private const float BarWidth = 200f;
    private const float Margin = 20f;

    public override void _Ready() {
        _fill = GetNode<ColorRect>("Fill");
        _label = GetNode<Label>("Label");
    }

    public void SetHealth(int health, int maxHealth) {
        var ratio = Mathf.Clamp((float)health / maxHealth, 0f, 1f);

        _fill.OffsetRight = Margin + BarWidth * ratio;

        _fill.Color = ratio > 0.5f
            ? Colors.Green.Lerp(Colors.Yellow, (1f - ratio) * 2f)
            : Colors.Yellow.Lerp(Colors.Red, (0.5f - ratio) * 2f);

        _label.Text = health.ToString();
    }
}
