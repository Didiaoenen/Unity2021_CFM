using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoSuggest;

public class TypeSuggestion : Suggestion
{
    public Type Type { get; }

    public TypeSuggestion(string errorMessage)
        : base(errorMessage)
    { }

    public TypeSuggestion(Type type, string searchString)
        : base(type.FullName, type.FullName, type.FullName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase), searchString.Length)
    {
        Type = type;
    }
}

public class TypeSuggestionProvider : AsyncSuggestionProvider
{
    private IEnumerable<Type> _types;
    public Type SelectedType { get; private set; } = null;
    public bool SelectedTypeIsValid { get; protected set; } = false;
    public override async Task<IEnumerable<Suggestion>> GetSuggestionsAsync(string currentValue, bool isFocused, CancellationToken cancellationToken)
    {
        IList<TypeSuggestion> results = null;
        SelectedTypeIsValid = false;
        SelectedType = null;

        await Task.Run(() =>
        {
            if (_types == null)
            {
                var typeQuery = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch (Exception)
                    {
                        return new Type[] { };
                    }
                });
                _types = typeQuery.ToList();
            }

            results = _types
                .AsParallel()
                .WithCancellation(cancellationToken)
                .Where((t) => Attribute.GetCustomAttribute(t, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)) == null)
                .Select((t) => new TypeSuggestion(t, currentValue))
                .Where((s) => s.DisplayTextMatchIndex > 0)
                .OrderBy(opt => opt.DisplayTextMatchIndex)
                .ThenBy(opt => opt.Value.Length)
                .ThenBy(opt => opt.Value)
                .ToList();

            cancellationToken.ThrowIfCancellationRequested();

            if (results.Any() && results[0].Value == currentValue)
            {
                SelectedType = results[0].Type;
            }
            else
            {
                SelectedType = null;
            }

            SelectedTypeIsValid = true;

            if (isFocused)
            {
                if (!results.Any())
                {
                    results.Add(new TypeSuggestion($""));
                }
            }
            else
            {
                results.Clear();

                if (!string.IsNullOrEmpty(currentValue) && SelectedType == null)
                {
                    results.Add(new TypeSuggestion($""));
                }
            }
         }).ConfigureAwait(false);

        return results;
    }
}
