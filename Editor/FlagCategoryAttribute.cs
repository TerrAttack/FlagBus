using System;

namespace Utils.Flags
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class FlagCategoryAttribute : Attribute
    {
        public FlagCategoryAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; }
    }
}
