namespace Gseey.Framework.Common.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 反射帮助类
    /// </summary>
    public sealed class ReflectionHelper
    {
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

        /// <summary>
        /// 获取类型的属性集合
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfos(object obj)
        {
            var type = obj.GetType();
            var propInfoList = currentPropDict.GetOrAdd(type, t =>
            {
                var propList = t.GetProperties().AsEnumerable();

                return propList;
            });
            return propInfoList;
        }

        /// <summary>
        /// 获取类型的属性集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfos<T>()
        {
            var propInfoList = currentPropDict.GetOrAdd(typeof(T), t =>
            {
                var propList = t.GetProperties().AsEnumerable();

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
                var attrList = t.GetCustomAttributes();

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
                var memberList = t.GetMethods().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfos(object obj)
        {
            var type = obj.GetType();
            var memberInfoList = currentMemberDict.GetOrAdd(type, t =>
            {
                var memberList = t.GetMembers().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }

        /// <summary>
        /// 获取类型的自定义标签集合
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetFieldInfos(object obj)
        {
            var type = obj.GetType();
            var memberInfoList = currentFieldDict.GetOrAdd(type, t =>
            {
                var memberList = t.GetFields().AsEnumerable();

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
                var memberList = t.GetEvents().AsEnumerable();

                return memberList;
            });
            return memberInfoList;
        }
    }
}
