using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AdminCore.Common.Interfaces;
using AdminCore.DataETL.Attributes;
using Castle.Core.Internal;
using ChoETL;

namespace AdminCore.DataETL
{
    /// <summary>
    /// Implements the ETL Library ChoEtl for conversion of data to csv.
    /// https://github.com/Cinchoo/ChoETL
    /// </summary>
    public class CsvFileTransformAdapter : IFileTransformAdapter
    {
        public byte[] GenerateByteArray<T>(IList<T> data) where T : class
        {
            var msg = new StringBuilder();

            if (data != null && data.Any())
            {
                var objectsToWriteList = CreateExpandoObjectList(data);

                var config = CreateConfig<T>();
                using (var parser = new ChoCSVWriter(new StringWriter(msg), config))
                {
                    parser.Write(objectsToWriteList);
                }
            }

            return Encoding.UTF8.GetBytes(msg.ToString());
        }

        /// <summary>
        /// Create the correct ChoEtl config using the CsvRecordFieldAttributes in the class <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private ChoCSVRecordConfiguration CreateConfig<T>()
        {
            var config = new ChoCSVRecordConfiguration();
            foreach (var csvRecordField in GetCsvExportableRecordFieldAttributes<T>())
            {
                if (csvRecordField != null)
                {
                    config.CSVRecordFieldConfigurations.Add(new ChoCSVRecordFieldConfiguration(csvRecordField.Name, csvRecordField.ColumnPosition));
                }
            }

            config.WithFirstLineHeader(true);
            return config;
        }

        /// <summary>
        /// Create a list of ExpandoObject given <paramref name="data"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<ExpandoObject> CreateExpandoObjectList<T>(IList<T> data)
        {
            return data.Select(CreateExpandoObject).ToList();
        }

        /// <summary>
        /// Creates an ExpandoObject using reflection to iterate over the properties in the specified class of type <see cref="T"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private ExpandoObject CreateExpandoObject<T>(T obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();

            var exp = new ExpandoObject() as IDictionary<string, Object>;

            foreach (var propertyInfo in properties)
            {
                exp.Add(propertyInfo.Name, propertyInfo.GetValue(obj));
            }

            return (ExpandoObject)exp;
        }

        /// <summary>
        /// Return all CsvRecordFieldAttribute attributes in a class of type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerable<ExportableRecordFieldAttribute> GetCsvExportableRecordFieldAttributes<T>()
        {
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (IsRecordFieldIgnored(propertyInfo))
                {
                    yield return null;
                }
                else
                {
                    var style = propertyInfo.GetAttribute<ExportableRecordFieldAttribute>();
                    yield return style ?? throw new InvalidOperationException($"{propertyInfo.Name} is not decorated with the required ExportableRecordFieldAttribute");
                }
            }
        }

        /// <summary>
        /// Return true if the field is decorated with an Ignore attribute
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private bool IsRecordFieldIgnored(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetAttribute<IgnoreRecordFieldAttribute>() != null;
        }
    }
}
