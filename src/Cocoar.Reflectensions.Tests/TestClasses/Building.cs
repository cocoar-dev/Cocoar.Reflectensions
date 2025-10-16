using System;
using System.Threading.Tasks;

namespace Cocoar.Reflectensions.Tests.TestClasses
{
    public class Building : CamouflageMode
    {
        private bool _mainDoorIsOpen;
        private int _floors;

       public int WindowCount { get; set; }
        public bool HasGarden { get; set; }

        public Building() { }

        public Building(int floors) {
            _floors = floors;
        }

        public void OpenMainDoor() {
            _mainDoorIsOpen = true;
        }

        public async Task OpenMainDoorAsync(TimeSpan? delay = null) {
            await Task.Run(async () => {
                if (delay.HasValue) {
                    await Task.Delay(delay.Value);
                }
                OpenMainDoor();
            });
        }

        public void CloseDoor() {
            _mainDoorIsOpen = false;
        }

        public bool IsDoorOpen() {

            return _mainDoorIsOpen;
        }

        public int CountFloors() {
            return _floors;
        }

        public async Task<int> CountFloorsAsync(TimeSpan? delay = null) {
            return await Task.Run(async () => {
                if (delay.HasValue) {
                    await Task.Delay(delay.Value);
                }
                return _floors;
            });
        }


        public async Task<int> CountFloorsAsync1(TimeSpan? delay, string text) {
            return await Task.Run(async () => {
                if (delay.HasValue) {
                    await Task.Delay(delay.Value);
                }
                return _floors;
            });
        }
    }
}
