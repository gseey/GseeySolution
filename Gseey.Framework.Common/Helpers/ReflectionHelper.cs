using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gseey.Framework.Common.Helpers
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public sealed class ReflectionHelper
    {
        #region 私有属性

        /// <summary>
        /// 类型的属性集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> currentPropDict = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
        /// <summary>
        /// 类型的自定义标签集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<Attribute>> currentCustomAttributeDict = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();
        /// <summary>
        /// 类型的自定义标签集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<Attribute>> currentAttributeDict = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();
        /// <summary>
        /// 类型的属性集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<MemberInfo>> currentMemberDict = new ConcurrentDictionary<Type, IEnumerable<MemberInfo>>();
        /// <summary>
        /// 类型的字段集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<FieldInfo>> currentFieldDict = new ConcurrentDictionary<Type, IEnumerable<FieldInfo>>();
        /// <summary>
        /// 类型的方法集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<MethodInfo>> currentMethodDict = new ConcurrentDictionary<Type, IEnumerable<MethodInfo>>();
        /// <summary>
        /// 类型的方法集合
        /// </summary>
        private static ConcurrentDictionary<Type, IEnumerable<EventInfo>> currentEventDict = new ConcurrentDictionary<Type, IEnumerable<EventInfo>>();

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取类型的属性集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfos<T>()
        {
            var propInfoList = currentPropDict.GetOrAdd(typeof(T), t =>
            {
                var propList = t.GetType().GetProperties().AsEnumerable();

                return propList;
            });
            return propInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributes<T>()
        {
            var attrInfoList = currentCustomAttributeDict.GetOrAdd(typeof(T), t =>
            {
                var attrList = t.GetType().GetCustomAttributes();

                return attrList;
            });
            return attrInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetAttributes<T>()
        {
            var attrInfoList = currentCustomAttributeDict.GetOrAdd(typeof(T), t =>
            {
                var attrs = t.GetType().Attributes;
                var attrList = new List<Attribute>();


                return attrList;
            });
            return attrInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodInfos<T>()
        {
            var memberInfoList = currentMethodDict.GetOrAdd(typeof(T), t =>
            {
                var memberList = t.GetType().GetMethods().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfos<T>()
        {
            var memberInfoList = currentMemberDict.GetOrAdd(typeof(T), t =>
            {
                var memberList = t.GetType().GetMembers().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetFieldInfos<T>()
        {
            var memberInfoList = currentFieldDict.GetOrAdd(typeof(T), t =>
            {
                var memberList = t.GetType().GetFields().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<EventInfo> GetEventInfos<T>()
        {
            var memberInfoList = currentEventDict.GetOrAdd(typeof(T), t =>
            {
                var memberList = t.GetType().GetEvents().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }

        #endregion
    }
}
