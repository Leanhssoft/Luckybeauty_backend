using BanHangBeautify.Data.Entities;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace BanHangBeautify.AppCommon
{
    public static class ObjectHelper
    {
        //public static List<T> ConvertListObjects<T, U>(List<U> uItems)
        //{
        //    List<T> tItems = null;
        //    T t1 = default(T);
        //    if ((uItems != null) && (uItems.Count > 0))
        //    {
        //        tItems = new List<T>();
        //        foreach (U u in uItems)
        //        {
        //            T t = ConvertHelper.ConvertObject<T>(u, t1);
        //            tItems.Add(t);
        //        }
        //    }
        //    return tItems;
        //}

        /// <summary>
        /// kiểm tra trùng dữ liệu ở cột colName (vị trí thứ indexColumn), từ dòng fromRow -->  toRow
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="colName">Tên cột kiểm tra (A,B,C..)</param>
        /// <param name="indexColumn">Vị trí của cột (1,2,3...)</param>
        /// <param name="fromRow">Đọc từ dòng</param>
        /// <param name="toRow">Đọc đến dòng</param>
        /// <returns>trả về danh sách bị trùng lặp CommonClass.ExcelErrorDto[] </returns>
        public static List<CommonClass.ExcelErrorDto> Excel_CheckDuplicateData(ExcelWorksheet worksheet, string colName, int indexColumn, int fromRow, int toRow)
        {
            List<CommonClass.ExcelErrorDto> lstErr = new List<CommonClass.ExcelErrorDto>();
            try
            {
                var listData = worksheet.Cells[$"{colName}{fromRow}:{colName}{toRow}"]
                    .Where(x => x.Value != null && x.Value.ToString() != string.Empty)
                    .Select(x => x.Value.ToString().Trim().ToUpper()).ToList();// convert to UpperCase
                for (int row = 3; row <= toRow; row++)
                {
                    var valCell = worksheet.Cells[row, indexColumn].Value?.ToString();
                    if (!string.IsNullOrEmpty(valCell))
                    {
                        valCell = valCell.Trim().ToUpper();
                        var duplicate = listData.Where(x => x == valCell).Count() > 1;
                        if (duplicate)
                        {
                            lstErr.Add(new CommonClass.ExcelErrorDto { RowNumber = row, GiaTriDuLieu = valCell });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstErr.Add(new CommonClass.ExcelErrorDto { RowNumber = 0, GiaTriDuLieu = ex.Message.ToString() });
            }
            return lstErr;
        }

        public static bool Excel_CheckNumber(string dataType)
        {
            string[] arrDataType = {"System.Sbyte", "System.byte", "System.Int16", "System.Int32", "System.Int64",
                "System.UInt16","System.UInt32", "System.UInt64", "System.Single","System.Double", "System.Decimal" };
            if (!arrDataType.Contains(dataType))
            {
                return false;
            }
            return true;
        }

        public static object CreateObject(Type type, DataRow row)
        {
            object objTarget = null;
            try
            {
                if (row == null)
                {
                    return objTarget;
                }
                objTarget = Activator.CreateInstance(type);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (property.CanWrite)
                    {
                        Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var value = row[property.Name];
                        object safeValue = value == null ? null
                                                           : Convert.ChangeType(value, t);

                        property.SetValue(objTarget, safeValue, null);
                        //property.SetValue(objTarget, Convert.ChangeType(row[property.Name], property.PropertyType), null);
                    }
                }
            }
            finally
            {
            }
            return objTarget;
        }

        public static object CreateObject(Type type, IDataReader dr)
        {
            object objTarget = null;
            try
            {
                if (!dr.Read())
                {
                    return objTarget;
                }
                objTarget = Activator.CreateInstance(type);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (property.CanWrite)
                    {
                        Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var value = dr[property.Name];
                        object safeValue = value == null ? null
                                                           : Convert.ChangeType(value, t);

                        property.SetValue(objTarget, safeValue, null);
                        //property.SetValue(objTarget, Convert.ChangeType(dr[property.Name], property.PropertyType), null);
                    }
                }
            }
            finally
            {
                dr.Close();
            }
            return objTarget;
        }

        public static List<T> FillCollection<T>(DataSet ds)
        {
            return FillCollection<T>(ds, string.Empty);
        }

        public static List<T> FillCollection<T>(DataTable dt)
        {
            return FillCollection<T>(dt, string.Empty);
        }

        public static U FillCollection<U, T>(IDataReader _dr)
        {
            U u = default;
            try
            {
                u = Activator.CreateInstance<U>();
                MethodInfo method = typeof(U).GetMethod("Add", new Type[] { typeof(T) });
                if (method == null)
                {
                    throw new InvalidOperationException("Return type does not contain Add method");
                }
                while (_dr.Read())
                {
                    T objTarget = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in objTarget.GetType().GetProperties())
                    {
                        try
                        {
                            if (_dr.GetOrdinal(property.Name) >= 0 && !(!property.CanWrite || Convert.IsDBNull(_dr[property.Name])))
                            {
                                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                var value = _dr[property.Name];
                                object safeValue = value == null ? null
                                                                   : Convert.ChangeType(value, t);

                                property.SetValue(objTarget, safeValue, null);
                                //property.SetValue(objTarget, Convert.ChangeType(_dr[property.Name], property.PropertyType), null);
                            }
                        }
                        catch (InvalidCastException)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                    }
                    if (method != null)
                    {
                        method.Invoke(u, new object[] { objTarget });
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _dr.Close();
            }
            return u;
        }

        public static object GetSafeValue(object value, Type typeOfValue)
        {
            Type t = Nullable.GetUnderlyingType(typeOfValue) ?? typeOfValue;
            object safeValue = value == null ? null
                                               : Convert.ChangeType(value, t);
            return safeValue;
        }

        public static List<T> FillCollection<T>(IDataReader _dr)
        {
            List<T> _list = new List<T>();
            try
            {
                while (_dr.Read())
                {
                    T objTarget = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in objTarget.GetType().GetProperties())
                    {
                        try
                        {
                            if (property != null)
                            {
                                if (_dr.GetOrdinal(property.Name) >= 0 && !(!property.CanWrite || Convert.IsDBNull(_dr[property.Name])))
                                {
                                    Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                    var value = _dr[property.Name];
                                    object safeValue = value == null ? null
                                                                       : Convert.ChangeType(value, t);

                                    property.SetValue(objTarget, safeValue, null);

                                    //property.SetValue(objTarget, Convert.ChangeType(_dr[property.Name], property.PropertyType), null);
                                }
                            }
                        }
                        catch (InvalidCastException)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            //if (property.GetSetMethod() != null)
                            //    property.SetValue(objTarget, null, null);
                        }
                    }
                    if (_list.IndexOf(objTarget) < 0)
                    {
                        _list.Add(objTarget);
                    }
                }
            }
            finally
            {
                _dr.Close();
            }
            return _list;
        }

        public static List<T> FillCollection<T>(DataSet ds, string propertyNames)
        {
            List<T> _list = new List<T>();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    _list = FillCollection<T>(ds.Tables[0], propertyNames);
                }
            }
            finally
            {
            }
            return _list;
        }

        public static List<T> FillCollection<T>(DataTable dt, string propertyNames)
        {
            List<T> _list = new List<T>();
            try
            {
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return _list;
                }
                foreach (DataRow row in dt.Rows)
                {
                    T objTarget = FillObject<T>(row, propertyNames);
                    if (_list.IndexOf(objTarget) < 0)
                    {
                        _list.Add(objTarget);
                    }
                }
            }
            finally
            {
            }
            return _list;
        }

        public static List<T> FillCollection<T>(IDataReader _dr, string propertyNames)
        {
            List<T> _list = new List<T>();
            try
            {
                while (_dr.Read())
                {
                    T objTarget = Activator.CreateInstance<T>();
                    PropertyInfo[] objProperties = objTarget.GetType().GetProperties();
                    string[] proNames = propertyNames.Split(new char[] { ',' });
                    for (int i = 0; i < proNames.Length; i++)
                    {
                        PropertyInfo property = objTarget.GetType().GetProperty(proNames[i]);
                        if (_dr.GetOrdinal(property.Name) >= 0 && !(!property.CanWrite || Convert.IsDBNull(_dr[property.Name])))
                        {
                            property.SetValue(objTarget, Convert.ChangeType(_dr[property.Name], property.PropertyType), null);
                        }
                    }
                    if (_list.IndexOf(objTarget) < 0)
                    {
                        _list.Add(objTarget);
                    }
                }
            }
            finally
            {
                _dr.Close();
            }
            return _list;
        }

        public static T FillObject<T>(DataRow row)
        {
            T objTarget = default;
            try
            {
                if (row == null)
                {
                    return objTarget;
                }
                objTarget = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in objTarget.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(property.Name) && !(!property.CanWrite || Convert.IsDBNull(row[property.Name])))
                    {
                        Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        if (row.Table.Columns[property.Name].DataType.Name == t.Name)
                        {
                            var value = row[property.Name];
                            object safeValue = value == null ? null : Convert.ChangeType(value, t);
                            property.SetValue(objTarget, safeValue, null);
                        }
                    }
                }
            }
            finally
            {
            }
            return objTarget;
        }

        public static T FillObject<T>(IDataReader _dr)
        {
            T objTarget = default;
            try
            {
                if (!_dr.Read())
                {
                    return objTarget;
                }
                objTarget = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in objTarget.GetType().GetProperties())
                {
                    try
                    {
                        if (_dr.GetOrdinal(property.Name) >= 0 && !(!property.CanWrite || Convert.IsDBNull(_dr[property.Name])))
                        {
                            Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            var value = _dr[property.Name];
                            object safeValue = value == null ? null
                                                               : Convert.ChangeType(value, t);

                            property.SetValue(objTarget, safeValue, null);
                            //property.SetValue(objTarget, _dr[property.Name], null);
                        }
                    }
                    catch (InvalidCastException)
                    {
                        property.SetValue(objTarget, null, null);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        //property.SetValue(objTarget, null, null);
                    }
                }
            }
            finally
            {
                _dr.Close();
            }
            return objTarget;
        }

        public static T FillObject<T>(DataRow row, string propertyNames)
        {
            if (string.IsNullOrEmpty(propertyNames))
            {
                return FillObject<T>(row);
            }
            T objTarget = default;
            try
            {
                if (row != null)
                {
                    objTarget = Activator.CreateInstance<T>();
                    PropertyInfo[] objProperties = objTarget.GetType().GetProperties();
                    string[] fieldList = propertyNames.Split(new char[] { ',' });
                    for (int i = 0; i < fieldList.Length; i++)
                    {
                        PropertyInfo property = objTarget.GetType().GetProperty(fieldList[i]);
                        if (row.Table.Columns.Contains(property.Name) && !(!property.CanWrite || Convert.IsDBNull(row[property.Name])))
                        {
                            Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            var value = row[property.Name];
                            object safeValue = value == null ? null
                                                               : Convert.ChangeType(value, t);

                            property.SetValue(objTarget, safeValue, null);
                            //property.SetValue(objTarget, Convert.ChangeType(row[property.Name], property.PropertyType), null);
                        }
                    }
                }
            }
            finally
            {
            }
            return objTarget;
        }

        public static T FillObject<T>(IDataReader _dr, string propertyNames)
        {
            if (string.IsNullOrEmpty(propertyNames))
            {
                return FillObject<T>(_dr);
            }
            T objTarget = default;
            try
            {
                if (_dr.Read())
                {
                    objTarget = Activator.CreateInstance<T>();
                    PropertyInfo[] objProperties = objTarget.GetType().GetProperties();
                    string[] fieldList = propertyNames.Split(new char[] { ',' });
                    for (int i = 0; i < fieldList.Length; i++)
                    {
                        PropertyInfo property = objTarget.GetType().GetProperty(fieldList[i]);
                        if (!(!property.CanWrite || Convert.IsDBNull(_dr[property.Name])))
                        {
                            Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            var value = _dr[property.Name];
                            object safeValue = value == null ? null
                                                               : Convert.ChangeType(value, t);

                            property.SetValue(objTarget, safeValue, null);
                            //property.SetValue(objTarget, Convert.ChangeType(_dr[property.Name], property.PropertyType), null);
                        }
                    }
                }
            }
            finally
            {
                _dr.Close();
            }
            return objTarget;
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                return Nullable.GetUnderlyingType(t);
            }
            return t;
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable tb = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }
            if (items != null)
            {
                foreach (T item in items)
                {
                    object[] values = new object[props.Length];
                    for (int i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                    tb.Rows.Add(values);
                }
            }
            return tb;
        }

        public static string GetStringFromDataReader(IDataReader dataReader, string columnName)
        {
            string result = string.Empty;
            try
            {
                int ordinal = dataReader.GetOrdinal(columnName);

                if (!dataReader.IsDBNull(ordinal))
                    result = dataReader.GetString(ordinal);
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }

        public static bool IsJsonString(string jsonString)
        {
            try
            {
                if (!(jsonString.Contains("{") && jsonString.Contains("}")) && !(jsonString.Contains("[") && jsonString.Contains("]")))
                {
                    return false;
                }
                var tmpObj = JToken.Parse(jsonString);
                return true;
            }
            catch (FormatException fex)
            {
                //Invalid json format
                Console.WriteLine(fex);
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
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

        /// <summary>
        /// Encodes the file as Base64 string.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A base64 encoded file.</returns>
        private static string EncodeFile(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            var encodedData = Convert.ToBase64String(filebytes, Base64FormattingOptions.InsertLineBreaks);

            return encodedData;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                .FirstOrDefault();
        }

        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                var property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        private static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}
