using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UIElements;

namespace Galamania
{
    public abstract class UIBehaviour : MonoBehaviour
    {
        public const string SelectorRegex = @"([\*#\.A-Za-z0-9-_]+)";
        public VisualElement Root => GetComponent<UIDocument>().rootVisualElement;
        
        public List<VisualElement> Find(string query)
        {
            List<VisualElement> elements = new() { Root };
            MatchCollection matches = Regex.Matches(query, SelectorRegex);

            static char Trim(string selector) => selector.Contains('>') ? '>' : ' ';
            string Selector(int i) => query[matches[i].Index..(matches[i].Index + matches[i].Length)];
            char Relation(int i) => i > 0 ? Trim(query[(matches[i - 1].Index + matches[i - 1].Length)..matches[i].Index]) : ' ';

            for (int i = 0; i < matches.Count; i++)
            {
                elements = FindChildren(Selector(i), Relation(current) != '>');
            }

            return elements;
        }

        public static List<VisualElement> FindChildren(IEnumerable<VisualElement> elements, List<string> selectors, bool deep)
        {
            List<VisualElement> results = new();

            foreach (VisualElement child in elements)
            {
                if (child == null) continue;

                if (Match(child, selectors)) results.Add(child);
                if (deep) results.AddRange(FindChildren(child.Children(), selectors, deep, one));
            }

            return results;
        }

        public static bool Match(VisualElement element, string selector)
        {
            if (selector == "*") return true;
            if (element.GetType().Name == selector) return true;
            if (selector.StartsWith('#') && element.name == selector[1..]) return true;
            if (selector.StartsWith('.') && element.ClassListContains(selector[1..])) return true;

            return false;
        }
    }
}
