using System;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class ActionExtensionsTests
    {
        private class TestClass
        {
            public string Value { get; set; } = "";
            public int Number { get; set; }
        }

        private class AnotherTestClass
        {
            public bool Flag { get; set; }
        }

        [Fact]
        public void InvokeAction_WithSingleParameter_CreatesInstanceAndInvokesAction()
        {
            Action<TestClass> action = tc => tc.Value = "Modified";

            var result = action.InvokeAction();

            Assert.NotNull(result);
            Assert.Equal("Modified", result.Value);
        }

        [Fact]
        public void InvokeAction_WithProvidedInstance_UsesProvidedInstance()
        {
            var instance = new TestClass { Value = "Original" };
            Action<TestClass> action = tc => tc.Value = "Modified";

            var result = action.InvokeAction(instance);

            Assert.Same(instance, result);
            Assert.Equal("Modified", result.Value);
        }

        [Fact]
        public void InvokeAction_WithTwoParameters_CreatesInstancesAndInvokesAction()
        {
            Action<TestClass, AnotherTestClass> action = (tc, atc) =>
            {
                tc.Value = "First";
                atc.Flag = true;
            };

            var result = action.InvokeAction();

            Assert.NotNull(result.Item1);
            Assert.NotNull(result.Item2);
            Assert.Equal("First", result.Item1.Value);
            Assert.True(result.Item2.Flag);
        }

        [Fact]
        public void InvokeAction_WithTwoParameters_UsesProvidedInstances()
        {
            var first = new TestClass { Value = "Original1" };
            var second = new AnotherTestClass { Flag = false };
            Action<TestClass, AnotherTestClass> action = (tc, atc) =>
            {
                tc.Number = 42;
                atc.Flag = true;
            };

            var result = action.InvokeAction(first, second);

            Assert.Same(first, result.Item1);
            Assert.Same(second, result.Item2);
            Assert.Equal(42, result.Item1.Number);
            Assert.True(result.Item2.Flag);
        }

        [Fact]
        public void InvokeAction_WithThreeParameters_CreatesInstancesAndInvokesAction()
        {
            Action<TestClass, AnotherTestClass, TestClass> action = (tc1, atc, tc2) =>
            {
                tc1.Value = "First";
                atc.Flag = true;
                tc2.Number = 100;
            };

            var result = action.InvokeAction();

            Assert.NotNull(result.Item1);
            Assert.NotNull(result.Item2);
            Assert.NotNull(result.Item3);
            Assert.Equal("First", result.Item1.Value);
            Assert.True(result.Item2.Flag);
            Assert.Equal(100, result.Item3.Number);
        }

        [Fact]
        public void InvokeAction_WithThreeParameters_UsesProvidedInstances()
        {
            var first = new TestClass { Value = "A" };
            var second = new AnotherTestClass { Flag = false };
            var third = new TestClass { Number = 0 };
            Action<TestClass, AnotherTestClass, TestClass> action = (tc1, atc, tc2) =>
            {
                tc1.Value += "B";
                atc.Flag = true;
                tc2.Number = 999;
            };

            var result = action.InvokeAction(first, second, third);

            Assert.Same(first, result.Item1);
            Assert.Same(second, result.Item2);
            Assert.Same(third, result.Item3);
            Assert.Equal("AB", result.Item1.Value);
            Assert.True(result.Item2.Flag);
            Assert.Equal(999, result.Item3.Number);
        }

        [Fact]
        public void InvokeAction_WithThreeParameters_PartialProvidedInstances()
        {
            var first = new TestClass { Value = "Provided" };
            Action<TestClass, AnotherTestClass, TestClass> action = (tc1, atc, tc2) =>
            {
                tc1.Number = 1;
                atc.Flag = true;
                tc2.Value = "Created";
            };

            var result = action.InvokeAction(first, null, null);

            Assert.Same(first, result.Item1);
            Assert.NotNull(result.Item2);
            Assert.NotNull(result.Item3);
            Assert.Equal(1, result.Item1.Number);
            Assert.True(result.Item2.Flag);
            Assert.Equal("Created", result.Item3.Value);
        }
    }
}
