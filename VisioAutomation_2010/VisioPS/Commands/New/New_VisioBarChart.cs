using VisioAutomation.Models.Charting;
using VA = VisioAutomation;
using SMA = System.Management.Automation;

namespace VisioPS.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.New, "VisioBarChart")]
    public class New_VisioBarChart : VisioPS.VisioPSCmdlet
    {
        [System.Management.Automation.Parameter(Position = 0, Mandatory = true)]
        public double X0 { get; set; }

        [System.Management.Automation.Parameter(Position = 1, Mandatory = true)]
        public double Y0 { get; set; }

        [System.Management.Automation.Parameter(Position = 2, Mandatory = true)]
        public double X1 { get; set; }

        [System.Management.Automation.Parameter(Position = 3, Mandatory = true)]
        public double Y1 { get; set; }

        [SMA.Parameter(Mandatory = true)]
        public double[] Values;

        [SMA.Parameter(Mandatory = false)]
        public string[] Labels;

        protected override void ProcessRecord()
        {
            var rect = this.GetRectangle();
            var chart = new VA.Models.Charting.BarChart(rect);
            chart.DataPoints = new DataPointList(this.Values, this.Labels);
            this.WriteObject(chart);
        }

        protected VisioAutomation.Drawing.Rectangle GetRectangle()
        {
            return new VisioAutomation.Drawing.Rectangle(X0, Y0, X1, Y1);
        }
    }
}