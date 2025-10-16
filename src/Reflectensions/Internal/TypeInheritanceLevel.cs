using System;

namespace Cocoar.Reflectensions.Internal
{
    internal record TypeInheritanceLevel
    {
        public Type Type { get; }
        public Type From { get; }
        public int Level { get; }

        public TypeInheritanceLevel(Type type, Type from, int level)
        {
            Type = type;
            From = from;
            Level = level;
        }
    }
}
