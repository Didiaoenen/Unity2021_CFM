using System;
using System.Linq;
using System.Collections.Generic;

namespace AutoSuggest
{
    public class FixedListSuggestionProvider : ISuggestionProvider
    {
        private IReadOnlyList<string> _options;
        private readonly bool _warnForUnknownValues;

        public event Action SuggestionsChanged;

        public FixedListSuggestionProvider(IReadOnlyList<string> options, bool warnForUnknownValues)
        {
            _options = options;
            _warnForUnknownValues = warnForUnknownValues;
        }

        public IEnumerable<Suggestion> GetSuggestions(string currentValue, bool isFocused)
        {
            var errors = new List<Suggestion>();
            var suggestions = Enumerable.Empty<Suggestion>();

            var optionsWithInfo = _options.Select(s => new Suggestion(s, s, s.IndexOf(currentValue, StringComparison.CurrentCultureIgnoreCase), currentValue.Length));
            optionsWithInfo = optionsWithInfo.Where(opt => opt.DisplayTextMatchIndex >= 0);
            suggestions = optionsWithInfo.OrderBy(opt => opt.DisplayTextMatchIndex)
                .ThenBy(opt => opt.Value.Length)
                .ThenBy(opt => opt.Value);

            if (_warnForUnknownValues)
            {
                if (isFocused)
                {
                    if (!suggestions.Any())
                    {
                        errors.Add(new Suggestion($""));
                    }
                }
                else
                {
                    if (!suggestions.Any(s => s.Value == currentValue))
                    {
                        errors.Add(new Suggestion($""));
                    }
                }
            }

            if (!isFocused || (suggestions.Count() == 1 && suggestions.First().Value == currentValue))
            {
                suggestions = Enumerable.Empty<Suggestion>();
            }

            return errors.Concat(suggestions);
        }

        public void SetOptions(IReadOnlyList<string> options)
        {
            if (!_options.SequenceEqual(options))
            {
                _options = options;
                SuggestionsChanged?.Invoke();
            }
        }
    }
}
