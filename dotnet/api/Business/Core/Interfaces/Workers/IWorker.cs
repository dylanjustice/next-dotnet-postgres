using AndcultureCode.CSharp.Core.Interfaces;
using Hangfire;
using DylanJustice.Demo.Business.Core.Models.Jobs;

namespace DylanJustice.Demo.Business.Core.Interfaces.Workers
{
    public interface IWorker
    {
        #region Properties

        string Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Action handles the background job process.  i.e. The work that needs to be completed.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IResult<object> Action(Job job, IJobCancellationToken cancellationToken);

        /// <summary>
        /// Execute handles updating the job entity, call Action, and tracking the Action result.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IResult<bool> Execute(long jobId, IJobCancellationToken cancellationToken);

        #endregion
    }
}
