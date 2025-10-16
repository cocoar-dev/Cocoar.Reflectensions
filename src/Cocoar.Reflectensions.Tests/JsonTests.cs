using System.Threading.Tasks;
using Cocoar.Reflectensions.JsonConverters;
using Cocoar.Reflectensions.Tests.TestClasses;
using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class JsonTests
    {
        [Fact]
        public async Task DictionaryArrayToExpandabledObject()
        {

            var json = @"[
  {
    ""ObjectId"": ""7936646d-6ec0-0e56-b043-89ec311882e3"",
            ""LastModified"": ""2021-02-23T20:49:18.55"",
            ""TimeAdded"": ""2021-02-23T20:49:18.3685095"",
            ""DisplayName"": null,
            ""TargetResolutionTime"": null,
            ""Escalated"": false,
            ""Source"": ""Console"",
            ""Status"": ""Active"",
            ""ResolutionDescription"": null,
            ""NeedsKnowledgeArticle"": false,
            ""TierQueue"": null,
            ""HasCreatedKnowledgeArticle"": false,
            ""LastModifiedSource"": ""Console"",
            ""Classification"": ""E-Mail Problems"",
            ""ResolutionCategory"": null,
            ""Priority"": 9,
            ""Impact"": ""High"",
            ""Urgency"": ""Medium"",
            ""ClosedDate"": null,
            ""ResolvedDate"": null,
            ""Id"": ""IR2"",
            ""Title"": ""TestInc"",
            ""Description"": null,
            ""ContactMethod"": null,
            ""CreatedDate"": ""2021-02-23T20:48:58.017"",
            ""ScheduledStartDate"": null,
            ""ScheduledEndDate"": null,
            ""ActualStartDate"": null,
            ""ActualEndDate"": null,
            ""IsDowntime"": null,
            ""IsParent"": null,
            ""ScheduledDowntimeStartDate"": null,
            ""ScheduledDowntimeEndDate"": null,
            ""ActualDowntimeStartDate"": null,
            ""ActualDowntimeEndDate"": null,
            ""RequiredBy"": null,
            ""PlannedCost"": null,
            ""ActualCost"": null,
            ""PlannedWork"": null,
            ""ActualWork"": null,
            ""UserInput"": {},
            ""FirstAssignedDate"": null,
            ""FirstResponseDate"": null
        }
        ]";


            var arr = Json.Converter.ToObject<ExpandableObject[]>(json);
        }

        [Fact]
        public void DecimalToJson()
        {

            var numberProps = new NumberProps();
            numberProps.Decimal = 1.2m;
            numberProps.Integer = 7;
            numberProps.Double = 1.234;
            numberProps.Float = 1.2345678f;
            numberProps.Long = long.MaxValue;

           

            var jsonConv = new Json();

            jsonConv.RegisterJsonConverter<DecimalJsonConverter>();

            var json = jsonConv.ToJson(numberProps);


        }

        [Fact]
        public void JsonToDecimal()
        {

            var jsonConv = new Json();

            jsonConv.RegisterJsonConverter<DecimalJsonConverter>();

            var json = "{\"Integer\":7,\"Double\":1.234,\"Float\":1.23456776,\"Long\":9223372036854775807,\"Decimal\":1.2}";
            var obj = jsonConv.ToObject<NumberProps>(json);


        }
    }
}
