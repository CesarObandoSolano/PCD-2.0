using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plataforma.Areas.PCD.Controllers
{
    public class ProgramadorTarea
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<NotificarVencimiento>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 00))
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}