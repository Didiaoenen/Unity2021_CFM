using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/Tabs/TabItem", 2)]
public class TabItem : Button
{
    private TabControl _tabs;
    private Image _image;

    [SerializeField]
    private Sprite _active;
    [SerializeField]
    private Color _activeColor = Color.white;

    private bool _isOnTab;

    protected override void Awake()
    {
        _image = GetComponent<Image>();
        _tabs = GetComponentInParent<TabControl>();
        if (_active == null)
            _active = spriteState.highlightedSprite;

        onClick.AddListener(OnClick);
    }

    protected override void OnTransformParentChanged()
    {
        base.OnTransformParentChanged();
        _tabs = GetComponentInParent<TabControl>();
    }

    private void OnClick()
    {
        _tabs.SelectTab(this);
        SetSelected(true);
    }

    public void SetSelected(bool state)
    {
        if (_image == null) return;

        _isOnTab = state;
        switch (transition)
        {
            case Transition.ColorTint:
                if (state)
                    _image.CrossFadeColor(_activeColor, 0, true, true);
                else
                    _image.CrossFadeColor(colors.normalColor, 0, true, true);
                break;
            default:
                _image.overrideSprite = state ? _active : null;
                break;
        }
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (transition == Transition.ColorTint)
        {
            if (state == SelectionState.Normal && _isOnTab)
                SetSelected(_isOnTab);
            if (state == SelectionState.Highlighted && !_isOnTab)
                SetSelected(_isOnTab);
        }
        else
            SetSelected(_isOnTab);
    }

    protected override void InstantClearState()
    {
        base.InstantClearState();
        SetSelected(_isOnTab);
    }
}
