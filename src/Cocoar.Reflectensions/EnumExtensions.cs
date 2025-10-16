using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Cocoar.Reflectensions
{
    public static class EnumExtensions
    {

        public static string GetName(this Enum enumValue) {


            var enumType = enumValue.GetType();
            var enumStringValue = enumValue.ToString("F");

            var enums = enumStringValue.Split(',').Select(m => (Enum)Enum.Parse(enumType, m.Trim()));
            
            return String.Join(", ", enums.Select(GetSingleName));

        }

        private static string GetSingleName(Enum enumValue) {

            var val = enumValue.ToString("F");
            var field = enumValue.GetType()
                       .GetTypeInfo()
                       .DeclaredMembers
                       .SingleOrDefault(x => x.Name == val);


            return field?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value ?? field?.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? val;
        }


        public static bool TryFind(Type enumType, string value, out object? result) {
            return TryFind(enumType, value, false, out result);
        }
        public static bool TryFind(Type enumType, string value, bool ignoreCase, out object? result) {
            var fields = enumType.GetFields();

            var enumValues = value.Split(',').Select(v => v.Trim()).Where(v => !String.IsNullOrWhiteSpace(v)).ToList();

            if (!enumValues.Any()) {
                result = null;
                return false;
            }

            var enums = new List<string>();

            var comparsion = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            foreach (var val in enumValues) {
                foreach (var field in fields) {
                    var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (attribute != null) {
                        if (attribute.Value?.Equals(val, comparsion) == true) {
                            enums.Add(field.Name!);
                            continue;
                        }

                    }

                    var descAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                    if (descAttribute != null) {
                        if (descAttribute.Description.Equals(val, comparsion) == true) {
                            enums.Add(field.Name!);
                            continue;
                        }

                    }

                    if (field.Name?.Equals(val, comparsion) == true) {
                        enums.Add(field.Name);
                    }
                }
            }


            result = Enum.Parse(enumType, String.Join(",", enums), ignoreCase);
            return result != null;
        }

    }

}
