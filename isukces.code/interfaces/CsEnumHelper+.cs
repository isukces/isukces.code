using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace iSukces.Code.Interfaces
{
    partial class CsEnumHelper {

        // ================================
        public static object GetZeroValue(IEnumerable<object> values, Type ut) {
            if (ut == typeof(int))
                return values.FirstOrDefault(a => (int)a == 0);            
            if (ut == typeof(uint))
                return values.FirstOrDefault(a => (uint)a == 0);            
            if (ut == typeof(short))
                return values.FirstOrDefault(a => (short)a == 0);            
            if (ut == typeof(ushort))
                return values.FirstOrDefault(a => (ushort)a == 0);            
            if (ut == typeof(long))
                return values.FirstOrDefault(a => (long)a == 0);            
            if (ut == typeof(ulong))
                return values.FirstOrDefault(a => (ulong)a == 0);            
            if (ut == typeof(byte))
                return values.FirstOrDefault(a => (byte)a == 0);            
            if (ut == typeof(sbyte))
                return values.FirstOrDefault(a => (sbyte)a == 0);            
            return null;
        }

        // ================================
        public static IReadOnlyList<ushort> GetBitsMap(object value, Type ut) {
            var result = new List<ushort>();
            // ------------------- int
            if (ut == typeof(int)) {
                ushort bitIndex = 0;
                var tmp = (int)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = tmp >> 1;
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- uint
            if (ut == typeof(uint)) {
                ushort bitIndex = 0;
                var tmp = (uint)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = tmp >> 1;
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- short
            if (ut == typeof(short)) {
                ushort bitIndex = 0;
                var tmp = (short)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = (short)(tmp >> 1);
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- ushort
            if (ut == typeof(ushort)) {
                ushort bitIndex = 0;
                var tmp = (ushort)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = (ushort)(tmp >> 1);
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- long
            if (ut == typeof(long)) {
                ushort bitIndex = 0;
                var tmp = (long)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = tmp >> 1;
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- ulong
            if (ut == typeof(ulong)) {
                ushort bitIndex = 0;
                var tmp = (ulong)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = tmp >> 1;
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- byte
            if (ut == typeof(byte)) {
                ushort bitIndex = 0;
                var tmp = (byte)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = (byte)(tmp >> 1);
                    bitIndex ++ ;
                }
                return result;
            }            
            // ------------------- sbyte
            if (ut == typeof(sbyte)) {
                ushort bitIndex = 0;
                var tmp = (sbyte)value;
                while(tmp != 0) {
                    if ((tmp & 1) > 0)
                        result.Add(bitIndex);
                    tmp = (sbyte)(tmp >> 1);
                    bitIndex ++ ;
                }
                return result;
            }            
            return XArray.Empty<ushort>();
        }
        // ================================


        public static IEnumerable<string> GetFlagStrings(object value, Type ut, object[] maps, string typeName) {
            // ------------------- int
            if (ut == typeof(int)) {
                var tmp = (int)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (int)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp &= ~m1;
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- uint
            else if (ut == typeof(uint)) {
                var tmp = (uint)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (uint)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp &= ~m1;
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- short
            else if (ut == typeof(short)) {
                var tmp = (short)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (short)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp = (short)(tmp & ~m1);
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- ushort
            else if (ut == typeof(ushort)) {
                var tmp = (ushort)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (ushort)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp = (ushort)(tmp & ~m1);
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- long
            else if (ut == typeof(long)) {
                var tmp = (long)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (long)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp &= ~m1;
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- ulong
            else if (ut == typeof(ulong)) {
                var tmp = (ulong)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (ulong)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp &= ~m1;
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- byte
            else if (ut == typeof(byte)) {
                var tmp = (byte)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (byte)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp = (byte)(tmp & ~m1);
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
            // ------------------- sbyte
            else if (ut == typeof(sbyte)) {
                var tmp = (sbyte)value;
                foreach (var m in maps)
                {
                    if (tmp == 0) yield break;
                    var m1 = (sbyte)m;
                    if ((tmp & m1) != m1) continue;
                    yield return typeName + "." + m;
                    tmp = (sbyte)(tmp & ~m1);
                }

                if (tmp != 0)
                    yield return "(" + typeName + ")" + tmp.ToString(CultureInfo.InvariantCulture);
            }            
        
        }
        // ================================
    }
}
 