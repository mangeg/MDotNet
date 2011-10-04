namespace MUtils.Services.Contracts
{
	using System.Collections.ObjectModel;

	public interface IDocking
	{
		IAvalonDockViewModel ActivePane { get; }
		IAvalonDockViewModel ActiveDocument { get; }
		ObservableCollection<IAvalonDockViewModel> Panes { get; }
		ObservableCollection<IAvalonDockViewModel> Documents { get; }
		void Initialize( AvalonDockHost host );
	}
}