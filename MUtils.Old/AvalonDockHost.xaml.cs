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

	public interface IAvalonDockViewModel
	{
		FrameworkElement View { get; }
		String PanelName { get; }
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

	/// <summary>
	///   Interaction logic for AvalonDockMVVM.xaml
	/// </summary>
	public partial class AvalonDockHost : UserControl
	{
		public static readonly DependencyProperty PanesProperty =
			DependencyProperty.Register( "Panes", typeof( IList<IAvalonDockViewModel> ), typeof( AvalonDockHost ),
			                             new FrameworkPropertyMetadata( DockableContents_PropertyChanged ) );

		public static readonly DependencyProperty DocumentsProperty =
			DependencyProperty.Register( "Documents", typeof( IList<IAvalonDockViewModel> ), typeof( AvalonDockHost ),
			                             new FrameworkPropertyMetadata( Documents_PropertyChanged ) );

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
			                                     new FrameworkPropertyMetadata( true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );

		private readonly Dictionary<IAvalonDockViewModel, ManagedContent> _contents =
			new Dictionary<IAvalonDockViewModel, ManagedContent>();

		public AvalonDockHost()
		{
			InitializeComponent();
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

		public event EventHandler<EventArgs> AvalonDockLoaded;
		public event EventHandler<DocumentClosingEventArgs> DocumentClosing;

		private static void DockableContents_PropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
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
					observableCollection.CollectionChanged -= c.dockableContents_CollectionChanged;
				}
			}
			if ( e.NewValue != null )
			{
				var newValues = e.NewValue as IList<IAvalonDockViewModel>;
				c.AddPanels( newValues as IList, false );
				var observableCollection = newValues as INotifyCollectionChanged;
				if ( observableCollection != null )
				{
					observableCollection.CollectionChanged += c.dockableContents_CollectionChanged;
				}
			}
		}

		private void dockableContents_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e.Action == NotifyCollectionChangedAction.Reset )
			{
				ResetDocumentsOrPanes( sender );
			}
			else
			{
				if ( e.OldItems != null )
				{
					RemovePanels( e.NewItems as IEnumerable<IAvalonDockViewModel> );
				}
				if ( e.NewItems != null )
				{
					AddPanels( e.NewItems, false );
				}
			}
		}

		private static void Documents_PropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
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
					observableCollection.CollectionChanged -= c.documents_CollectionChanged;
				}
			}
			if ( e.NewValue != null )
			{
				var newValues = e.NewValue as IList<IAvalonDockViewModel>;
				c.AddPanels( e.NewValue as IList, true );
				var observableCollection = newValues as INotifyCollectionChanged;
				if ( observableCollection != null )
				{
					observableCollection.CollectionChanged += c.documents_CollectionChanged;
				}
			}
		}

		private void documents_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e.Action == NotifyCollectionChangedAction.Reset )
			{
				ResetDocumentsOrPanes( sender );
			}
			else
			{
				if ( e.OldItems != null )
				{
					RemovePanels( e.NewItems as IEnumerable<IAvalonDockViewModel> );
				}
				if ( e.NewItems != null )
				{
					AddPanels( e.NewItems, true );
				}
			}
		}

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
			managedContent.SetBinding( ToolTipProperty, new Binding( "PaneTooltip" ) );
			managedContent.SetBinding( NameProperty, new Binding( "PanelName" ) );

			managedContent.Content = panel.View;
			managedContent.Closed += managedContent_Closed;

			_contents[ panel ] = managedContent;

			if ( isDocument )
				managedContent.Closing += document_Closing;
			else
				( managedContent as DockableContent ).StateChanged += dockableContent_StateChanged;

			managedContent.Show( DockingManager );
			managedContent.Activate();
		}

		private void RemovePanels( IEnumerable<IAvalonDockViewModel> panels )
		{
			foreach ( var panel in panels )
			{
				RemovePanel( panel );
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

		private void managedContent_Closed( object sender, EventArgs e )
		{
			var managedContent = sender as ManagedContent;
			var content = managedContent.DataContext as IAvalonDockViewModel;

			_contents.Remove( content );

			managedContent.Closed -= managedContent_Closed;

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

		private void document_Closing( object sender, CancelEventArgs e )
		{
			var documentContent = sender as DocumentContent;
			var document = documentContent.DataContext as IAvalonDockViewModel;

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

			documentContent.Closing -= document_Closing;
		}

		private void dockableContent_StateChanged( object sender, RoutedEventArgs e )
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