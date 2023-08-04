using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UIElements;

namespace Galamania
{
    public abstract class UIBehaviour : MonoBehaviour
    {
        public const string SelectorType = @"\*\.#", SelectorName = @"A-Za-z0-9-_";
        
        public VisualElement Root => GetComponent<UIDocument>().rootVisualElement;
        
        public List<VisualElement> Find(string query)
        {
            List<VisualElement> results = new();

            foreach (string subquery in query.Split(','))
            {
                MatchCollection matches = Regex.Matches(subquery, $"([{SelectorType}{SelectorName}]+)");

                string Selector(int i) => subquery[matches[i].Index..(matches[i].Index + matches[i].Length)];
                char Relation(int i) => i > 0 ? Trim(subquery[(matches[i - 1].Index + matches[i - 1].Length)..matches[i].Index]) : ' ';
                static char Trim(string selector) => selector.Contains('>') ? '>' : (selector.Contains(',') ? ',' : ' ');

                IEnumerable<VisualElement> subresults = new List<VisualElement>() { Root };

                for (int i = 0; i < matches.Count; i++)
                    subresults = FindChildren(subresults, Selector(i), Relation(i) != '>');

                results.AddRange(subresults);
            }

            return results;
        }

        public static IEnumerable<VisualElement> FindChildren(IEnumerable<VisualElement> elements, string selector, bool deep)
        {
            List<VisualElement> results = new();

            foreach (VisualElement element in elements)
            {
                results.AddRange(element.Children().Where(child => IsMatch(child, selector)));
                if (deep) results.AddRange(FindChildren(element.Children(), selector, deep));
            }

            return results;
        }

        public static bool IsMatch(VisualElement element, string selector)
        {
            if (selector == "*") return true;
            
            foreach (string subselector in Regex.Split(selector, $"([{SelectorType}][{SelectorName}]+)"))
            {
                if (subselector.Length == 0) continue;
                else if (subselector.StartsWith('#') && element.name != subselector[1..]) return false;
                else if (subselector.StartsWith('.') && !element.ClassListContains(subselector[1..])) return false;
                else if (element.GetType().Name != subselector) return false;
            }

            return true;
        }
    }
}
