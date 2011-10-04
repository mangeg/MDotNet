namespace MDotNet.WPF.MVVM.ApplicationModel
{
	using System;
	using System.Windows;
	using System.Windows.Threading;
	using Logging;
	using MEF;
	using Service.Contracts;
	using View;

	/// <summary>
	///   Baseclass for bootstrapper.
	/// </summary>
	public class Bootstrapper
	{
		private readonly ILog _log = LogManager.GetLog();
		protected IServiceLocator _container;

		/// <summary>
		///   Initializes a new instance of the <see cref = "Bootstrapper" /> class.
		/// </summary>
		public Bootstrapper()
		{
			_log.Info( "{0} created", this.GetType().Name );

			if ( FrameworkConfiguration.IsInDesignMode )
				StartDesignTime();
			else
				StartRuntime();
		}

		/// <summary>
		///   Gets or sets the application.
		/// </summary>
		/// <value>
		///   The application.
		/// </value>
		public Application Application { get; set; }

		/// <summary>
		///   Gets the service locator container.
		/// </summary>
		public IServiceLocator Container
		{
			get { return _container; }
		}

		/// <summary>
		///   Starts the runtime version.
		/// </summary>
		public virtual void StartRuntime()
		{
			Application = Application.Current;
			Application.Startup += OnStartup;
			Application.Exit += OnExit;
			Application.DispatcherUnhandledException += OnUnhandledException;

			ConfigureBase();

			View.Initialize( IoC.GetInstance<IViewLocator>(), IoC.GetInstance<IViewBinder>() );
		}

		/// <summary>
		///   Starts the design time version.
		/// </summary>
		public virtual void StartDesignTime()
		{
			ConfigureBase();
			View.Initialize( IoC.GetInstance<IViewLocator>(), IoC.GetInstance<IViewBinder>() );
		}

		/// <summary>
		///   Configures this instance.
		///   Called during startup.
		/// </summary>
		public virtual void Configure() {}

		/// <summary>
		///   Called on Application.Startup.
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.Windows.StartupEventArgs" /> instance containing the event data.</param>
		public virtual void OnStartup( object sender, StartupEventArgs e )
		{
			DisplayRootView();
		}

		/// <summary>
		///   Called on Application.Exit.
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
		public virtual void OnExit( object sender, EventArgs e ) {}

		/// <summary>
		///   Called on unhandler exceptions for the application
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.Windows.Threading.DispatcherUnhandledExceptionEventArgs" /> instance containing the event data.</param>
		public virtual void OnUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e ) {}

		/// <summary>
		///   Displays the root view on startup.
		/// </summary>
		protected virtual void DisplayRootView() {}

		private void ConfigureBase()
		{
			_container = new MefLocator();
			Configure();
			_container.Initialize();
			IoC.Initialize( _container );
		}
	}

	public class Bootstrapper<TRootType> : Bootstrapper
	{
		protected override void DisplayRootView()
		{
			var viewModel = IoC.GetInstance( typeof( TRootType ) );
#if SILVERLIGHT
#else
			IoC.GetInstance<IWindowManager>().ShowWindow( viewModel, false, null );
#endif
		}
	}
}