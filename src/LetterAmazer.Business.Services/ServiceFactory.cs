using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace LetterAmazer.Business.Services
{
    public class ServiceFactory
    {
        private static ServiceFactory instance = new ServiceFactory();
        private IWindsorContainer container = new WindsorContainer();

        private ServiceFactory()
        {
            
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public static IWindsorContainer Container
        {
            get { return instance.container; }
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return instance.container.Resolve<T>();
        }

        /// <summary>
        /// Gets all this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> GetAll<T>()
        {
            return instance.container.ResolveAll<T>();
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T Get<T>(string name)
        {
            return (T)instance.container.Resolve<T>(name);
        }

        /// <summary>
        /// Gets all the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IList<T> GetAll<T>(string name)
        {
            return instance.container.ResolveAll<T>(name);
        }
    }
}
