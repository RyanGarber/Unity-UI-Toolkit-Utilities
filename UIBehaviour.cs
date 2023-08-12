using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UIElements;

namespace Galamania
{
    public abstract class UIBehaviour : MonoBehaviour
    {
        private UIElements? _root = null;
        public UIElements Root => _root ??= new(gameObject.GetComponent<UIDocument>().rootVisualElement);
    }

    public struct UIElements
    {
        public const string SelectorType = @"\*\.#", SelectorName = @"A-Za-z0-9-_";

        private List<VisualElement> _elements;
        public UIElements(params VisualElement[] elements) => _elements = elements.ToList();

        public UIElements Query(string query)
        {
            UIElements result = new();

            foreach (string subquery in query.Split(','))
            {
                MatchCollection matches = Regex.Matches(subquery, $"([{SelectorType}{SelectorName}]+)");

                string Selector(int i) => subquery[matches[i].Index..(matches[i].Index + matches[i].Length)];
                bool Descendents(int i) => i == 0 || !subquery[(matches[i - 1].Index + matches[i - 1].Length)..matches[i].Index].Contains('>');

                result._elements = _elements;
                for (int i = 0; i < matches.Count; i++)
                    result._elements = FindAllMatchingSelector(result._elements, Selector(i), Descendents(i));
            }

            return result;
        }

        public static List<VisualElement> FindAllMatchingSelector(IEnumerable<VisualElement> elements, string selector, bool descendents)
        {
            List<VisualElement> results = new();

            foreach (VisualElement element in elements)
            {
                results.AddRange(element.Children().Where(child => DoesOneMatchSelector(child, selector)));
                if (descendents) results.AddRange(FindAllMatchingSelector(element.Children(), selector, descendents));
            }

            return results;
        }

        public static bool DoesOneMatchSelector(VisualElement element, string selector)
        {
            if (selector == "*") return true;

            foreach (string subselector in Regex.Split(selector, $"([{SelectorType}][{SelectorName}]+)"))
            {
                if (subselector.Length == 0) continue;

                switch (subselector[0])
                {
                    case '#': if (element.name != subselector[1..]) return false; break;
                    case '.': if (!element.ClassListContains(subselector[1..])) return false; break;
                    default: if (element.GetType().Name != subselector) return false; break;
                }
            }

            return true;
        }

        public IEnumerable<VisualElement> All() => _elements;

        public IEnumerable<T> All<T>() where T : VisualElement => _elements.OfType<T>();

        public VisualElement First() => _elements.First();

        public T First<T>() where T : VisualElement => _elements.OfType<T>().First();

        public void ForEach(Action<VisualElement> action) => _elements.ForEach(action);

        public void ForEach<T>(Action<T> action) where T : VisualElement => _elements.OfType<T>().ToList().ForEach(action);
    }
}
