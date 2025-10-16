using System;
using System.Collections;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class IDictionaryExtensions
    {
        public static T Merge<T>(this T dict, T mergeWith) where T : IDictionary
        {

            var nDict = Activator.CreateInstance<T>();
            foreach (DictionaryEntry de in dict)
            {
                nDict.Add(de.Key, de.Value);
            }

            foreach (DictionaryEntry de in mergeWith)
            {
                if (!nDict.Contains(de.Key))
                {
                    nDict.Add(de.Key, de.Value);
                }
                else
                {
                    nDict[de.Key] = de.Value;
                }
            }

            return nDict;

        }
    }
}
