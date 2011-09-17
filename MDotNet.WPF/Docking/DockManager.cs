using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MDotNet.WPF.Docking
{
	public class DockManager : ContentControl
	{
		static DockManager()
        {
			DefaultStyleKeyProperty.OverrideMetadata( typeof( DockManager ), 
				new FrameworkPropertyMetadata( typeof( DockManager ) ) );
        }

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var leftPanel = GetTemplateChild( "PART_LeftAnchorTabPanel" ) as Panel;
			if(leftPanel == null)
				throw new ArgumentException( "Template child not found", "PART_LeftAnchorTabPanel" );
		}
	}
}
