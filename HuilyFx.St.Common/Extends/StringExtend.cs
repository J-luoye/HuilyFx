using System;
using System.Linq;
using System.Text;

namespace HuilyFx.St.Common.Extends
{
    /// <summary>
    /// String扩展类
    /// </summary>
    public static partial class StringExtend
    {
        /// <summary>
        /// 如果为null则返回String.Empty
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static string NullThenEmpty(this string source)
        {
            if (source == null)
            {
                return string.Empty;
            }
            return source;
        }

        /// <summary>
        /// 如果为null，则返回value的值
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string NullThen(this string source, string value)
        {
            if (source.IsNullOrEmpty())
            {
                return value;
            }
            return source;
        }

        /// <summary>
        /// 判断字符串是否为null或Empty
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 当值与when条件相同则返回then值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="when">比较值</param>
        /// <param name="then">返回值</param>
        /// <returns></returns>
        public static string WhenThen(this string source, string when, string then)
        {
            if (source == when)
            {
                return then;
            }
            return source;
        }

        /// <summary>
        /// 转换为小写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToLowerIfNoNull(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return source.ToLower();
        }

        /// <summary>
        /// 转换为大写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToUpperIfNoNull(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return source.ToUpper();
        }

        /// <summary>
        /// 是否匹配正则表达式参数
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pattert">regex参数</param>
        /// <returns></returns>
        public static bool IsMatch(this string source, string pattert)
        {
            source = source.NullThenEmpty();
            pattert = pattert.NullThenEmpty();
            return System.Text.RegularExpressions.Regex.IsMatch(source, pattert);
        }

        /// <summary>
        /// 匹配正则表示式
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pattert">regex参数</param>
        /// <returns></returns>
        public static string Match(this string source, string pattert)
        {
            source = source.NullThenEmpty();
            pattert = pattert.NullThenEmpty();
            return System.Text.RegularExpressions.Regex.Match(source, pattert).Value;
        }

        /// <summary>
        /// 匹配正则表示式
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pattert">regex参数</param>
        /// <returns></returns>
        public static string[] Matches(this string source, string pattert)
        {
            return System.Text.RegularExpressions.Regex
                .Matches(source.NullThenEmpty(), pattert.NullThenEmpty())
                .Cast<System.Text.RegularExpressions.Match>()
                .Select(item => item.Value)
                .ToArray();
        }

        /// <summary>
        /// 重复填充字符
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="totalLength">填充后的长度</param>
        /// <returns></returns>
        public static string Pad(this char source, int totalLength)
        {
            return string.Empty.PadRight(totalLength, source);
        }

        /// <summary>
        /// 以GB2312编码获取字符串的长度
        /// 一个中文占两个长度 英文一个长度
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static int LengthGB2312(this string source)
        {
            return Encoding.GetEncoding("gb2312").GetByteCount(source.NullThenEmpty());
        }

        /// <summary>
        /// 以GB2312编码切割字符串
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="lengthGB2312">保留的长度</param>
        /// <returns></returns>
        public static string SubstringGB2312(this string source, int lengthGB2312)
        {
            var encoding = Encoding.GetEncoding("gb2312");
            source = source.NullThenEmpty();

            if (lengthGB2312 >= source.LengthGB2312())
            {
                return source;
            }

            int index = 0;
            int length = 0;
            var chars = source.ToCharArray();

            while (length < lengthGB2312)
            {
                length += encoding.GetByteCount(chars, index, 1);
                if (length == lengthGB2312)
                {
                    return new string(chars, 0, index + 1);
                }
                else if (length > lengthGB2312)
                {
                    return new string(chars, 0, index);
                }
                index++;
            }
            return string.Empty;
        }

        /// <summary>
        /// 以GB2312编码切割字符串
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="lengthGB2312">保留的长度</param>
        /// <param name="padChar">填充字符</param>
        /// <returns></returns>
        public static string SubstringGB2312(this string source, int lengthGB2312, char padChar)
        {
            source = source.NullThenEmpty().SubstringGB2312(lengthGB2312);
            int count = lengthGB2312 - source.LengthGB2312();
            if (count == 0)
            {
                return source;
            }
            return string.Concat(source, padChar.Pad(count));
        }

        /// <summary>
        /// 字符串马赛克
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mask">马赛克字符</param>
        /// <param name="left">左边长度</param>
        /// <param name="length">打码长度</param>
        /// <returns></returns>
        public static string MaskAs(this string source, char mask, int left, int length)
        {
            if (source.IsNullOrEmpty())
            {
                return source;
            }
            var pattern = string.Format(@"(?<=^.{{{0}}}).{{{1}}}", left, length);
            return System.Text.RegularExpressions.Regex.Replace(source, pattern, mask.Pad(length));
        }

        /// <summary>
        /// 字符串长度省略
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length">按照gb2312编码长度</param>
        /// <returns></returns>
        public static string EllipsisGB2312(this string source, int length, string ellipsis = "...")
        {
            if (source.LengthGB2312() > length)
            {
                return source.SubstringGB2312(length) + ellipsis;
            }
            return source;
        }

        /// <summary>
        /// 获取和目标字符串的相似度
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="target">目标字符串</param>
        /// <returns></returns>
        public static decimal GetSimilarityWith(this string source, string target)
        {
            var Kq = 2m;
            var Kr = 1m;
            var Ks = 1m;

            var sourceArray = source.ToCharArray();
            var destArray = target.ToCharArray();

            //获取交集数量
            var q = sourceArray.Intersect(destArray).Count();
            var s = sourceArray.Length - q;
            var r = destArray.Length - q;
            return Kq * q / (Kq * q + Kr * r + Ks * s);
        }

        /// <summary>
        /// 转换为小写开头
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CamelCase(this string source)
        {
            if (source.IsNullOrEmpty())
            {
                return source;
            }
            var charArray = source.ToCharArray();
            charArray[0] = Char.ToLower(charArray[0]);
            return new string(charArray);
        }

        /// <summary>
        /// 转义Base64 字符
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string FixSUnBase64(this string base64)
        {
            return base64.Replace("+", "-").Replace("/", "_");
        }

        /// <summary>
        /// 修复 Base64 字符
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string FixDUnBase64(this string base64)
        {
            var str = base64.Replace("-", "+").Replace("_", "/");
            var padLength = 4 - str.Length % 4;
            if (padLength == 4)
            {
                return str;
            }
            var pad = new string('=', padLength);
            return str + pad;
        }
    }
}
