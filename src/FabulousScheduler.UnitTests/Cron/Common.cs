// ReSharper disable UnusedMethodReturnValue.Global
using FabulousScheduler.Core.Types;
using FabulousScheduler.Cron;
using FabulousScheduler.Cron.Abstraction;
using FabulousScheduler.Cron.Enums;
using FabulousScheduler.Cron.Interfaces;
using FabulousScheduler.Cron.Result;
// ReSharper disable ClassNeverInstantiated.Global

namespace Job.Core.Tests.Cron;

internal class CronJobRandomResult : BaseCronJob
{
	public TimeSpan JobSimulateWorkTime { get; }

	public CronJobRandomResult(string name, TimeSpan sleepDuration, TimeSpan jobSimulateWorkTime) : base(name,"test random result", sleepDuration)
	{
		JobSimulateWorkTime = jobSimulateWorkTime;
	}

	protected override async Task<JobResult<JobOk, JobFail>> ActionJob()
	{
		await Task.Delay(JobSimulateWorkTime);

		if (Random.Shared.Next(1, 10) % 2 == 0)
		{
			return new JobOk(this.Id);
		}

		return new JobFail(this.Id, CronJobFailEnum.FailedExecute, "test error", null);
	}
}

internal class CronJobOkResult : BaseCronJob
{
	public TimeSpan JobSimulateWorkTime { get; }

	public CronJobOkResult(string name, TimeSpan sleepDuration, TimeSpan jobSimulateWorkTime) : base(name,"test success", sleepDuration)
	{
		JobSimulateWorkTime = jobSimulateWorkTime;
	}

	protected override async Task<JobResult<JobOk, JobFail>> ActionJob()
	{
		await Task.Delay(JobSimulateWorkTime);
		return new JobOk(this.Id);
	}

}

internal class CronJobFailedExecuteResult : BaseCronJob
{
	public TimeSpan JobSimulateWorkTime { get; }

	public CronJobFailedExecuteResult(string name, TimeSpan sleepDuration, TimeSpan jobSimulateWorkTime) : base(name,"test error", sleepDuration)
	{
		JobSimulateWorkTime = jobSimulateWorkTime;
	}

	protected override async Task<JobResult<JobOk, JobFail>> ActionJob()
	{
		await Task.Delay(JobSimulateWorkTime);
		return new JobFail(this.Id, CronJobFailEnum.FailedExecute, "test error", null);
	}

	
}

internal class CronJobInternalExceptionResult : BaseCronJob
{
	private TimeSpan JobSimulateWorkTime { get; }

	public CronJobInternalExceptionResult(string name, TimeSpan sleepDuration, TimeSpan jobSimulateWorkTime) : base(name, "test error", sleepDuration)
	{
		JobSimulateWorkTime = jobSimulateWorkTime;
	}

	protected override async Task<JobResult<JobOk, JobFail>> ActionJob()
	{
		await Task.Delay(JobSimulateWorkTime);
		throw new Exception("some internal exp");
	}

}



internal class CronJobManagerManualRecheck : BaseCronJobManager
{
	public CronJobManagerManualRecheck(Config? config) : base(config)
	{
		
	}
	
	public Task<JobResult<JobOk, JobFail>[]> RecheckJobs()
	{
		return ExecuteReadyJob();
	}

	public new bool Register(ICronJob job)
	{
		return base.Register(job);
	}

	public new int Register(IEnumerable<ICronJob> jobs)
	{
		return base.Register(jobs);
	}

}