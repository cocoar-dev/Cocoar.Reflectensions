using System;
using Cocoar.Reflectensions.Helper;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;
using Xunit.Abstractions;

namespace Cocoar.Reflectensions.Tests.Invoketests {
    public class InvokeSyncTests {
        private readonly TimeSpan _delay = TimeSpan.FromSeconds(1);


        private readonly ITestOutputHelper _output;

        public InvokeSyncTests(ITestOutputHelper output) {
            this._output = output;
        }

        [Fact]
        public void InvokeMethod_ReturnsInt_WhenMethodReturnsInt() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloors");
            var count = InvokeHelper.InvokeMethod<int>(building, method!);

            Assert.Equal(7, count);
        }

        [Fact]
        public void InvokeMethod_ConvertsIntToLong_WhenRequestedTypeDiffers() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloors");
            var count = InvokeHelper.InvokeMethod<long>(building, method!);
            
            Assert.Equal(7, count);
        }

        [Fact]
        public void InvokeVoidMethod_ExecutesTaskMethod_Successfully() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("OpenMainDoorAsync");
            
            InvokeHelper.InvokeVoidMethod(building, method!, _delay);
        }

        [Fact]
        public void InvokeMethod_ReturnsInt_WhenMethodReturnsTaskOfInt() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloorsAsync");
            var floorCount = InvokeHelper.InvokeMethod<int>(building, method!, _delay);

            Assert.Equal(7, floorCount);
        }

        [Fact]
        public void InvokeMethod_ConvertsIntToDecimal_WhenMethodReturnsTaskOfInt() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloorsAsync");
            var floorCount = InvokeHelper.InvokeMethod<decimal>(building, method!, _delay);

            Assert.Equal(7, floorCount);
        }

    }
}
