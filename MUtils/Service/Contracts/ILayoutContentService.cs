using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace MUtils.Service.Contracts
{
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
