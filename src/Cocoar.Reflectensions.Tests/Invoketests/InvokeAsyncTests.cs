using System;
using System.Diagnostics;
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
        public async Task InvokeAsync_int() {
            var building = new Building(7);

            var method = building.GetType().GetMethod("CountFloors");

            var count = await InvokeHelper.InvokeMethodAsync<int>(building, method);
            
            Assert.Equal(7, count);
        }

        [Fact]
        public async Task InvokeAsync_int_TO_long() {
            var building = new Building(7);
            var method = building.GetType().GetMethod("CountFloors");
            var count = await InvokeHelper.InvokeMethodAsync<long>(building, method);
            Assert.Equal(7, count);
        }

        [Fact]
        public async Task InvokeAsync_Task() {

            var building = new Building(7);
            var sw = new Stopwatch();
            sw.Start();
            var method = building.GetType().GetMethod("OpenMainDoorAsync");
            await InvokeHelper.InvokeVoidMethodAsync(building, method, _delay);
            sw.Stop();

            //Assert.True(sw.Elapsed.Ticks >= _delay.Ticks);
        }

        [Fact]
        public async Task InvokeAsync_Task_OF_int() {

            var building = new Building(7);
            var sw = new Stopwatch();
            sw.Start();
            var method = building.GetType().GetMethod("CountFloorsAsync");
            var floorCount = await InvokeHelper.InvokeMethodAsync<int>(building, method, _delay);
            sw.Stop();

            //Assert.True(sw.Elapsed.Ticks >= _delay.Ticks);
            Assert.Equal(7, floorCount);
        }

        [Fact]
        public async Task InvokeAsync_Task_OF_int_TO_decimal() {

            var building = new Building(7);
            var sw = new Stopwatch();
            sw.Start();
            var method = building.GetType().GetMethod("CountFloorsAsync");
            var floorCount = await InvokeHelper.InvokeMethodAsync<decimal>(building, method, _delay);
            sw.Stop();

            //Assert.True(sw.Elapsed.Ticks >= _delay.Ticks);
            Assert.Equal(7, floorCount);
        }

        [Fact]
        public async Task InvokeAsync_Task_OF_int_TO_decimal_null() {

            var building = new Building(7);
            var sw = new Stopwatch();
            sw.Start();
            var method = building.GetType().GetMethod("CountFloorsAsync1");
            var floorCount = await InvokeHelper.InvokeMethodAsync<decimal>(building, method, _delay, null);
            sw.Stop();

            //Assert.True(sw.Elapsed.Ticks >= _delay.Ticks);
            Assert.Equal(7, floorCount);
        }


        [Fact]
        public async Task InvokeAsyncObject()
        {

            var testObj = new InvokeAsyncTestClass();

            var t = await testObj.GetNameAsync();

        }

    }
}
