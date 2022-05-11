using System;
using System.Data;

namespace KesselRunFramework.DataAccess.Infrastructure.Extensions
{
    public static class DataReaderExtensions
    {
        public static string GetFirstString(this IDataReader source)
        {
            return source.GetString(0);
        }

        public static string GetSecondString(this IDataReader source)
        {
            return source.GetString(1);
        }

        public static byte GetFirstByte(this IDataReader source)
        {
            return source.GetByte(0);
        }

        public static byte GetSecondByte(this IDataReader source)
        {
            return source.GetByte(1);
        }

        public static int GetFirstInt(this IDataReader source)
        {
            return source.GetInt32(0);
        }

        public static int GetSecondInt(this IDataReader source)
        {
            return source.GetInt32(1);
        }

        public static short GetFirstShort(this IDataReader source)
        {
            return source.GetInt16(0);
        }

        public static short GetSecondShort(this IDataReader source)
        {
            return source.GetInt16(1);
        }

        public static DateTime GetFirstDate(this IDataReader source)
        {
            return source.GetDateTime(0);
        }

        public static DateTime GetSecondDate(this IDataReader source)
        {
            return source.GetDateTime(1);
        }
    }
}
