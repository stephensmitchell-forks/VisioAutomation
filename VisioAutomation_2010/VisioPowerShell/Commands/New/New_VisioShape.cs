﻿using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;
using VA=VisioAutomation;
using SMA = System.Management.Automation;

namespace VisioPowerShell.Commands
{
    [SMA.CmdletAttribute(SMA.VerbsCommon.New, "VisioShape")]
    public class New_VisioShape : VisioCmdlet
    {
        [SMA.ParameterAttribute(Position = 0, Mandatory = true)]
        public IVisio.Master[] Masters { get; set; }

        [SMA.ParameterAttribute(Position = 1, Mandatory = true)]
        public double [] Points { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public string[] Names { get; set; }

        [SMA.ParameterAttribute(Mandatory = false)]
        public SMA.SwitchParameter NoSelect=false;

        protected override void ProcessRecord()
        {
            this.WriteVerbose("NoSelect: {0}", this.NoSelect);

            var points = VisioAutomation.Drawing.Point.FromDoubles(this.Points).ToList();
            var shape_ids = this.client.Master.Drop(this.Masters, points);

            var page = this.client.Page.Get();
            var shape_objects = VisioAutomation.Shapes.ShapeHelper.GetShapesFromIDs(page.Shapes, shape_ids);

          // If Names is not empty... assign it to the shape
             if (this.Names != null)
             {
                 int up_to = System.Math.Min(shape_objects.Count, this.Names.Length);
                 for (int i = 0; i < up_to; i++)
                 {
                     string cur_name = this.Names[i];
                     if (cur_name != null)
                     {
                         var cur_shape = shape_objects[i];
                         cur_shape.NameU = cur_name;                         
                     }
                 }
             }

            this.client.Selection.None();

            if (!this.NoSelect)
            {
                // Select the Shapes
                ((SMA.Cmdlet) this).WriteVerbose("Selecting");
                this.client.Selection.Select(shape_objects);
            }


            this.WriteObject(shape_objects, false);
        }
    }
}