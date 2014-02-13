using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Runtime;

namespace VisitorPattern.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("LoadStart")]
    [Category("General")]
    [Description("This command creates the Projects element")]
    [CLSCompliant(false)]
    public class LoadStart : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<LoadStart>();

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IVisitorPattern CurrentElement { get; set; }

        /// <summary>
        /// This command creates the Projects element 
        /// </summary>
        /// <remarks></remarks>
        public override void Execute()
        {
            tracer.Warn(
                "Executing LoadProjects on current View defaultView");

            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();



            // Make initial trace statement for this command
            tracer.Info(
                "Executing LoadProjects on current View defaultView");

            if (CurrentElement.DefaultView.Projects == null)
            {
                CurrentElement.DefaultView.CreateProjects("Projects");
            }

        }
    }
}
