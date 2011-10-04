namespace MUtils
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using AvalonDock;
	using MDotNet.WPF.MVVM;
	using MDotNet.WPF.MVVM.View;

	public interface IAvalonDockViewModel
	{
		String PaneTitle { get; }
		String PaneName { get; }
		bool IsPaneVisible { get; set; }
	}

	public sealed class DocumentClosingEventArgs : EventArgs
	{
		internal DocumentClosingEventArgs( IAvalonDockViewModel document )
		{
			Document = document;
			Cancel = false;
		}

		public IAvalonDockViewModel Document { get; private set; }
		public bool Cancel { get; set; }
	}

	public partial class AvalonDockHost : UserControl
	{
		public static readonly DependencyProperty PanesProperty =
			DependencyProperty.Register( "Panes", typeof( IList<IAvalonDockViewModel> ), typeof( AvalonDockHost ),
										 new FrameworkPropertyMetadata( DockableContentsOnPropertyChanged ) );
		public static readonly DependencyProperty DocumentsProperty =
			DependencyProperty.Register( "Documents", typeof( IList<IAvalonDockViewModel> ), typeof( AvalonDockHost ),
										 new FrameworkPropertyMetadata( DocumentsOnPropertyChanged ) );
		public static readonly DependencyProperty ActivePaneProperty =
			DependencyProperty.Register( "ActivePane", typeof( IAvalonDockViewModel ), typeof( AvalonDockHost ),
			                             new FrameworkPropertyMetadata( null,
			                                                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );
		public static readonly DependencyProperty ActiveDocumentProperty =
			DependencyProperty.Register( "ActiveDocument", typeof( IAvalonDockViewModel ), typeof( AvalonDockHost ),
			                             new FrameworkPropertyMetadata( null,
			                                                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );
		public static readonly DependencyProperty IsPaneVisibleProperty =
			DependencyProperty.RegisterAttached( "IsPaneVisible", typeof( bool ), typeof( AvalonDockHost ),
			                                     new FrameworkPropertyMetadata( true,
			                                                                    FrameworkPropertyMetadataOptions.
			                                                                    	BindsTwoWayByDefault, OnIsPanelVisibleChange ) );

		private readonly IViewBinder _binder;
		private readonly Dictionary<IAvalonDockViewModel, ManagedContent> _contents =
			new Dictionary<IAvalonDockViewModel, ManagedContent>();
		private readonly IViewLocator _locator;

		public AvalonDockHost()
		{
			InitializeComponent();
			_binder = IoC.GetInstance<IViewBinder>();
			_locator = IoC.GetInstance<IViewLocator>();
		}

		public IList<IAvalonDockViewModel> Panes
		{
			get { return ( IList<IAvalonDockViewModel> )GetValue( PanesProperty ); }
			set { SetValue( PanesProperty, value ); }
		}
		public IList<IAvalonDockViewModel> Documents
		{
			get { return ( IList<IAvalonDockViewModel> )GetValue( DocumentsProperty ); }
			set { SetValue( DocumentsProperty, value ); }
		}
		public IAvalonDockViewModel ActivePane
		{
			get { return ( IAvalonDockViewModel )GetValue( ActivePaneProperty ); }
			set { SetValue( ActivePaneProperty, value ); }
		}
		public IAvalonDockViewModel ActiveDocument
		{
			get { return ( IAvalonDockViewModel )GetValue( ActiveDocumentProperty ); }
			set { SetValue( ActiveDocumentProperty, value ); }
		}
		public DockingManager DockingManager
		{
			get { return dockingManager; }
		}

		public static void SetIsPaneVisible( UIElement element, bool value )
		{
			element.SetValue( IsPaneVisibleProperty, value );
		}
		public static bool GetIsPaneVisible( UIElement element )
		{
			return ( bool )element.GetValue( IsPaneVisibleProperty );
		}
		public static void OnIsPanelVisibleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var avalonDockContent = d as ManagedContent;
			if ( avalonDockContent != null )
			{
				var isVisible = ( bool )e.NewValue;
				if ( isVisible )
				{
					avalonDockContent.Show();
				}
				else
				{
					avalonDockContent.Hide();
				}
			}
		}

		public event EventHandler<EventArgs> AvalonDockLoaded;
		public event EventHandler<DocumentClosingEventArgs> DocumentClosing;

		private void ResetDocumentsOrPanes( object sender )
		{
			if ( sender == this.Documents )
			{
				foreach ( var item in _contents.ToArray() )
				{
					if ( item.Value is DocumentContent )
					{
						RemovePanel( item.Key );
					}
				}
			}
			else if ( sender == this.Panes )
			{
				foreach ( var item in _contents.ToArray() )
				{
					if ( item.Value is DockableContent )
					{
						RemovePanel( item.Key );
					}
				}
			}
			else
			{
				throw new ApplicationException( "Unexpected CollectionChanged event sender: " + sender.GetType().Name );
			}
		}

		private void AddPanels( IEnumerable panels, bool isDocument )
		{
			foreach ( var panel in panels )
			{
				AddPanel( panel as IAvalonDockViewModel, isDocument );
			}
		}
		private void AddPanel( IAvalonDockViewModel panel, bool isDocument )
		{
			ManagedContent managedContent = null;
			managedContent = isDocument ? ( ManagedContent )new DocumentContent() : new DockableContent();
			managedContent.DataContext = panel;
			managedContent.SetBinding( ManagedContent.TitleProperty, new Binding( "PaneTitle" ) );
			managedContent.SetBinding( NameProperty, new Binding( "PaneName" ) );
			managedContent.SetBinding( IsPaneVisibleProperty, new Binding( "IsPanelVisible" ) );

			var view = _locator.LocateForModel( panel );
			_binder.Bind( panel, view );
			managedContent.Content = view;

			managedContent.Closed += OnManagedContentClosed;

			_contents[ panel ] = managedContent;
			
			if ( isDocument )
				managedContent.Closing += OnDocumentClosing;
			else
				( managedContent as DockableContent ).StateChanged += OnStateChanged;
			
			managedContent.Show( DockingManager );
			managedContent.Activate();
		}

		private void RemovePanels( IEnumerable panels )
		{
			foreach ( var panel in panels )
			{
				RemovePanel( panel as IAvalonDockViewModel );
			}
		}
		private void RemovePanel( IAvalonDockViewModel panel )
		{
			ManagedContent managedContent = null;
			if ( _contents.TryGetValue( panel, out managedContent ) )
			{
				managedContent.Close();
			}
		}

		private static void DockableContentsOnPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var c = d as AvalonDockHost;
			if ( c == null ) return;

			if ( e.OldValue != null )
			{
				var oldValues = e.OldValue as IList<IAvalonDockViewModel>;
				c.RemovePanels( oldValues );
				var observableCollection = oldValues as INotifyCollectionChanged;
				if ( observableCollection != null )
				{
					observableCollection.CollectionChanged -= c.DockableContentsOnCollectionChanged;
				}
			}
			if ( e.NewValue != null )
			{
				var newValues = e.NewValue as IList<IAvalonDockViewModel>;
				c.AddPanels( newValues as IList, false );
				var observableCollection = newValues as INotifyCollectionChanged;
				if ( observableCollection != null )
				{
					observableCollection.CollectionChanged += c.DockableContentsOnCollectionChanged;
				}
			}
		}
		private void DockableContentsOnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e.Action == NotifyCollectionChangedAction.Reset )
			{
				ResetDocumentsOrPanes( sender );
			}
			else
			{
				if ( e.OldItems != null )
				{
					RemovePanels( e.NewItems as IEnumerable );
				}
				if ( e.NewItems != null )
				{
					AddPanels( e.NewItems, false );
				}
			}
		}

		private static void DocumentsOnPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var c = d as AvalonDockHost;
			if ( c == null ) return;

			if ( e.OldValue != null )
			{
				var oldValues = e.OldValue as IList<IAvalonDockViewModel>;
				c.RemovePanels( oldValues );
				var observableCollection = oldValues as INotifyCollectionChanged;
				if ( observableCollection != null )
				{
					observableCollection.CollectionChanged -= c.DocumentsOnCollectionChanged;
				}
			}
			if ( e.NewValue != null )
			{
				var newValues = e.NewValue as IList<IAvalonDockViewModel>;
				c.AddPanels( e.NewValue as IList, true );
				var observableCollection = newValues as INotifyCollectionChanged;
				if ( observableCollection != null )
				{
					observableCollection.CollectionChanged += c.DocumentsOnCollectionChanged;
				}
			}
		}
		private void DocumentsOnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e.Action == NotifyCollectionChangedAction.Reset )
			{
				ResetDocumentsOrPanes( sender );
			}
			else
			{
				if ( e.OldItems != null )
				{
					RemovePanels( e.OldItems as IEnumerable );
				}
				if ( e.NewItems != null )
				{
					AddPanels( e.NewItems, true );
				}
			}
		}

		private void OnManagedContentClosed( object sender, EventArgs e )
		{
			var managedContent = sender as ManagedContent;
			var content = managedContent.DataContext as IAvalonDockViewModel;

			_contents.Remove( content );

			managedContent.Closed -= OnManagedContentClosed;

			var documentContent = managedContent as DocumentContent;
			if ( documentContent != null )
			{
				Documents.Remove( content );
				if ( ActiveDocument == content )
					ActiveDocument = null;
			}
			var dockableContent = managedContent as DockableContent;
			if ( dockableContent != null )
			{
				Panes.Remove( content );
				if ( ActivePane == content )
					ActivePane = null;
			}
		}
		private void OnDocumentClosing( object sender, CancelEventArgs e )
		{
			var documentContent = sender as DocumentContent;
			if ( documentContent == null ) return;
			var document = documentContent.DataContext as IAvalonDockViewModel;
			if ( document == null ) return;

			if ( DocumentClosing != null )
			{
				var args = new DocumentClosingEventArgs( document );
				DocumentClosing( this, args );

				if ( args.Cancel )
				{
					e.Cancel = true;
					return;
				}
			}

			documentContent.Closing -= OnDocumentClosing;
		}
		private void OnStateChanged( object sender, RoutedEventArgs e )
		{
			var dockableContent = sender as DockableContent;
			SetIsPaneVisible( dockableContent, dockableContent.State != DockableContentState.Hidden );
		}

		private void AvalonDock_Loaded( object sender, RoutedEventArgs e )
		{
			if ( AvalonDockLoaded != null )
			{
				AvalonDockLoaded( this, EventArgs.Empty );
			}
		}
	}
}