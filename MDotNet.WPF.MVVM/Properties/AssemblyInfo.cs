﻿using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using MDotNet.Common;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle( "MDotNet.WPF.MVVM" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( AppInfo.CompanyName )]
[assembly: AssemblyProduct( "MDotNet.WPF.MVVM" )]
[assembly: AssemblyCopyright( AppInfo.CopyrightNote )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid( "3f11334d-161c-4145-bd20-566007a42908" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion( "1.0.0.0" )]
[assembly: AssemblyFileVersion( "1.0.0.0" )]

[assembly: XmlnsDefinition( AppInfo.XamlNamepsace, "MDotNet.WPF.MVVM.View" )]
[assembly: XmlnsPrefix( AppInfo.XamlNamepsace, "mdot" )]