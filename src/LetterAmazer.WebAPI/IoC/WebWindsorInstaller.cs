using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Http.Controllers;

namespace LetterAmazer.WebAPI.IoC
{
    /// <summary>
    /// Installers are simply types that implement the IWindsorInstaller interface.
    /// The interface has a single method called Install. The method gets an instance of the container, which it can then register components
    /// with using fluent registration API.
    /// http://docs.castleproject.org/Windsor.Installers.ashx#codeInstallerFactorycode_class_4
    /// </summary>
    public class WebWindsorInstaller : IWindsorInstaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Singleton instances
            container.Register(
                Component.For<IControllerFactory>().ImplementedBy<WindsorControllerFactory>(),
                Component.For<IControllerActivator>().ImplementedBy<WindsorControllerActivator>());

            container.Register(Classes.FromThisAssembly().BasedOn<IController>().Configure(c => c.LifestyleTransient()));
            container.Register(Classes.FromThisAssembly().BasedOn<IHttpController>().Configure(c => c.LifestyleTransient()));


            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.Contacts").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.Homepage").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.Newss").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.Products").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.Services").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.ShoppingCarts").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
            //container.Register(AllTypes.FromAssemblyNamed("LevelFive.CleaningSystem.Modules.Members").BasedOn<IController>().Configure(c => c.LifeStyle.Transient));
        }
    }
}