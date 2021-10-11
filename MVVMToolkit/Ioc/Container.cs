using System;
using Ninject;

namespace PB.MVVMToolkit.Ioc
{
    /// <summary>
    /// Container class used for dependency injection
    /// Class inherits Ninject Nuget
    /// http://www.ninject.org/
    /// </summary>
    internal class Container
    {
        /// <summary>
        /// Kernel
        /// </summary>
        public IKernel Kernel
        {
            get;
            private set;
        }

        private static volatile Container instance = null;
        private static object syncRoot = new Object();

        private Container()
        {
            this.Kernel = new Ninject.StandardKernel();
            this.Kernel.Load(new Module());
        }

        /// <summary>
        /// Container Singleton
        /// </summary>
        internal static Container Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Container();
                    }
                }
                return instance;
            }
        }
    }
}
