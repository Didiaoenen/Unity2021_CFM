using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoSuggest
{
    public class AutoSuggestField
    {
        public class Options
        {
            public DisplayMode DisplayMode { get; set; } = DisplayMode.Inline;
            public int MaxSuggestionsToDisplay { get; set; } = 7;
        }

        private const int _scrollDistance = 3;
        private const string _keyFieldNamePrefix = "AutoSuggestField";
        private static int _controlCount = 0;

        private static GUIStyle _dropDownBoxStyle = null;
        private static GUIStyle _itemStyle = null;
        private static GUIStyle _selectedItemStyle = null;
        private static Vector2 _scrollbarSize;

        private List<Suggestion> _cachedSuggestions = new List<Suggestion>();
        private string _textForCachedSuggestions = null;
        private bool _focusedForCachedSuggestions = false;
        private bool _cacheInvalid = false;
        private readonly object _cacheInvalidationLock = new object();
        private ISuggestionProvider _suggestionProvider;
        private readonly GUIContent _label;
        private ValueAnimator _heightAnimator = new ValueAnimator(0.0f, 0.5f);
        private DrawSpaceClaimer _drawSpaceClaimer;
        private Options _options;
        private int _selectedIndex = 0;
        private int _scrolledIndex = 0;
        private bool _isFocused = false;
        private bool _setSelectedSuggestionToTextField = false;
        private readonly int _controlId;
        private Rect _textFieldPosition;
        private bool _prevRenderWasFirstPass = false;
        private bool _haveWarnedAboutRenderPassError = false;

        private bool ThreadSafeCacheInvalid
        {
            get
            {
                lock (_cacheInvalidationLock)
                {
                    return _cacheInvalid;
                }
            }
            set
            {
                lock (_cacheInvalidationLock)
                {
                    _cacheInvalid = value;
                }
            }
        }

        public AutoSuggestField(ISuggestionProvider suggestionProvider, GUIContent label, Options options)
        {
            _suggestionProvider = suggestionProvider;
            _label = label;
            _options = options;
            _drawSpaceClaimer = new DrawSpaceClaimer(_options.DisplayMode);

            _suggestionProvider.SuggestionsChanged += SuggestionProvider_SuggestionsChanged;
            EditorApplication.update += EditorApplication_Update;

            _controlId = _controlCount++;
        }

        public string OnGUI(string text)
        {
            const bool isFirstRenderPass = true;
            EnforceRenderPassOrdering(isFirstRenderPass);

            if (Event.current.type == EventType.KeyDown && _isFocused && _cachedSuggestions.Any())
            {
                if (Event.current.keyCode == KeyCode.Return)
                {
                    SetCurrentSelectedIndexToTextField();
                }
                else
                {
                    OnKeyPressed();
                }
            }

            string controlName = _keyFieldNamePrefix + _controlId;

            if (_setSelectedSuggestionToTextField && _cachedSuggestions.Any())
            {
                text = _cachedSuggestions[_selectedIndex].Value;
            }

            string newText;
            using (var horizontalScope = new GUILayout.HorizontalScope())
            {
                GUI.SetNextControlName(controlName);
                newText = EditorGUILayout.TextField(_label, text);
            }

            if (Event.current.type == EventType.Repaint)
            {
                _isFocused = (GUI.GetNameOfFocusedControl() == controlName);
            }

            if (_setSelectedSuggestionToTextField)
            {
                GUI.FocusControl(controlName);
                _isFocused = true;
                _setSelectedSuggestionToTextField = false;
            }

            if (newText != _textForCachedSuggestions || _isFocused != _focusedForCachedSuggestions || ThreadSafeCacheInvalid)
            {
                if (Event.current.type == EventType.Layout)
                {
                    _textForCachedSuggestions = newText;
                    _focusedForCachedSuggestions = _isFocused;
                    ThreadSafeCacheInvalid = false;

                    var suggestions = _suggestionProvider.GetSuggestions(newText, _isFocused);
                    _cachedSuggestions = (suggestions != null) ? suggestions.ToList() : new List<Suggestion>();
                    _selectedIndex = 0;
                    _scrolledIndex = 0;
                }
                else
                {
                    EditorWindow.focusedWindow?.Repaint();
                }
            }

            _textFieldPosition = EditorGUILayout.GetControlRect(false, 0);

            DrawAutoSuggestionOverlay(_textFieldPosition, isFirstRenderPass);

            return newText;
        }

        public void OnGUISecondPass()
        {
            const bool isFirstRenderPass = false;
            EnforceRenderPassOrdering(isFirstRenderPass);

            if (_options.DisplayMode == DisplayMode.Overlay)
            {
                DrawAutoSuggestionOverlay(_textFieldPosition, isFirstRenderPass);
            }
        }

        private void DrawAutoSuggestionOverlay(Rect textFieldPosition, bool isFirstRenderPass)
        {
            CreateStylesIfNeeded();

            var suggestionButtonHeight = EditorGUIUtility.singleLineHeight;
            var suggestionButtonHeightWithSpacing = suggestionButtonHeight + EditorGUIUtility.standardVerticalSpacing;
            var errorHeight = 2.0f * EditorGUIUtility.singleLineHeight;
            var errorHeightWithSpacing = errorHeight + EditorGUIUtility.standardVerticalSpacing;

            int numberToDisplay = Math.Min(_cachedSuggestions.Count, _options.MaxSuggestionsToDisplay);

            float height = (_cachedSuggestions.Any()) ? EditorGUIUtility.standardVerticalSpacing : 0.0f;
            for (int i = 0; i < numberToDisplay; i++)
            {
                if (_cachedSuggestions[i].IsErrorMessage)
                {
                    height += errorHeightWithSpacing;
                }
                else
                {
                    height += suggestionButtonHeightWithSpacing;
                }
            }

            Rect currentPosition = new Rect(
                x: textFieldPosition.x + EditorGUIUtility.labelWidth,
                y: textFieldPosition.y - EditorGUIUtility.standardVerticalSpacing,
                width: Math.Max(0, textFieldPosition.width - EditorGUIUtility.labelWidth),
                height: height);

            if (Event.current.isScrollWheel && currentPosition.Contains(Event.current.mousePosition))
            {
                if (Event.current.delta.y > 0)
                {
                    _scrolledIndex += _scrollDistance;
                }
                else
                {
                    _scrolledIndex -= _scrollDistance;
                }

                ClampScrolledIndex(false);
                Event.current.Use();
            }

            if (Event.current.type == EventType.Repaint)
            {
                _heightAnimator.Target = currentPosition.height;
            }

            _drawSpaceClaimer.ClaimDrawSpace(isFirstRenderPass, _heightAnimator.Current);
            currentPosition.height = _heightAnimator.Current;

            if (currentPosition.height > _scrollbarSize.y * 2 && _cachedSuggestions.Count > _options.MaxSuggestionsToDisplay)
            {
                var barPosition = new Rect(
                    currentPosition.xMax - _scrollbarSize.x,
                    currentPosition.y,
                    _scrollbarSize.x,
                    currentPosition.height);

                EditorGUIUtility.AddCursorRect(barPosition, MouseCursor.Arrow);

                float scrollMin = 0.0f;
                float scrollMax = _cachedSuggestions.Count;

                _scrolledIndex = (int)GUI.VerticalScrollbar(barPosition, _scrolledIndex, _options.MaxSuggestionsToDisplay, scrollMin, scrollMax, GUI.skin.verticalScrollbar);
                ClampScrolledIndex(false);
                currentPosition.width -= _scrollbarSize.x;
            }

            using (var scrollScope = new GUI.ScrollViewScope(currentPosition, Vector2.zero, currentPosition, false, false, GUIStyle.none, GUIStyle.none))
            {
                GUI.Box(currentPosition, string.Empty, _dropDownBoxStyle);
                scrollScope.handleScrollWheel = true;
                EditorGUIUtility.AddCursorRect(currentPosition, MouseCursor.Arrow);

                currentPosition.y += EditorGUIUtility.standardVerticalSpacing;
                currentPosition.height = suggestionButtonHeight;
                int maxRemainingSuggestionsToDisplay = _options.MaxSuggestionsToDisplay;

                for (int i = _scrolledIndex; i < _cachedSuggestions.Count; i++)
                {
                    var suggestion = _cachedSuggestions[i];
                    var style = _itemStyle;

                    if (i == _selectedIndex)
                    {
                        style = _selectedItemStyle;
                    }

                    if (suggestion.IsErrorMessage)
                    {
                        currentPosition.height = errorHeight;

                        EditorGUI.HelpBox(currentPosition, suggestion.DisplayText, MessageType.Error);

                        currentPosition.y += errorHeightWithSpacing;
                    }
                    else
                    {
                        currentPosition.height = suggestionButtonHeight;

                        if (GUI.Button(currentPosition, suggestion.RichDisplayText, style))
                        {
                            _selectedIndex = i;
                            SetCurrentSelectedIndexToTextField();
                        }

                        currentPosition.y += suggestionButtonHeightWithSpacing;

                    }

                    if (--maxRemainingSuggestionsToDisplay == 0)
                    {
                        break;
                    }
                }
            }
        }

        private void SetCurrentSelectedIndexToTextField()
        {
            if (!_cachedSuggestions[_selectedIndex].IsErrorMessage)
            {
                _setSelectedSuggestionToTextField = true;
                GUI.FocusControl("");
            }
        }

        private void SuggestionProvider_SuggestionsChanged()
        {
            ThreadSafeCacheInvalid = true;
        }

        private void EditorApplication_Update()
        {
            if (_heightAnimator.Update() || ThreadSafeCacheInvalid)
            {
                EditorWindow.focusedWindow?.Repaint();
            }
        }

        private void OnKeyPressed()
        {
            var current = Event.current;

            if (current.keyCode == KeyCode.UpArrow)
            {
                _selectedIndex--;
            }
            else if (current.keyCode == KeyCode.DownArrow)
            {
                _selectedIndex++;
            }
            else
            {
                return;
            }

            current.Use();
            ClampSelectedIndex();
            ClampScrolledIndex(true);
        }

        private void ClampSelectedIndex()
        {
            _selectedIndex = MathUtils.Clamp(_selectedIndex, 0, _cachedSuggestions.Count() - 1);
        }

        private void ClampScrolledIndex(bool scrollSelectedItemIntoView)
        {
            if (scrollSelectedItemIntoView)
            {
                var maxScrolledIndex = _selectedIndex;
                var minScrolledIndex = _selectedIndex - (_options.MaxSuggestionsToDisplay - 1);
                _scrolledIndex = MathUtils.Clamp(_scrolledIndex, minScrolledIndex, maxScrolledIndex);
            }

            if (_cachedSuggestions.Count < _options.MaxSuggestionsToDisplay)
            {
                _scrolledIndex = 0;
            }
            else
            {
                _scrolledIndex = MathUtils.Clamp(_scrolledIndex, 0, _cachedSuggestions.Count - _options.MaxSuggestionsToDisplay);
            }
        }

        private void CreateStylesIfNeeded()
        {
            if (_dropDownBoxStyle == null)
            {
                _dropDownBoxStyle = GUI.skin.textField;
                _itemStyle = new GUIStyle(GUI.skin.label);
                _itemStyle.richText = true;

                _selectedItemStyle = new GUIStyle(_itemStyle);

                var selectedItemBackgroundTexture = new Texture2D(1, 1);
                selectedItemBackgroundTexture.SetPixel(0, 0, GUI.skin.settings.selectionColor);
                selectedItemBackgroundTexture.Apply();
                _selectedItemStyle.normal.background = selectedItemBackgroundTexture;

                _scrollbarSize = GUI.skin.verticalScrollbar.CalcSize(new GUIContent(""));
            }
        }

        private void EnforceRenderPassOrdering(bool isFirstRenderPass)
        {
            if (_haveWarnedAboutRenderPassError)
            {
                return;
            }

            if (_options.DisplayMode == DisplayMode.Overlay)
            {
                if (_prevRenderWasFirstPass == isFirstRenderPass)
                {
                    string currentPassName = (isFirstRenderPass) ? nameof(OnGUI) : nameof(OnGUISecondPass);
                    Debug.LogError($"When using AutoSuggestField in Overlay mode, you must call OnGUI, then render other controls in the pane, then call OnGUISecondPass.  " +
                        $"You have called {currentPassName} twice in a row.");
                    _haveWarnedAboutRenderPassError = true;
                }
            }
            else
            {
                if (!isFirstRenderPass)
                {
                    Debug.LogWarning("When using AutoSuggestField in Inline mode, there is no need to call OnGUISecondPass()");
                    _haveWarnedAboutRenderPassError = true;
                }
            }

            _prevRenderWasFirstPass = isFirstRenderPass;
        }
    }
}
