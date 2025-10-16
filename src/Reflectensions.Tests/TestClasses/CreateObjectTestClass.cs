namespace Cocoar.Reflectensions.Tests.TestClasses {
    public class CreateObjectTestClass<T> {
        public string Name { get; set; }

        public T Data { get; set; }

        public CreateObjectTestClass(string name) {
            Name = name;
        }

        public CreateObjectTestClass(string name, T data) {
            Name = name;
            Data = data;
        }

        public CreateObjectTestClass() {

        }
    }
}
