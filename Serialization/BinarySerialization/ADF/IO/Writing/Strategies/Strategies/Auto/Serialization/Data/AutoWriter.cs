namespace Zion.Serialization.ADF
{
    public delegate void AutoWriter<in T>(StreamGroup Target, T Value);
}