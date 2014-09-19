using SMA = System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.New, "VisioGroup")]
    public class New_VisioGroup : VisioCmdlet
    {
        protected override void ProcessRecord()
        {
            var scriptingsession = this.ScriptingSession;
            var group = scriptingsession.Arrange.Group();
            this.WriteObject(group);
        }
    }
}