using PredicateLib;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


namespace System
{
    /// <summary>
    /// 提供搜索内容查询
    /// </summary>
    public static class Searcher
    {
        /// <summary>
        /// 获取排序字符串
        /// </summary>
        public static string OrderBy
        {
            get
            {
                return Searcher.GetValue("OrderBy");
            }
        }

        /// <summary>
        /// 获取Keyword
        /// </summary>
        public static string Keyword
        {
            get
            {
                return Searcher.GetValue("Keyword");
            }
        }

        /// <summary>
        /// 获取指定的值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies["search"];
            if (cookie == null)
            {
                return null;
            }

            var values = cookie.Values.GetValues(name);
            if (values == null || values.Length == 0)
            {
                return null;
            }

            var value = HttpUtility.UrlDecode(values.FirstOrDefault()).Trim();
            if (value.IsNullOrEmpty())
            {
                return null;
            }
            return value;
        }


        /// <summary>
        /// 返回默认为True的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return item => true;
        }

        /// <summary>
        /// 返回默认为False的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return item => false;
        }

        /// <summary>
        /// 表示默认为true的搜索结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Condition<T> Condition<T>()
        {
            var items = GetConditionItems<T>();
            return new Condition<T>(items);
        }

        /// <summary>
        /// 从Cookie获取搜索条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerable<ConditionItem<T>> GetConditionItems<T>()
        {
            foreach (var member in ConditionItem<T>.TypeProperties)
            {
                var value = Searcher.GetValue(member.Name);
                if (value.IsNullOrEmpty() == false)
                {
                    yield return new ConditionItem<T>(member, value, null);
                }
            }
        }
    }
}
