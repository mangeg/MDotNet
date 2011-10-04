namespace MUtils.Service.Contracts
{
	using System.Collections.ObjectModel;

	public interface ILayoutContentService
	{
		AvalonDockHost Host { get; }
		ObservableCollection<IAvalonDockViewModel> Panes { get; }
		ObservableCollection<IAvalonDockViewModel> Documents { get; }
		IAvalonDockViewModel ActivePane { get; }
		IAvalonDockViewModel ActiveDocument { get; }
		void Initialize( AvalonDockHost host );
	}
}