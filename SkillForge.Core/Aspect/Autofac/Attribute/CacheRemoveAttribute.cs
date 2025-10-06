namespace SkillForge.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CacheRemoveAttribute : Attribute
    {
        public string Pattern { get; }
        public CacheRemoveAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}