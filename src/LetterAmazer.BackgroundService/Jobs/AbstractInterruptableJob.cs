using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.BackgroundService.Jobs
{
    public abstract class AbstractInterruptableJob : IInterruptableJob
    {
        private object synRoot = new object();
        private bool stopped = false;

        public void Interrupt()
        {
            lock (synRoot)
            {
                stopped = true;
            }
        }

        public void Execute(IJobExecutionContext context)
        {
            if (stopped) return;

            ExecuteJob(context);
        }

        public bool IsStopped
        {
            get { return stopped; }
        }

        protected abstract void ExecuteJob(IJobExecutionContext context);
    }
}
