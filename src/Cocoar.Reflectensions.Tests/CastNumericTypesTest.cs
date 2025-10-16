using System;
using Cocoar.Reflectensions.ExtensionMethods;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class CastNumericTypesTest
    {

        [Theory]
        [InlineData(typeof(short))]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void sbyte_IsCastable_To(Type to)
        {
            var castable = typeof(sbyte).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void byte_IsCastable_To(Type to)
        {
            var castable = typeof(byte).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void short_IsCastable_To(Type to)
        {
            var castable = typeof(short).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void ushort_IsCastable_To(Type to)
        {
            var castable = typeof(ushort).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(long))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void int_IsCastable_To(Type to)
        {
            var castable = typeof(int).IsImplicitCastableTo(to);
            Assert.True(castable);
        }


        [Theory]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void uint_IsCastable_To(Type to)
        {
            var castable = typeof(uint).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void long_IsCastable_To(Type to)
        {

            var castable = typeof(long).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void char_IsCastable_To(Type to)
        {

            var castable = typeof(char).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

        [Theory]
        [InlineData(typeof(double))]
        public void float_IsCastable_To(Type to)
        {
            var castable = typeof(float).IsImplicitCastableTo(to);
            Assert.True(castable);
        }


        [Theory]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        public void ulong_IsCastable_To(Type to)
        {
            var castable = typeof(ulong).IsImplicitCastableTo(to);
            Assert.True(castable);
        }

    }
}
