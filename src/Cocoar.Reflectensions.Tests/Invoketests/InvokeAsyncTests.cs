using System;
using System.Threading.Tasks;
using Cocoar.Reflectensions.Helper;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;
using Xunit.Abstractions;

namespace Cocoar.Reflectensions.Tests.Invoketests {
    public class InvokeAsyncTests {

        private readonly TimeSpan _delay = TimeSpan.FromSeconds(1);

        private readonly ITestOutputHelper _output;

        public InvokeAsyncTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task InvokeMethodAsync_ReturnsInt_WhenMethodReturnsInt() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloors");
            var count = await InvokeHelper.InvokeMethodAsync<int>(building, method!);
            
            Assert.Equal(7, count);
        }

        [Fact]
        public async Task InvokeMethodAsync_ConvertsIntToLong_WhenRequestedTypeDiffers() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloors");
            var count = await InvokeHelper.InvokeMethodAsync<long>(building, method!);
            
            Assert.Equal(7, count);
        }

        [Fact]
        public async Task InvokeVoidMethodAsync_ExecutesTaskMethod_Successfully() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("OpenMainDoorAsync");
            
            await InvokeHelper.InvokeVoidMethodAsync(building, method!, _delay);
        }

        [Fact]
        public async Task InvokeMethodAsync_ReturnsInt_WhenMethodReturnsTaskOfInt() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloorsAsync");
            var floorCount = await InvokeHelper.InvokeMethodAsync<int>(building, method!, _delay);

            Assert.Equal(7, floorCount);
        }

        [Fact]
        public async Task InvokeMethodAsync_ConvertsIntToDecimal_WhenMethodReturnsTaskOfInt() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloorsAsync");
            var floorCount = await InvokeHelper.InvokeMethodAsync<decimal>(building, method!, _delay);

            Assert.Equal(7, floorCount);
        }

        [Fact]
        public async Task InvokeMethodAsync_ConvertsIntToDecimal_WithNullParameter() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloorsAsync1");
            var floorCount = await InvokeHelper.InvokeMethodAsync<decimal>(building, method!, _delay, null!);

            Assert.Equal(7, floorCount);
        }


        [Fact]
        public async Task InvokeAsyncMethod_ExecutesSuccessfully()
        {
            var testObj = new InvokeAsyncTestClass();
            var name = await testObj.GetNameAsync();
            
            Assert.Null(name);
        }

    }
}
