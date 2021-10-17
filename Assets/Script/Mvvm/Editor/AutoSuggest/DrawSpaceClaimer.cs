using UnityEditor;
using UnityEngine;

namespace AutoSuggest
{
    public class DrawSpaceClaimer
    {
        private readonly DisplayMode _displayMode;
        private float _drawLocation;
        private float _additionDrawSpoaceToClaim;
        private bool _hasShownError = false;
        private bool _hasRenderedFirstPass = false;
        private bool _hasRenderedSecondPass = false;

        public DrawSpaceClaimer(DisplayMode displayMode)
        {
            _displayMode = displayMode;
        }

        public void ClaimDrawSpace(bool isFirstRenderPass, float desiredHeight)
        {
            if (_displayMode == DisplayMode.Inline)
            {
                if (isFirstRenderPass)
                {
                    EditorGUILayout.GetControlRect(false, desiredHeight);
                }
            }
            else if (_displayMode == DisplayMode.Overlay)
            {
                if (isFirstRenderPass)
                {
                    if (_hasRenderedFirstPass && !_hasRenderedSecondPass && !_hasShownError)
                    {
                        _hasShownError = true;
                        Debug.LogError("");
                    }

                    _drawLocation = EditorGUILayout.GetControlRect(false, 0.0f).y;
                    _hasRenderedFirstPass = true;
                }
                else
                {
                    var currentDrawLocation = EditorGUILayout.GetControlRect(false, 0.0f).y;
                    var currentHeight = currentDrawLocation - _drawLocation;

                    bool isRealRenderPass = Event.current.type != EventType.Layout && Event.current.type != EventType.Used;

                    if (isFirstRenderPass && currentHeight < desiredHeight)
                    {
                        _additionDrawSpoaceToClaim = desiredHeight - currentHeight;
                    }

                    EditorGUILayout.GetControlRect(false, _additionDrawSpoaceToClaim);
                    _hasRenderedSecondPass = true;
                }
            }
        }
    }
}
