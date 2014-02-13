using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Runtime;
using NuPattern.Runtime.ToolkitInterface;

namespace VisitorPattern.Automation.ValueProviders
{
    /// <summary>
    /// A custom value provider that provides a value to other automation classes.
    /// </summary>
    [DisplayName("Get namespace from parent")]
    [Category("General")]
    [Description("Grabs the namespace form the IClass' parent project")]
    [CLSCompliant(false)]
    public class NamespaceFromParent : NuPattern.Runtime.ValueProvider
    {
        private static readonly ITracer tracer = Tracer.Get<NamespaceFromParent>();


        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the provided result.
        /// </summary>
        /// <remarks></remarks>
        public override object Evaluate()
        {
            // Verify all [Required] and [Import]ed properties have valid values.
            this.ValidateObject();
            var klass = CurrentElement.As<IClass>();
            
            var project = klass.Parent;
            var result = project.Namespace;

            return result;
        }
    }
}
