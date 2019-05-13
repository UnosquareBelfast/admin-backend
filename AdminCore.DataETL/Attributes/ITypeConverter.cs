namespace AdminCore.Common.Attributes.DataEtl
{
    public interface ITypeConverter
    {
        object ConvertTo(object value);
    }
}