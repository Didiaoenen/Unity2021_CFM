using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AutoSuggest
{
    public abstract class AsyncSuggestionProvider : ISuggestionProvider
    {
        private CancellationTokenSource _cancelSource;
        private string _searchString;
        private bool _searchFocus;
        private IEnumerable<Suggestion> _results = Enumerable.Empty<Suggestion>();

        public event Action SuggestionsChanged;

        public IEnumerable<Suggestion> GetSuggestions(string currentValue, bool isFocused)
        {
            if (_searchString != currentValue || _searchFocus != isFocused)
            {
                _searchString = currentValue;
                _searchFocus = isFocused;

                GetSuggestionsAndUpdateResultsAsync(currentValue, isFocused);
            }

            return _results;
        }

        abstract public Task<IEnumerable<Suggestion>> GetSuggestionsAsync(string currentValue, bool isFocused, CancellationToken cancellationToken);

        private async void GetSuggestionsAndUpdateResultsAsync(string currentValue, bool isFocused)
        {
            if (_cancelSource != null)
            {
                _cancelSource.Cancel();
            }
            _cancelSource = new CancellationTokenSource();

            try
            {
                _results = await GetSuggestionsAsync(currentValue, isFocused, _cancelSource.Token).ConfigureAwait(false);

                SuggestionsChanged?.Invoke();
            }
            catch (OperationCanceledException)
            {

            }
        }
    }
}
