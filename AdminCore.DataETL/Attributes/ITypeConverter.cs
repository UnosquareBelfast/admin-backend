namespace AdminCore.DataETL.Attributes
{
    public interface ITypeConverter
    {
        object ConvertTo(object value);
    }
}