namespace MDotNet.WPF.MVVM.ApplicationModel
{
	using System.ComponentModel.Composition;
	using System.Linq;
	using System.Windows;
	using MEF.Attributes;
	using Service.Contracts;
	using View;

#if SILVERLIGHT
	using System.Windows.Navigation;
#endif

	[PartCreationPolicy( CreationPolicy.NonShared )]
	[ExportService( typeof( IWindowManager ), ServiceType.Runtime )]
	internal class DefaultWindowManager : IWindowManager
	{
		private readonly IViewBinder _viewBinder;
		private readonly IViewLocator _viewLocator;

		[ImportingConstructor]
		public DefaultWindowManager( IViewLocator viewLocator, IViewBinder viewBinder )
		{
			_viewLocator = viewLocator;
			_viewBinder = viewBinder;
		}

		#region IWindowManager Members

		public void ShowWindow( object rootModel, bool isDialog, object context )
		{
#if SILVERLIGHT
			var navWindow = Application.Current.MainWindow as NavigationWindow;
			if ( navWindow != null )
			{
				//var window = CreatePage( rootModel, context );
				//navWindow.Navigate( window );
			}
			else
#endif
			{
				var window = CreateWindow( rootModel, false, context );
				window.Show();
			}
		}

		#endregion

		protected virtual Window CreateWindow( object rootModel, bool isDialog, object context )
		{
			var view = EnsureWindow( rootModel, _viewLocator.LocateForModel( rootModel ), isDialog );
			_viewBinder.Bind( rootModel, view );

			//ViewModelBinder.Bind( rootModel, view, context );

			/*var haveDisplayName = rootModel as IHaveDisplayName;
			if ( haveDisplayName != null && !view.HasBinding( Window.TitleProperty ) )
			{
				var binding = new Binding( "DisplayName" ) { Mode = BindingMode.TwoWay };
				view.SetBinding( Window.TitleProperty, binding );
			}*/

			//new WindowConductor( rootModel, view );

			return view;
		}

		protected virtual Window EnsureWindow( object model, object view, bool isDialog )
		{
			var window = view as Window;

			if ( window == null )
			{
				window = new Window
					{
						Content = view,
						SizeToContent = SizeToContent.WidthAndHeight
					};

				/*window.SetValue( View.IsGeneratedProperty, true );*/

				var owner = InferOwnerOf( window );
				if ( owner != null )
				{
					window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					window.Owner = owner;
				}
				else
				{
					window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
			}
			else
			{
				var owner = InferOwnerOf( window );
				if ( owner != null && isDialog )
					window.Owner = owner;
			}
			//Application.Current.MainWindow = window;
			return window;
		}

		/// <summary>
		/// Infers the owner of a new window being opened
		/// </summary>
		/// <param name="window">The window being opened</param>
		/// <returns>The inferred owner</returns>
		protected virtual Window InferOwnerOf( Window window )
		{
			if ( Application.Current == null ) return null;

			var active = Application.Current.Windows.OfType<Window>()
				.Where( x => x.IsActive )
				.FirstOrDefault();
			active = active ?? Application.Current.MainWindow;
			return active == window ? null : active;
		}
	}
}