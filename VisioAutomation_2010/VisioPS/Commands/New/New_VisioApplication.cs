using VAS=VisioAutomation.Scripting;
using SMA = System.Management.Automation;

namespace VisioPS.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.New, "VisioApplication")]
    public class New_VisioApplication : VisioPSCmdlet
    {
        protected override void ProcessRecord()
        {
            var scriptingsession = this.ScriptingSession;
            var app = scriptingsession.Application.New();

            // Currently we do not send the application back to the pipeline this.WriteObject(app); 
            // The reasins is that in the past we have seen that doing then can later cause the Visio 
            // application to have an error when it shuts down. it's not clear why
            
            // TODO: investigate why calling write-object and returning app can cause the visio application to have an error when it shuts down
        }
    }
}