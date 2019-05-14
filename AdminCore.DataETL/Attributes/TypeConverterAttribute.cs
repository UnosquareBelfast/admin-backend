using System;

namespace AdminCore.DataETL.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TypeConverterAttribute : Attribute
    {
        public Type ConverterType { get; set; }
        
        public virtual object CreateInstance()
        {
            return ConverterType != null ? Activator.CreateInstance(ConverterType) : null;
        }
    }
}