using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using NuPattern;
using NuPattern.ComponentModel;
using NuPattern.Diagnostics;
using NuPattern.Runtime;
using NuPattern.Runtime.References;
using NuPattern.Runtime.ToolkitInterface;
using NuPattern.VisualStudio.Solution;

namespace VisitorPattern.Automation.Commands
{
    /// <summary>
    /// A custom command that performs some automation.
    /// </summary>
    [DisplayName("Load projects into solution builder")]
    [Category("General")]
    [Description("Loads projects from your solution into solution builder and syncs them.")]
    [CLSCompliant(false)]
    public class LoadProjects : NuPattern.Runtime.Command
    {
        private static readonly ITracer tracer = Tracer.Get<LoadProjects>();

        ///// <summary>
        ///// Gets or sets the solution.
        ///// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public ISolution Solution
        {
            get;
            set;
        }

        [Required]
        [Import(AllowDefault = true)]
        public IUriReferenceService UriReferenceService { get; set; }
        

       

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProjects CurrentElement
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this commmand.
        /// </summary>
        /// <remarks></remarks>
        public override void Execute()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();

            // Make initial trace statement for this command
            tracer.Info(
                "Executing LoadProjects on current element '{0}' ", this.CurrentElement.InstanceName);
            var iProjects = this.Solution.Find<IProject>();
         
            //SyncProjects
            SyncProjects(CurrentElement.MyProjects == null ? null : CurrentElement.MyProjects.ToList(), iProjects.ToArray());

        }

        private void SyncProjects(ICollection<IMyProject> loadedProjects, ICollection<IProject> dteProjects)
        {
            if (loadedProjects != null)
            {
                //remove unneed from loaded
                foreach (var i in loadedProjects)
                {
                    if (dteProjects.All(p => p.Name != i.InstanceName))
                    {
                        i.Delete();
                    }
                }
            }

            //add and update the ones we need
            foreach (var p in dteProjects)
            {
                var nuPatProject = loadedProjects == null ? null : loadedProjects.FirstOrDefault(v => v.InstanceName == p.Name);
                if (nuPatProject == null)
                {
                    var p1 = p;
                    CurrentElement.CreateMyProject(p1.Name, proj => SolutionArtifactLinkReference.AddReference(proj.AsCollection(), this.UriReferenceService.CreateUri(p1)));
                }
            }
        }


    }
}
