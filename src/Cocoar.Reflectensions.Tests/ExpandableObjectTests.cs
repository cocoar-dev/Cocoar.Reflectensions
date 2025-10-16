using System;
using System.Collections.Generic;
using Cocoar.Reflectensions.Tests.TestClasses;
using Newtonsoft.Json;
using Xunit;

namespace Cocoar.Reflectensions.Tests {
    public class ExpandableObjectTests {
        [Fact]
        public void SerializeToDictionary() {

            var exp1 = new Expandable1();
            exp1.Name = "Bernhard";
            exp1.Age = 99;
            exp1["Ok"] = true;


            var jsonDict = JsonConvert.SerializeObject(exp1);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonDict);  //exp1.AsDictionary();


            Assert.Equal(true, dict["Ok"]);
            Assert.Equal("Bernhard", dict["Name"]);
            Assert.Equal("99", dict["Age"].ToString());

        }

        [Fact]
        public void SerializeInherit() {

            var exp2 = new Expandable2();
            exp2.Name = "Bernhard";
            exp2.Age = 99;
            exp2["Ok"] = true;
            exp2.Dates = new List<DateTime> {
                DateTime.Now,
                DateTime.Now.AddDays(1)
            };

            var exp3 = new Expandable2();
            exp3.Age = 3;
            exp3.Dates = new List<DateTime> {
                DateTime.Now.AddMonths(1),
                DateTime.Now.AddDays(7)
            };
            var exp4 = new Expandable1();
            exp4.Age = 4;
            var autobot = new Autobot("Bruce");
            autobot.ChangeNickName("Brucy");

            exp4["Autobot"] = autobot;



            exp2["nested"] = exp3;
            exp2["list1"] = new List<object> {
                exp3,
                exp4
            };

            exp2["list2"] = new List<object> {
               123123,
                "TestValue",
                exp4
            };

        }

        [Fact]
        public void AsDictionary() {

            var exp2 = new Expandable2();
            exp2.Name = "Bernhard";
            exp2.Age = 99;
            exp2["Ok"] = true;
           


            var idict = exp2 as IDictionary<string, object>;

            var n = idict["Name"];


            var dict = idict;

            var ndict = (IDictionary<string, object>)Activator.CreateInstance(typeof(Expandable2));
            ndict["Name"] = "BernhardDict";
            ndict["Age"] = 10;

            var lengt = ndict.Count;

            var _ex = ndict as Expandable2;

            var a = _ex.Age;

            Assert.Equal(true, dict["Ok"]);
            Assert.Equal("Bernhard", dict["Name"]);
            Assert.Equal("99", dict["Age"].ToString());

        }

        [Fact]
        public void FromDictionary() {


            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["test"] = "123";
            var exp = new ExpandableObject(dict);
            
        }

        [Fact]
        public void DeserializeFromJson()
        {
            var exp1 = new Expandable1();
            exp1.Name = "Bernhard";
            exp1.Age = 99;
            exp1["Ok"] = true;


            var jsonDict = JsonConvert.SerializeObject(exp1);

            var dict = JsonConvert.DeserializeObject<Expandable2>(jsonDict);  //exp1.AsDictionary();

            
            Assert.Equal(true, dict["Ok"]);
            Assert.Equal("Bernhard", dict["Name"]);
            Assert.Equal("99", dict["Age"].ToString());
            Assert.Equal(99, dict.Age);

        }
    }
}
