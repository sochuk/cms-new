using FastMember;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CMS.Helper
{
    public static class Converter
    {
        public static byte[] GetByteArrayFromImage(string ImageFileName)
        {
            byte[] byteArray = null;
            try
            {
                using (FileStream stream = new FileStream(ImageFileName, FileMode.Open, FileAccess.Read))
                {
                    byteArray = new byte[stream.Length];
                    stream.Read(byteArray, 0, (int)stream.Length);
                }
            }
            catch
            {
                return new byte[0];
            }
            
            return byteArray;
        }

        public static Bitmap ResizeImage(Image image, int MaxWidthPixel = 600)
        {
            int wImg = image.Width;
            int hImg = image.Height;

            while (wImg >= MaxWidthPixel)
            {
                wImg = Convert.ToInt32(wImg * 0.9);
                hImg = Convert.ToInt32(hImg * 0.9);
            }

            var destRect = new Rectangle(0, 0, wImg, hImg);
            var destImage = new Bitmap(wImg, hImg);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void SaveAsJpeg(Bitmap bitmap, string filename)
        {           
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;            
            bitmap.Save(filename, myImageCodecInfo, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static int ToInteger(this string str)
        {
            if (str == "" || str == null || str == string.Empty) str = "0";
            return Convert.ToInt32(str);
        }

        public static long ToLong(this string str)
        {
            if (str == "" || str == null || str == string.Empty) str = "0";
            return long.Parse(str);
        }

        public static int ToInteger(this object obj)
        {
            if (obj == null) obj = "0";
            try
            {
                int.Parse(obj.ToString());
            }
            catch
            {
                obj = "0";
            }
            
            return Convert.ToInt32(obj);
        }

        public static int ToInteger(this bool obj)
        {
            if (obj == true) return 1;
            return 0;
        }

        public static string ToNullString(this object obj)
        {
            if (obj == null) obj = string.Empty;
            return Convert.ToString(obj);
        }

        public static bool ToBoolean(this string str)
        {
            if (str == "" || str == string.Empty || str == null || str == "0") return false;
            return Convert.ToBoolean(str.ToUpper() == "FALSE" ? false : true);
        }

        public static bool ToBoolean(this object obj)
        {
            if (obj == null || obj.ToString() == "0") obj = "false";
            if(obj.ToString() == "1") obj = "true";

            try
            {
                Convert.ToBoolean(obj.ToString());
            }
            catch
            {
                obj = "false";
            }

            return Convert.ToBoolean(obj);
        }

        public static string[] ToStringSingleArray(this DataTable dt, string column)
        {
            string[] array = dt.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
            return array;
        }

        public static string ToSelectIN(this DataTable dt, string column)
        {
            string result = "(";
            if(dt.Rows.Count > 0)
            {
                int x = 1;
                foreach (DataRow row in dt.Rows)
                {
                    StringBuilder sb = new StringBuilder(row[column].ToString());                    
                    if (x == dt.Rows.Count)
                    {
                        result += "'" + sb.ToString() + "')";
                    }
                    else
                    {
                        result += "'" + sb.ToString() + "',";
                    }
                    x++;
                }
            }
            return result;
        }

        public static string ToSelectIN(this DataTable dt, string column, string dateFormat)
        {
            string result = "(";
            if (dt.Rows.Count > 0)
            {
                int x = 1;
                foreach (DataRow row in dt.Rows)
                {
                    string f = Convert.ToDateTime(row[column]).ToString(dateFormat);
                    StringBuilder sb = new StringBuilder(f);
                    if (x == dt.Rows.Count)
                    {
                        result += "'" + sb.ToString() + "')";
                    }
                    else
                    {
                        result += "'" + sb.ToString() + "',";
                    }
                    x++;
                }
            }
            return result;
        }

        public static string ToSelectIN(this List<object> list)
        {
            string result = "(";
            int x = 1;
            foreach(var i in list)
            {
                StringBuilder sb = new StringBuilder(i.ToString());
                if (x == list.Count)
                {
                    result += "'" + sb.ToString() + "')";
                }
                else
                {
                    result += "'" + sb.ToString() + "',";
                }
                x++;
            }
            return result;
        }

        public static string ToSelectIN(this List<int> list)
        {
            string result = "(";
            int x = 1;
            foreach (var i in list)
            {
                StringBuilder sb = new StringBuilder(i.ToString());
                if (x == list.Count)
                {
                    result += "'" + sb.ToString() + "')";
                }
                else
                {
                    result += "'" + sb.ToString() + "',";
                }
                x++;
            }
            return result;
        }

        public static object[] ToObjectSingleArray(this DataTable dt, string column)
        {
            object[] array = dt.Rows.OfType<DataRow>().Select(k => k[0]).ToArray();                 
            return array;
        }

        public static int[] ToIntSingleArray(this DataTable dt, string column)
        {
            int[] array = dt.Rows.OfType<DataRow>().Select(k => (int)k[0]).ToArray();
            return array;
        }

        public static string ToTitleCase(this string str, string country="id-ID")
        {
            if (str == null) return string.Empty;
            System.Globalization.TextInfo culture = new System.Globalization.CultureInfo(country, false).TextInfo;
            return culture.ToTitleCase(str);
        }

        public static List<T> ToModel<T>(this DataTable dataTable)
        {
            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dataTable.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                            throw new Exception();
                        }
                    }
                }
                return objT;
            }).ToList();
        }

        public static DataTable ToDataTable(this IEnumerable<object> data)
        {
            DataTable dt = new DataTable();
            using (var reader = ObjectReader.Create(data.ToArray()))
            {
                dt.Load(reader);
            }
            return dt;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            PropertyInfo[] propertyInfo = data.GetType().GetProperties();
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataSet ToDataSet(string[,] input)
        {
            var dataSet = new DataSet();
            var dataTable = dataSet.Tables.Add();
            var iFila = input.GetLongLength(0);
            var iCol = input.GetLongLength(1);

            //Fila
            for (var f = 1; f < iFila; f++)
            {
                var row = dataTable.Rows.Add();
                //Columna
                for (var c = 0; c < iCol; c++)
                {
                    if (f == 1) dataTable.Columns.Add(input[0, c]);
                    row[c] = input[f, c];
                }
            }
            return dataSet;
        }

        public static DataTable ToDataTable(this Type enumType, string column_name1 = null , string column_name2 = null)
        {
            DataTable table = new DataTable();

            //Get the type of ENUM for DataColumn
            if (column_name1 == null)
            {
                table.Columns.Add("Id", Enum.GetUnderlyingType(enumType));
            }
            else
            {
                table.Columns.Add(column_name1, Enum.GetUnderlyingType(enumType));
            }

            //Column that contains the Captions/Keys of Enum
            if (column_name2 == null)
            {
                table.Columns.Add("Description", typeof(string));
            }
            else
            {
                table.Columns.Add(column_name2, typeof(string));
            }
                                   

            //Add the items from the enum:
            foreach (Enum name in Enum.GetValues(enumType))
            {
                table.Rows.Add(Enum.Parse(enumType, name.ToString()), name.GetDescription());
            }

            return table;
        }

        public static string GetDescription(this Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
                else
                {
                    return genericEnumType.Name;
                }
            }
            return GenericEnum.ToString();
        }

        public static string ToDelimiter(this IList<object> data, string delimiter, bool useQuote = false)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for(int x=0; x <= data.Count-1; x++)
            {
                if(x != data.Count - 1)
                {
                    if (useQuote)
                    {
                        builder.Append("\"").Append(data[x].ToString()).Append("\"").Append(delimiter);
                    }
                    else
                    {
                        builder.Append(data[x].ToString()).Append(delimiter);
                    }
                    
                }
                else
                {
                    if (useQuote)
                    {
                        builder.Append("\"").Append(data[x].ToString()).Append("\"");
                    }
                    else
                    {
                        builder.Append(data[x].ToString());
                    }
                    
                }
                
            }
            return builder.ToString();
        }

        public static string ToDelimiter(this int[] data, string delimiter, bool useQuote = false)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int x = 0; x <= data.Count() - 1; x++)
            {
                if (x != data.Count() - 1)
                {
                    if (useQuote)
                    {
                        builder.Append("\"").Append(data[x].ToString()).Append("\"").Append(delimiter);
                    }
                    else
                    {
                        builder.Append(data[x].ToString()).Append(delimiter);
                    }
                }
                else
                {
                    if (useQuote)
                    {
                        builder.Append("\"").Append(data[x].ToString()).Append("\"");
                    }
                    else
                    {
                        builder.Append(data[x].ToString());
                    }
                }
            }
            return builder.ToString();
        }

        public static string ToDelimiterWithBracket(this int[] data, string delimiter, bool useQuote = false)
        {
            string output = Converter.ToDelimiter(data, delimiter, useQuote);
            return "(" + output + ")";
        }

        public static List<T> ToList<T>(this DataTable dt)
        {
            string json = JsonConvert.SerializeObject(dt);
            List<T> data = JsonConvert.DeserializeObject<List<T>>(json);
            return data;
        }

        
        public static string To64Byte(this string Text)
        {
            if (Text == null)
            {
                return null;
            }
            var plainTextBytes = Encoding.UTF8.GetBytes(Text);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string From64Byte(string EncodedData)
        {
            if (EncodedData == null)
            {
                return null;
            }
            var base64EncodedBytes = Convert.FromBase64String(EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}