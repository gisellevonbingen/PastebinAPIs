using Giselle.Commons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class QueryValues : List<QueryValue>
    {
        public static QueryValues Parse(string str)
        {
            str = StringUtils.RemovePrefix(str, HttpUtility2.QuerySeparator);
            var splits = str.Split(HttpUtility2.QueryValuesDelimiter);

            var queryValues = new QueryValues();
            var valueDelimiter = QueryValue.ValueDelimiter;

            foreach (var pair in splits)
            {
                if (string.IsNullOrWhiteSpace(pair) == false)
                {
                    var delimiterIndex = pair.IndexOf(valueDelimiter);
                    string key = null;
                    string value = null;

                    if (delimiterIndex == -1)
                    {
                        key = pair;
                    }
                    else
                    {
                        key = pair.Substring(0, delimiterIndex);
                        var valueStartIndex = delimiterIndex + valueDelimiter.Length;
                        value = pair.Substring(valueStartIndex, pair.Length - valueStartIndex);
                    }

                    queryValues.Add(new QueryValue(key, value));
                }

            }

            return queryValues;
        }

        public QueryValues()
        {

        }

        public QueryValues(IEnumerable<QueryValue> collection) : base(collection)
        {

        }

        public override string ToString()
        {
            return this.ToString(true, true);
        }

        public string ToString(bool prefix, bool containsNullOrWhiteSpace)
        {
            var toString = string.Join(HttpUtility2.QueryValuesDelimiter, this.Where(p =>
            {
                if (containsNullOrWhiteSpace == false)
                {
                    if (string.IsNullOrWhiteSpace(p.Key) == true || string.IsNullOrWhiteSpace(p.Value) == true)
                    {
                        return false;
                    }

                }

                return true;
            }));

            if (prefix == true)
            {
                return StringUtils.AddPrefix(toString, HttpUtility2.QuerySeparator);
            }
            else
            {
                return toString;
            }

        }

        public void RemoveAll(string key)
        {
            foreach (var value in this.ToArray())
            {
                if (value.Key.Equals(key) == true)
                {
                    this.Remove(value);
                }

            }

        }

        public void Add<T>(string key, T value)
        {
            this.Add(new QueryValue(key, string.Concat(value)));
        }

        public void AddRange<T>(string key, IEnumerable<T> values)
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    this.Add(key, value);
                }

            }

        }

        public string Single(string key)
        {
            return this.FirstOrDefault(pair => pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;
        }

        public string[] Array(string key)
        {
            return this.Where(pair => pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Select(pair => pair.Value).ToArray();
        }

    }

}
