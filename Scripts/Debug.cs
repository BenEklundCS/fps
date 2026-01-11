namespace CosmicDoom.Scripts;

using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Array = Godot.Collections.Array;

public partial class Debug : Control {
    [Export] public NodePath DebugPath;
    [Export] public bool DebugEnabled = false;
    
    private PropertyInfo[] _properties;
    private GridContainer _grid;
    private Dictionary<string, int> _indexDict = new ();

    public override void _Ready() {
        if (DebugEnabled) ReadyDebug();
    }

    public override void _Process(double delta) {
        if (DebugEnabled) ProcessDebug(delta);
    }

    private void ReadyDebug() {
        _grid = GetNode<GridContainer>("GridContainer");

        Print(GetNode(DebugPath).GetType());
        _properties = GetNode().GetType().GetProperties();
        
        for (var i = 0; i < _properties.Length; i++) {
            var name = _properties[i].Name;
            var label = GetLabel(name);
            _indexDict.Add(name, i);
            _grid.AddChild(label);
        }
    }

    private void ProcessDebug(double delta) {
        foreach (var property in _properties) {
            var name = property.Name;
            var value = property.GetGetMethod()
                ?.Invoke(GetNode(), null)
                ?.ToString();
            var newText = $"{name}, {value}";
            _grid.GetChild<Label>(_indexDict[property.Name]).Text = newText;
        }
    }

    private Node GetNode() {
        return GetNode(DebugPath);
    }

    private static Label GetLabel(string name) {
        var label = new Label();
        label.Name = name;
        label.AddThemeFontSizeOverride("font_size", 10);
        return label;
    }
}
