using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Framework.Autofac
{
    public class AutofacHelper
    {
        #region 内部枚举

        public enum LifeCycleEnum
        {
            /// <summary>
            /// 对每一个依赖或每一次调用创建一个新的唯一的实例。这也是默认的创建实例的方式。
            /// Configure the component so that every dependent component or call to Resolve() gets a new, unique instance (default.)
            /// </summary>
            InstancePerDependency = 10,

            /// <summary>
            /// 在一个生命周期域中，每一个依赖或调用创建一个单一的共享的实例，且每一个不同的生命周期域，实例是唯一的，不共享的。
            /// Configure the component so that every dependent component or call to Resolve() within a single ILifetimeScope gets the same, shared instance. Dependent components in different lifetime scopes will get different instances.
            /// </summary>
            InstancePerLifetimeScope = 20,

            /// <summary>
            /// 在一个做标识的生命周期域中，每一个依赖或调用创建一个单一的共享的实例。打了标识了的生命周期域中的子标识域中可以共享父级域中的实例。若在整个继承层次中没有找到打标识的生命周期域，则会抛出异常：DependencyResolutionException
            /// Configure the component so that every dependent component or call to Resolve() within a ILifetimeScope tagged with any of the provided tags value gets the same, shared instance. Dependent components in lifetime scopes that are children of the tagged scope will share the parent's instance. If no appropriately tagged scope can be found in the hierarchy an DependencyResolutionException is thrown.
            /// </summary>
            InstancePerMatchingLifetimeScope = 30,

            /// <summary>
            /// 在一个生命周期域中所拥有的实例创建的生命周期中，每一个依赖组件或调用Resolve()方法创建一个单一的共享的实例，并且子生命周期域共享父生命周期域中的实例。若在继承层级中没有发现合适的拥有子实例的生命周期域，则抛出异常：DependencyResolutionException。
            /// Configure the component so that every dependent component or call to Resolve() within a ILifetimeScope created by an owned instance gets the same, shared instance. Dependent components in lifetime scopes that are children of the owned instance scope will share the parent's instance. If no appropriate owned instance scope can be found in the hierarchy an DependencyResolutionException is thrown.
            /// </summary>
            InstancePerOwned = 40,

            /// <summary>
            /// 每一次依赖组件或调用Resolve()方法都会得到一个相同的共享的实例。其实就是单例模式。
            /// Configure the component so that every dependent component or call to Resolve() gets the same, shared instance.
            /// </summary>
            SingleInstance = 50,

            /// <summary>
            /// 在一次Http请求上下文中,共享一个组件实例。
            /// </summary>
            InstancePerRequest = 60
        }

        #endregion

        private static ContainerBuilder builder = new ContainerBuilder();

        /// <summary>
        /// 注册接口
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TClass">接口的实际实现</typeparam>
        /// <returns></returns>
        public static void Register<TInterface, TClass>(LifeCycleEnum lifeCycle = LifeCycleEnum.InstancePerDependency)
        {
            var result = builder.RegisterType<TClass>().As<TInterface>();

            switch (lifeCycle)
            {
                case LifeCycleEnum.InstancePerDependency:
                default:
                    result.InstancePerDependency();
                    break;
                case LifeCycleEnum.InstancePerLifetimeScope:
                    result.InstancePerLifetimeScope();
                    break;
                case LifeCycleEnum.InstancePerMatchingLifetimeScope:
                    result.InstancePerMatchingLifetimeScope();
                    break;
                case LifeCycleEnum.InstancePerOwned:
                    result.InstancePerOwned(typeof(TInterface));
                    break;
                case LifeCycleEnum.SingleInstance:
                    result.SingleInstance();
                    break;
                case LifeCycleEnum.InstancePerRequest:
                    result.InstancePerRequest();
                    break;
            }
        }

        /// <summary>
        /// 获取接口实例
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <returns></returns>
        public static TInterface Resolve<TInterface>()
        {
            var result = builder.Build().Resolve<TInterface>();
            return result;
        }
    }
}
