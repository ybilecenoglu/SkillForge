namespace CoreTestFramework.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CacheAttribute : Attribute
    {
        public int DurationTime { get; }

        public CacheAttribute(int durationTime = 60)
        {
            DurationTime = durationTime;
        }
    }
}