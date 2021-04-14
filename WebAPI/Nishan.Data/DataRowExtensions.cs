using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Nishan.Data
{
    public static class DataRowExtensions
    {
        private static bool IsNullValue(object value) => value == null || value == DBNull.Value;
        private static readonly CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        public static bool IsNullValue(this DataRow dataRow, string columnName)
        {
            var value = dataRow.GetValue(columnName);
            return IsNullValue(value);
        }

        public static int GetInteger(this DataRow dataRow, string columnName)
        {
            var value = dataRow.GetValue(columnName);
            if (IsNullValue(value))
            {
                throw new InvalidCastException($"The value of the column {columnName} is null, can not be cast to Integer");
            }

            return Convert.ToInt32(value, invariantCulture);
        }

        public static Guid? GetGuid(this DataRow dataRow, string columnName)
        {
            var stringResult = GetString(dataRow, columnName);
            if (string.IsNullOrWhiteSpace(stringResult))
            {
                return null;
            }
            else
            {
                return Guid.Parse(stringResult);
            }
        }

        public static string GetString(this DataRow dataRow, string columnName)
        {
            var value = dataRow.GetValue(columnName);
            return IsNullValue(value) ? null : Convert.ToString(value);
        }

        private static object GetValue(this DataRow dataRow, int index)
        {
            if (dataRow is null)
            {
                throw new ArgumentNullException(nameof(dataRow));
            }

            if (dataRow.Table.Columns.Count < index - 1)
            {
                throw new IndexOutOfRangeException($"The index {index} is out of range");
            }

            return dataRow[index];
        }

        private static object GetValue(this DataRow dataRow, string columnName)
        {
            if (dataRow is null)
            {
                throw new ArgumentNullException(nameof(dataRow));
            }

            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException("Column name can not be blank", nameof(columnName));
            }
            if (dataRow.Table.Columns.Contains(columnName))
            {
                try
                {
                    return dataRow[columnName];
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to read value for column {columnName}", ex);
                }
            }
            {
                throw new IndexOutOfRangeException($"Column {columnName} does not exist");
            }
        }

        public static T GetValue<T>(this DataRow dataRow, string columnName) => GetValue(dataRow, columnName, default(T));
        public static T GetValue<T>(this DataRow dataRow, string columnName, T defaultValue)
        {
            object value = dataRow.GetValue(columnName);
            T result = defaultValue;
            var type = typeof(T);
            if (IsNullValue(value))
            {
                bool nullable = (!type.IsValueType) ||
                          (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
                if (!nullable)
                {
                    throw new InvalidCastException(
                        $"The value of the column {columnName} is null, can not assign to a non-nullable type");
                }
                //Note: Null assignment not required, default(T) already assigned
            }
            else
            {
                try
                {
                    result = (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T), invariantCulture);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Failed to convert value for column '{columnName}' ", ex);
                }
            }

            return result;
        }

        public static string GetPlainTextValue(this DataRow dataRow, string columnName)
        {
            object value = dataRow.GetValue(columnName);
            var doc = new HtmlDocument();
            doc.LoadHtml(value.ToString());

            //List<string> NodeText = new List<string>();
            StringBuilder NodeText = new StringBuilder();
            var nodes = doc.DocumentNode.Descendants().Where(n => n.NodeType == HtmlNodeType.Text);
            foreach (var node in nodes)
            {
                if (!string.IsNullOrWhiteSpace(node.InnerText))
                    NodeText.Append(node.InnerText);
                // NodeText.Add(node.InnerText); 
            }


            return NodeText.ToString();
        }
    }
}