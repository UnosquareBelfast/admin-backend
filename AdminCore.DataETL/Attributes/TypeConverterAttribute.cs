using System;

namespace AdminCore.Common.Attributes.DataEtl
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TypeConverterAttribute : Attribute
    {
//        public ITypeConverter Converter { get; set; }
        public Type ConverterType { get; set; }
        
        public virtual object CreateInstance()
        {
            return ConverterType != null ? Activator.CreateInstance(ConverterType) : null;
        }
    }
}