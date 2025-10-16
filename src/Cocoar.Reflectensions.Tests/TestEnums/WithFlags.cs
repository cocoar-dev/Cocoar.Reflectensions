using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Cocoar.Reflectensions.Tests.TestEnums
{
    [Flags]
    public enum WithFlags {
        One = 1,

        [Description("__Two__")]
        Two = 2,

        [Description("__Four__")]
        [EnumMember(Value = "_Four")]
        Three = 4
    }
}