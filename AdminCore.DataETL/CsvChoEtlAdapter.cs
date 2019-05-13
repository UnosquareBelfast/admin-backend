using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AdminCore.Common.Attributes.DataEtl;
using AdminCore.Common.Interfaces;
using AutoMapper;
using Castle.Core.Internal;
using ChoETL;

namespace AdminCore.DataETL
{
    public class CsvChoEtlAdapter : IDataEtlAdapter
    {
        public byte[] GenerateByteArray<T>(IList<T> data) 
            where T : class
        {
            var objectsToWriteList = CreateExpandoObjectList(data);

            var config = CreateConfig<T>();
            var msg = new StringBuilder();
            using (var parser = new ChoCSVWriter(new StringWriter(msg), config))
            {
                parser.Write(objectsToWriteList);
            }
            
            return ASCIIEncoding.UTF32.GetBytes(msg.ToString());
        }

        /// <summary>
        /// Create the correct ChoEtl config using the CsvRecordFieldAttributes in the class <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private ChoCSVRecordConfiguration CreateConfig<T>()
        {
            var config = new ChoCSVRecordConfiguration();
            foreach (var csvRecordField in GetCsvRecordFieldAttributes<T>())
            {
                config.CSVRecordFieldConfigurations.Add(new ChoCSVRecordFieldConfiguration(csvRecordField.Name, csvRecordField.ColumnPosition));
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
                var convertedVal = ConvertUsingConverterAttribute(propertyInfo, obj);
                exp.Add(propertyInfo.Name, convertedVal);
            }

            return (ExpandoObject)exp;
        }

        /// <summary>
        /// Convert a field value using the converter specified by the <see cref="TypeConverterAttribute"/> decorating the field. 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="objToConvert"></param>
        /// <returns></returns>
        private object ConvertUsingConverterAttribute(PropertyInfo propertyInfo, Object objToConvert)
        {
            var converter = (ITypeConverter) propertyInfo.GetAttribute<TypeConverterAttribute>()?.CreateInstance();
            return converter != null ? converter.ConvertTo(propertyInfo.GetValue(objToConvert, null)) : propertyInfo.GetValue(objToConvert, null);
        }
        
        /// <summary>
        /// Return all CsvRecordFieldAttribute attributes in a class of type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerable<CsvRecordFieldAttribute> GetCsvRecordFieldAttributes<T>()
        {
            int i = 0;
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                var style = propertyInfo.GetAttribute<CsvRecordFieldAttribute>();
                yield return style ?? new CsvRecordFieldAttribute { Name = propertyInfo.Name, ColumnPosition = i++};
            }
        }
    }
}