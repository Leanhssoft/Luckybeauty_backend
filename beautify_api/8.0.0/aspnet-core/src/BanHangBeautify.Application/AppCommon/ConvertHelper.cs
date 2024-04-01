using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace BanHangBeautify.AppCommon
{
    public class ConvertHelper
    {
        /// <summary>
        /// chuyển chuỗi Tiếng Việt thành chuỗi không dấu (english)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertToUnSign(string text)
        {
            if (text != null && !string.IsNullOrWhiteSpace(text))
            {
                text = text.Trim();
                for (int i = 33; i < 48; i++)
                {
                    if (i != 45)
                    {
                        text = text.Replace(((char)i).ToString(), "");
                    }
                }

                for (int i = 58; i < 65; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }

                for (int i = 91; i < 97; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                for (int i = 123; i < 127; i++)
                {
                    text = text.Replace(((char)i).ToString(), "");
                }
                //text = text.Replace(" ", "-");
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
                return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            else
            {
                return "";
            }
        }
        public static object ConvertByType(Type type, object objConvert)
        {
            switch (type.Name)
            {
                case "String":
                    return ToString(objConvert);

                case "Int":
                    return ToInt32(objConvert);

                case "Double":
                    return ToDouble(objConvert);

                case "DateTime":
                    return ToDateTime(objConvert);

                case "Decimal":
                    return ToDecimal(objConvert);

                case "Float":
                    return ToInt64(objConvert);

                case "Guid":
                    return ToGuid(objConvert.ToString());

                case "Bool":
                    return ToBoolean(objConvert);

                case "Boolean":
                    return ToBoolean(objConvert);

                case "Int16":
                    return ToInt16(objConvert);

                case "Int32":
                    return ToInt32(objConvert);

                case "Int64":
                    return ToInt64(objConvert);

                case "Byte":
                    return ToByte(objConvert);
            }

            return null;
        }

        public static List<U> ConvertList<U, T>(List<T> listObject)
        {
            if (listObject == null)
            {
                return null;
            }

            List<U> uList = new List<U>();
            foreach (T t in listObject)
            {
                if (t != null)
                {
                    U uDefault = default;
                    U u = ConvertObject(t, uDefault);
                    uList.Add(u);
                }
            }

            return uList;
        }

        public static T ConvertObject<T>(object t, T defaultValue)
        {
            T obj = default;
            try
            {
                obj = (T)t;
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
            catch (ArgumentNullException)
            {
                return defaultValue;
            }

            return obj;
        }

        public static bool ToBoolean(object obj)
        {
            bool retVal;
            if (obj == null)
            {
                return false;
            }

            return bool.TryParse(obj.ToString(), out retVal) && retVal;
        }

        public static bool ToBoolean(DataRow dr, string field)
        {
            if (dr == null)
            {
                return false;
            }

            if (!dr.Table.Columns.Contains(field))
            {
                return false;
            }

            return Convert.ToBoolean(dr[field]);
        }

        public static bool ToBoolean(IDataReader dr, string field)
        {
            if (dr == null)
            {
                return false;
            }

            if (!HasColumn(dr, field))
            {
                return false;
            }

            return Convert.ToBoolean(dr[field]);
        }

        public static byte ToByte(object obj)
        {
            return ToByte(obj, 0);
        }

        public static byte ToByte(object obj, byte defaultValue)
        {
            byte retVal;
            if (obj != null && byte.TryParse(obj.ToString(), out retVal))
            {
                return retVal;
            }

            return defaultValue;
        }

        public static string ToSystemDate(string vietnamDate)
        {
            string[] part = vietnamDate.Split('/');
            if (part.Length > 1)
            {
                return string.Format("{0}-{1}-{2}", part[2], part[1], part[0]);
            }

            return string.Empty;
        }

        public static string ToVietNameseDate(object date)
        {
            string str = string.Empty;

            if (date.Equals("0001-01-01") || date.Equals("0000-00-00"))
                return "";

            if (date != null)
            {
                try
                {
                    str = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                }
                catch
                {
                    str = string.Empty;
                }

                if (str == "01/01/0001")
                {
                    str = string.Empty;
                }
            }

            return str;
        }

        public static DateTime ToDateTime(object obj)
        {
            return ToDateTime(obj, DateTime.Now);
        }

        public static DateTime ToDateTime(object obj, DateTime defaultValue)
        {
            DateTime retVal;
            if (obj == null)
            {
                return defaultValue;
            }

            if (!DateTime.TryParse(obj.ToString(), out retVal))
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static DateTime? ToDateTimeNullable(object obj, out bool isHaveError)
        {
            isHaveError = false;
            DateTime retVal;
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return null;
            }

            string[] formats = { "d/M/yy", "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "MM-dd-yyyy", "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "M/d/yy" };

            if (!DateTime.TryParseExact(obj.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out retVal))
            {
                isHaveError = true;
                return null;
            }

            return retVal;
        }

        public static DateTime ToDateTimeExact(object obj, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                pattern = "dd/MM/yyyy";
            }

            DateTime retVal;
            if (obj == null)
            {
                return DateTime.MinValue;
            }

            if (!DateTime.TryParseExact(obj.ToString(), pattern, null, DateTimeStyles.None, out retVal))
            {
                retVal = DateTime.Now;
                if (retVal == new DateTime(1, 1, 1))
                {
                    return DateTime.MinValue;
                }
            }

            return retVal;
        }
        public static string ConverDateTimeToString(DateTime? dt, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                pattern = "yyyy-MM-dd";
            }

            if (dt == null)
            {
                return string.Empty;
            }
            return dt?.ToString(pattern, CultureInfo.InvariantCulture);
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0M);
        }

        public static decimal ToDecimal(object obj, decimal defaultValue)
        {
            decimal retVal;
            if (obj != null && decimal.TryParse(obj.ToString(), out retVal))
            {
                return retVal;
            }

            return defaultValue;
        }

        public static double ToDouble(object obj)
        {
            return ToDouble(obj, 0.0);
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double retVal;
            if (obj != null && double.TryParse(obj.ToString(), out retVal))
            {
                return retVal;
            }

            return defaultValue;
        }

        public static Guid ToGuid(object obj, Guid defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            Guid retVal = defaultValue;
            try
            {
                retVal = ToGuid(obj);
            }
            catch (Exception)
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static short ToInt16(object obj)
        {
            return ToInt16(obj, 0);
        }

        public static short ToInt16(object obj, short defaultValue)
        {
            short retVal;
            if (obj != null && short.TryParse(obj.ToString(), out retVal))
            {
                return retVal;
            }

            return defaultValue;
        }

        public static int ToInt32(object obj)
        {
            return ToInt32(obj, 0);
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            int retVal;
            if (obj == null)
            {
                return 0;
            }

            if (int.TryParse(obj.ToString(), out retVal))
            {
                return retVal;
            }

            return defaultValue;
        }

        public static int ToInt32(IDataReader dr, string field, int defaultValue)
        {
            int retVal;
            if (dr == null)
            {
                return 0;
            }

            if (HasColumn(dr, field))
            {
                if (int.TryParse(dr[field].ToString(), out retVal))
                {
                    return retVal;
                }
            }

            return defaultValue;
        }

        public static int ToInteger(object obj)
        {
            return ToInt32(obj);
        }

        public static long ToInt64(object obj)
        {
            return ToInt64(obj, 0L);
        }

        public static long ToInt64(object obj, long defaultValue)
        {
            long retVal;
            if (obj != null && long.TryParse(obj.ToString(), out retVal))
            {
                return retVal;
            }

            return defaultValue;
        }

        public static string ToString(object obj)
        {
            return ToString(obj, string.Empty);
        }

        public static string ToString(object obj, string defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            string retVal = obj.ToString();
            if (string.IsNullOrEmpty(retVal))
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static string ToString(IDataReader dr, string field, string defaultValue)
        {
            if (dr == null)
            {
                return defaultValue;
            }

            if (HasColumn(dr, field))
            {
                return dr[field].ToString();
            }

            return defaultValue;
        }

        public static string ToString(DataRow dr, string field, string defaultValue)
        {
            if (dr == null)
            {
                return defaultValue;
            }

            if (dr.Table.Columns.Contains(field))
            {
                return dr[field].ToString();
            }

            return defaultValue;
        }

        public static string ToDateString(DateTime dt)
        {
            // If datatype is DateTime, then nothing else is necessary. 
            return dt == DateTime.MinValue ? string.Empty : dt.ToString("dd/MM/yyyy");
        }

        public static string ToTimeString(DateTime dt)
        {
            // If datatype is DateTime, then nothing else is necessary. 
            if (dt == DateTime.MinValue)
                return string.Empty;
            return dt.ToShortTimeString();
        }

        public static Guid ToGuid(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return Guid.Empty;
            // If datatype is Guid, then nothing else is necessary. 
            if (obj.GetType() == Type.GetType("System.Guid"))
                return (Guid)obj;

            string str = obj.ToString();
            if (str == string.Empty)
                return Guid.Empty;
            return XmlConvert.ToGuid(str);
        }

        public static bool IsEmptyGuid(Guid g)
        {
            if (g == Guid.Empty)
                return true;
            return false;
        }

        public static bool IsEmptyGuid(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            string str = obj.ToString();
            if (str == string.Empty)
                return true;
            Guid g = XmlConvert.ToGuid(str);
            if (g == Guid.Empty)
                return true;
            return false;
        }

        public static byte[] ToBinary(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return new byte[0];
            return (byte[])obj;
        }

        public static object ToDBGuid(Guid g)
        {
            if (g == Guid.Empty)
                return DBNull.Value;
            return g;
        }

        public static object ToDBGuid(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return DBNull.Value;
            // If datatype is Guid, then nothing else is necessary. 
            if (obj.GetType() == Type.GetType("System.Guid"))
                return obj;
            string str = obj.ToString();
            if (str == string.Empty)
                return DBNull.Value;
            Guid g = XmlConvert.ToGuid(str);
            if (g == Guid.Empty)
                return DBNull.Value;
            return g;
        }

        public static object ToDBDateTime(DateTime dt)
        {
            // 03/28/2010 Paul.  Check for SQL Server minimum date. 
            // SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
            if (dt == DateTime.MinValue)
                return DBNull.Value;
            else if (dt.Year < 1753)
                return DBNull.Value;
            return dt;
        }

        public static object ToDBDateTime(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return DBNull.Value;
            if (!Information.IsDate(obj))
                return DBNull.Value;
            DateTime dt = Convert.ToDateTime(obj);
            if (dt == DateTime.MinValue)
                return DBNull.Value;
            return dt;
        }

        public static object ToDBInteger(int n)
        {
            return n;
        }

        public static object ToDBInteger(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return DBNull.Value;
            // If datatype is Integer, then nothing else is necessary. 
            if (obj.GetType() == Type.GetType("System.Int32"))
                return obj;
            string str = obj.ToString();
            if (str == string.Empty || !Information.IsNumeric(str))
                return DBNull.Value;
            // 03/05/2011 Paul.  Lets start using TryParse to protect against non-numeric strings. 
            // This should prevent ugly exceptions when an alpha string is used. 
            //return Int32.Parse(str, NumberStyles.Any);
            int nValue = 0;
            // 03/16/2011 Paul.  We need to allow any style so that separators will get ignored. 
            int.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out nValue);
            return nValue;
        }

        public static string ObjectToJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private static bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        public static bool IsDbNull(object value)
        {
            return value == DBNull.Value;
        }
    }

    public class Information
    {
        public static bool IsDate(object o)
        {
            if (o == null)
                return false;
            else if (o is DateTime)
                return true;
            else if (o is string)
            {
                try
                {
                    DateTime.Parse(o as string);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public static bool IsNumeric(object o)
        {
            if (o == null || o is DateTime)
                return false;
            else if (o is short || o is int || o is long || o is decimal || o is float || o is double)
                return true;
            else
            {
                try
                {
                    if (o is string)
                        double.Parse(o as string);
                    else
                        double.Parse(o.ToString());
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }
    }
}
