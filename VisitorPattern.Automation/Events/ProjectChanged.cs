using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Runtime.Events;
using NuPattern.VisualStudio.Solution;

namespace VisitorPattern.Automation.Events
{
    /// <summary>
    /// The IOnProjectChanged event type is fired when the 
    /// </summary>
    public interface IOnProjectChanged: IObservable<IEvent<EventArgs>>, IObservableEvent
    {
    }

    /// <summary>
    /// The ProjectAdded event, that republishes the event for listening automation.
    /// </summary>
    [DisplayName("Project in solution is added, removed, or renamed")]
    [Category("General")]
    [Description("Raises the OnProjectChanged custom event.")]
    [Event(typeof(IOnProjectChanged))]
    [Export(typeof(IOnProjectChanged))]
    public class OnProjectChanged : IOnProjectChanged
    {
        private static readonly ITracer tracer = Tracer.Get<OnProjectChanged>();
        private IObservable<IEvent<EventArgs>> sourceEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnProjectAdded"/> class.
        /// </summary>
        [ImportingConstructor]
        public OnProjectChanged([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            Guard.NotNull(() => serviceProvider, serviceProvider);

            
            
            Dte = serviceProvider.GetService<EnvDTE.DTE>();
            Dte.Events.SolutionEvents.ProjectAdded += OnAdded();
            Dte.Events.SolutionEvents.ProjectRemoved += OnRemove();
            Dte.Events.SolutionEvents.ProjectRenamed += OnRename();

            tracer.Info("Projects have Changed");

             
            this.sourceEvent = WeakObservable.FromEvent<EventArgs>(
                handler => this.ProjectChanged += handler,
                handler => this.ProjectChanged -= handler);
        }

        private DTE Dte { get; set; }

        /// <summary>
        /// Defines the automation event.
        /// </summary>
        public event EventHandler<EventArgs> ProjectChanged = (sender, args) => { };

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        public IDisposable Subscribe(IObserver<IEvent<EventArgs>> observer)
        {
            return this.sourceEvent.Subscribe(observer);
        }

        /// <summary>
        /// Re-publishes the event for listening automation
        /// </summary>
        private _dispSolutionEvents_ProjectAddedEventHandler OnAdded()
        {
            return project => this.ProjectChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Re-publishes the event for listening automation
        /// </summary>
        private _dispSolutionEvents_ProjectRemovedEventHandler OnRemove()
        {
            return project => this.ProjectChanged(this, EventArgs.Empty);
        }


        private _dispSolutionEvents_ProjectRenamedEventHandler OnRename()
        {
            return (project, name) => this.ProjectChanged(this, EventArgs.Empty);
        }

    }
}
