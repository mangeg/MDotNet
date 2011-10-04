namespace MDotNet.WPF.MVVM.View
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Linq;
	using System.Windows;
	using System.Windows.Interactivity;
	using Action;
	using Extensions;
	using Logging;
	using MEF.Attributes;
	using Action = Action.Action;

	[PartCreationPolicy( CreationPolicy.Shared )]
	[ExportService( typeof( IViewBinder ), ServiceType.Both )]
	public class DefaultViewBinder : IViewBinder
	{
		private static readonly ILog _sLog = LogManager.GetLog();
		public static bool ApplyConventionsByDefault = true;

		public static readonly DependencyProperty ConventionsAppliedProperty =
			DependencyProperty.RegisterAttached(
				"ConventionsApplied",
				typeof( bool ),
				typeof( DefaultViewBinder ),
				null
				);

		private readonly IConventionManager _conventionManager;

		[ImportingConstructor]
		public DefaultViewBinder( IConventionManager conventionManager )
		{
			_conventionManager = conventionManager;
		}

		#region IViewBinder Members
		/// <summary>
		/// Binds the properties if an element to the view model.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <param name="viewModelType">Type of the view model.</param>
		/// <returns>A list of unmatched elements</returns>
		public IEnumerable<FrameworkElement> BindProperties( IEnumerable<FrameworkElement> elements, Type viewModelType )
		{
			var unmatchedElements = new List<FrameworkElement>();

			foreach ( var element in elements )
			{
				var property = viewModelType.GetPropertyCaseInsensitive( element.Name );

				if ( property == null )
				{
					unmatchedElements.Add( element );
					_sLog.Warn( "No matching property found for {0}", element.Name );
					continue;
				}

				var convetion = _conventionManager.GetElementConvention( element.GetType() );
				if ( convetion == null )
				{
					unmatchedElements.Add( element );
					_sLog.Warn( "No binding convetion found for {0}", element.GetType() );
					continue;
				}

				var applied = _conventionManager.SetBinding( viewModelType, element.Name, property, element, convetion );

				if ( applied )
				{
					if ( convetion.Extra != null )
						convetion.Extra( viewModelType, element.Name, property, element );
					//_sLog.Info( "Binding convetion applied on element {0}", element.Name );
				}
				else
				{
					_sLog.Warn( "Binding not applied, element {0} has existing binding.", element.Name );
				}
			}

			return unmatchedElements;
		}

		/// <summary>
		/// Binds the actions of an element to the view model.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <param name="viewModelType">Type of the view model.</param>
		/// <returns>A list of unmatched elements</returns>
		public IEnumerable<FrameworkElement> BindActions( IEnumerable<FrameworkElement> elements, Type viewModelType )
		{
			var unmatchedElements = elements.ToList();
			var methods = viewModelType.GetMethods();

			foreach ( var method in methods )
			{
				var foundCountrol = unmatchedElements.FindName( method.Name );
				if ( foundCountrol == null )
				{
					//_sLog.Info( "No action convension applied: No action element for {0}.", method.Name );
					continue;
				}

				unmatchedElements.Remove( foundCountrol );

				var triggers = Interaction.GetTriggers( foundCountrol );
				if ( triggers != null && triggers.Count > 0 )
				{
					//_sLog.Info( "No action convension applied: Interaction.Triggers allready set on {0}", foundCountrol.Name );
					continue;
				}

				var message = method.Name;
				var parameters = method.GetParameters();

				if ( parameters.Length > 0 )
				{
					message += "(";

					foreach ( var parameter in parameters )
					{
						var paramName = parameter.Name;
						var specialValue = "$" + paramName.ToLower();

						if ( MessageBinder.SpecialValues.ContainsKey( specialValue ) )
							paramName = specialValue;

						message += paramName + ",";
					}

					message = message.Remove( message.Length - 1, 1 );
					message += ")";
				}

				_sLog.Info( "Action Convention Applied: Action {0} on element {1}.", method.Name, message );
				Message.SetAttach( foundCountrol, message );
			}

			return unmatchedElements;
		}


		/// <summary>
		/// Binds the specified view model to the view.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <param name="view">The view.</param>
		public void Bind( object viewModel, DependencyObject view )
		{
			//_sLog.Info( "Binding {0} to {1}", viewModel, view );

			Action.SetTarget( view, viewModel );
			var viewAwareModel = viewModel as IViewAware;
			if ( viewAwareModel != null )
			{
				viewAwareModel.AttachView( view );
			}

			var element = View.GetFirstNonGeneratedView( view ) as FrameworkElement;
			if ( element == null )
				return;

			if ( !ShouldApplyConventions( element ) )
			{
				_sLog.Warn( "Skipping convetions for {0} and {1}", element, viewModel );
				return;
			}

			var viewModelType = viewModel.GetType();
			var elements = BindingScope.GetNamedElements( view );

			elements = BindActions( elements, viewModelType );
			elements = BindProperties( elements, viewModelType );

			view.SetValue( ConventionsAppliedProperty, true );
		}

		#endregion

		public static bool ShouldApplyConventions( FrameworkElement view )
		{
			var overriden = View.GetApplyConventions( view );
			return overriden.GetValueOrDefault( ApplyConventionsByDefault );
		}
	}
}