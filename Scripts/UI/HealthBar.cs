namespace CosmicDoom.Scripts.UI;

using Godot;

public partial class HealthBar : Control {
    private ColorRect _background;
    private ColorRect _fill;
    private Label _label;

    private const float BarWidth = 200f;
    private const float BarHeight = 24f;
    private const float Margin = 20f;

    public override void _Ready() {
        SetAnchorsPreset(LayoutPreset.FullRect);
        MouseFilter = MouseFilterEnum.Ignore;

        _background = new ColorRect();
        _background.OffsetLeft = Margin;
        _background.OffsetTop = Margin;
        _background.OffsetRight = Margin + BarWidth;
        _background.OffsetBottom = Margin + BarHeight;
        _background.Color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
        _background.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(_background);

        _fill = new ColorRect();
        _fill.OffsetLeft = Margin;
        _fill.OffsetTop = Margin;
        _fill.OffsetRight = Margin + BarWidth;
        _fill.OffsetBottom = Margin + BarHeight;
        _fill.Color = Colors.Green;
        _fill.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(_fill);

        _label = new Label();
        _label.OffsetLeft = Margin;
        _label.OffsetTop = Margin;
        _label.OffsetRight = Margin + BarWidth;
        _label.OffsetBottom = Margin + BarHeight;
        _label.HorizontalAlignment = HorizontalAlignment.Center;
        _label.VerticalAlignment = VerticalAlignment.Center;
        _label.MouseFilter = MouseFilterEnum.Ignore;
        _label.LabelSettings = new LabelSettings {
            OutlineSize = 2,
            OutlineColor = Colors.Black
        };
        AddChild(_label);
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
