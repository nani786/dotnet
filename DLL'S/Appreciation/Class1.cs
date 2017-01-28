using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AjaxControlToolkit.HTMLEditor;
/// <summary>
/// Summary description for Class1
/// </summary>
/// 

namespace TestEditor
{
    public class Class1 : Editor
    {
        public Class1()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        protected override void FillTopToolbar()
        {

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold());
         
            
        }

        protected override void FillBottomToolbar()
        {

            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignMode());

            BottomToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.PreviewMode());

        }
    }
}