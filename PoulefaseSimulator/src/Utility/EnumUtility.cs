using System;
using System.Collections.Generic;

namespace PouleSimulator
{
    public static class EnumUtility
    {
        internal static class EnumValueCache<T> where T : System.Enum
        {
            internal static T[] enumValues;

            static EnumValueCache() {
                enumValues = Enum.GetValues(typeof(T)) as T[];
            }
        }

        internal static class EnumNameCache<T> where T : System.Enum
        {
            internal static string[] enumNames;

            static EnumNameCache() {
                enumNames = Enum.GetNames(typeof(T));
            }
        }

        internal static class EnumNameToEnumMappingCache<T> where T : System.Enum
        {
            internal static Dictionary<string, T> nameToEnum;

            static EnumNameToEnumMappingCache() {
                IReadOnlyList<string> names = GetCachedEnumNames<T>();
                IReadOnlyList<T> enums = GetCachedEnumValues<T>();

                nameToEnum = new Dictionary<string, T>(names.Count);
                for(int i = 0; i < names.Count; i++) { nameToEnum.Add(names[i], enums[i]); }
            }
        }

        internal static class EnumToEnumNameMappingCache<T> where T : System.Enum
        {
            internal static Dictionary<T, string> enumToName;

            static EnumToEnumNameMappingCache() {
                IReadOnlyList<T> enums = GetCachedEnumValues<T>();
                IReadOnlyList<string> names = GetCachedEnumNames<T>();

                enumToName = new Dictionary<T, string>(enums.Count);
                for(int i = 0; i < enums.Count; i++) { enumToName.Add(enums[i], names[i]); }
            }
        }

        /// <summary>
        /// Returns the result of Enum.GetValues(typeof(TEnum)) as TEnum[] (array gets allocated on each call)
        /// </summary>
        public static TEnum[] GetValues<TEnum>() where TEnum : Enum {
            return Enum.GetValues(typeof(TEnum)) as TEnum[];
        }


        /// <summary>
        /// If not previously requested, caches the array of enum values for type T, then returns it as IReadOnlyList
        /// </summary>
        public static IReadOnlyList<T> GetCachedEnumValues<T>() where T : System.Enum {
            return EnumValueCache<T>.enumValues;
        }

        /// <summary>
        /// If not previously requested, caches the array of enum names for type T, then returns it as IReadOnlyList
        /// </summary>
        public static IReadOnlyList<string> GetCachedEnumNames<T>() where T : System.Enum {
            return EnumNameCache<T>.enumNames;
        }

        /// <summary>
        /// If not previously requested, caches the array of enums and enum names for type T, and creates a name->enum mapping for it, returned as IReadOnlyDictionary
        /// </summary>
        public static IReadOnlyDictionary<string, T> GetCachedEnumNameToEnumMap<T>() where T : System.Enum {
            return EnumNameToEnumMappingCache<T>.nameToEnum;
        }

        /// <summary>
        /// If not previously requested, caches the array of enums and enum names for type T, and creates a enum->name mapping for it, returned as IReadOnlyDictionary
        /// </summary>
        public static IReadOnlyDictionary<T, string> GetCachedEnumToEnumNameMap<T>() where T : System.Enum {
            return EnumToEnumNameMappingCache<T>.enumToName;
        }

        /// <summary>
        /// Attempts to resolve an enum by name, when returned false resolvedEnum will be set to 'defaultEnum'
        /// </summary>
        public static bool TryGetEnumForName<T>(string enumName, out T resolvedEnum, T defaultEnum = default, bool ignoreCase = true) where T : struct {
            resolvedEnum = defaultEnum;

            if(string.IsNullOrEmpty(enumName)) return false;
            if(!Enum.TryParse<T>(enumName, ignoreCase, out T enumValue)) return false;

            resolvedEnum = enumValue;
            return true;
        }
    }
}