using System;
using System.Collections.Generic;

namespace AutoSuggest
{
    public interface ISuggestionProvider
    {
        IEnumerable<Suggestion> GetSuggestions(string currentValue, bool isFocused);

        event Action SuggestionsChanged;
    }
}
