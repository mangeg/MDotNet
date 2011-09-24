using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MDotNet.WPF.ApplicationModel
{
	public class Bootstrapper
	{
		private readonly bool _useApplication;

		public Application Application { get; set; }

		public Bootstrapper(bool useApplication = true)
		{
			_useApplication = useApplication;
		}


		protected virtual void StartRuntime()
		{

		}
		protected virtual void OnStartup( object sender, StartupEventArgs e ){}
#if SILVERLIGHT
		/// <summary>
		/// Override this to add custom behavior for unhandled exceptions.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		protected virtual void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e) {}
#else
		/// <summary>
		/// Override this to add custom behavior for unhandled exceptions.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		protected virtual void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) { }
#endif
#if SILVERLIGHT && !WP7
        /// <summary>
        /// Locates the view model, locates the associate view, binds them and shows it as the root view.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="viewModelType">The view model type.</param>
        protected static void DisplayRootViewFor(Application application, Type viewModelType) {
            var viewModel = IoC.GetInstance(viewModelType, null);
            var view = ViewLocator.LocateForModel(viewModel, null, null);

            ViewModelBinder.Bind(viewModel, view, null);

            var activator = viewModel as IActivate;
            if(activator != null)
                activator.Activate();

            Mouse.Initialize(view);
            application.RootVisual = view;
        }
#else
        /// <summary>
        /// Locates the view model, locates the associate view, binds them and shows it as the root view.
        /// </summary>
        /// <param name="viewModelType">The view model type.</param>
        protected static void DisplayRootViewFor(Type viewModelType) {
            /*IWindowManager windowManager;

            try
            {
                windowManager = IoC.Get<IWindowManager>();
            }
            catch
            {
                windowManager = new WindowManager();
            }

            windowManager.ShowWindow(IoC.GetInstance(viewModelType, null));*/
        }
#endif
	}

#if!WP7
	public class Bootstrapper<TRootModel> : Bootstrapper
	{
		protected override void OnStartup( object sender, StartupEventArgs e )
		{
#if SILVERLIGHT
            DisplayRootViewFor(Application, typeof(TRootModel));
#else
			DisplayRootViewFor( typeof (TRootModel) );
#endif
		}
	}
#endif
}
