﻿

#pragma checksum "C:\Users\Helix\Documents\Cvo Antwerpen HBO Informatica 2015-2016\Project Van Gansewinkel\App\MachineInspectie\MachineInspectie\Views\MachineInspection\StartInspectionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A25A75E90EB599CD991481236DD207A0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MachineInspectie
{
    partial class StartInspectionPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 22 "..\..\..\Views\MachineInspection\StartInspectionPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnStart_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 23 "..\..\..\Views\MachineInspection\StartInspectionPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnReset_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 24 "..\..\..\Views\MachineInspection\StartInspectionPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnLocation_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 30 "..\..\..\Views\MachineInspection\StartInspectionPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnMatis_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 32 "..\..\..\Views\MachineInspection\StartInspectionPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListPickerFlyout)(target)).ItemsPicked += this.ListPickerMatis_ItemsPicked;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 26 "..\..\..\Views\MachineInspection\StartInspectionPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListPickerFlyout)(target)).ItemsPicked += this.ListPickerLocatie_ItemsPicked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


