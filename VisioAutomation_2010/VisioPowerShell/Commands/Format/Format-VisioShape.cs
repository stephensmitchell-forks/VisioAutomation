using VA = VisioAutomation;
using SMA = System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.Format, "VisioShape")]
    public class Format_VisioShape : VisioCmdlet
    {
        [SMA.Parameter(Mandatory = false)]
        public double NudgeX { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public double NudgeY { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter DistributeHorizontal { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter DistributeVertical { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public VerticalAlignment AlignVertical = VerticalAlignment.None;

        [SMA.Parameter(Mandatory = false)]
        public HorizontalAlignment AlignHorizontal = HorizontalAlignment.None;

        [SMA.Parameter(Mandatory = false)]
        public IVisio.Shape[] Shapes;

        protected override void ProcessRecord()
        {
            var scriptingsession = this.ScriptingSession;
            if (this.NudgeX != 0.0 || this.NudgeY != 0.0)
            {
                scriptingsession.Arrange.Nudge(this.Shapes, this.NudgeX, this.NudgeY);                
            }

            if (this.DistributeHorizontal)
            {
                scriptingsession.Arrange.Distribute(this.Shapes, VA.Drawing.Axis.XAxis);
            }

            if (this.DistributeVertical)
            {
                scriptingsession.Arrange.Distribute(this.Shapes, VA.Drawing.Axis.YAxis);
            }

            if (this.AlignVertical != VerticalAlignment.None)
            {
                scriptingsession.Arrange.Align(this.Shapes, (VA.Drawing.AlignmentVertical)AlignVertical);
            }

            if (this.AlignHorizontal != HorizontalAlignment.None)
            {
                scriptingsession.Arrange.Align(this.Shapes, (VA.Drawing.AlignmentHorizontal)AlignHorizontal);
            }

        }
    }
}