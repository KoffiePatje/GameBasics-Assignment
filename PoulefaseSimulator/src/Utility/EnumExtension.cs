using System;
using System.Collections.Generic;

namespace PouleSimulator
{
    /// <summary>
    /// Extension class for <see cref="Enum"/>
    /// </summary>
    public static class EnumExtension
    {
        private static class EnumIterationUtility<T> where T : Enum
        {
            private static readonly IReadOnlyList<T> enumValues;

            static EnumIterationUtility() {
                enumValues = EnumUtility.GetCachedEnumValues<T>();
                if(enumValues.Count <= 0) throw new NotSupportedException("Next/Previous operations are not supported for enums without any values");
            }

            internal static T Next(T enumValue, bool allowWrapping) {
                int enumIndex = enumValues.IndexOf(enumValue);

                if(enumIndex < enumValues.Count - 1) {
                    enumIndex += 1;
                } else if(allowWrapping) {
                    enumIndex = 0;
                }

                return enumValues[enumIndex];
            }

            internal static T Previous(T enumValue, bool allowWrapping) {
                int enumIndex = enumValues.IndexOf(enumValue);

                if(enumIndex > 0) {
                    enumIndex -= 1;
                } else if(allowWrapping) {
                    enumIndex = enumValues.Count - 1;
                }

                return enumValues[enumIndex];
            }
        }

        /// <summary>
        /// Allows getting the 'Previous' enum, taking into account that enums sometimes leave gaps in terms of matching integer values
        /// </summary>
        public static T Previous<T>(this T enumValue, bool allowWrapping = true) where T : Enum {
            return EnumIterationUtility<T>.Previous(enumValue, allowWrapping);
        }

        /// <summary>
        /// Allows getting the 'Next' enum, taking into account that enums sometimes leave gaps in terms of matching integer values
        /// </summary>
        public static T Next<T>(this T enumValue, bool allowWrapping = true) where T : Enum {
            return EnumIterationUtility<T>.Next(enumValue, allowWrapping);
        }
    }
}
