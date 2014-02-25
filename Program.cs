using System;
using FeatureLoggerService.Repositories;
using FeatureLoggerService.Services;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace FeatureLoggerService
{
    class Program
    {
        public static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());

                var container = new UnityContainer();
                container
                    .RegisterType(typeof (IRepository<>), typeof (Repository<>))
                    .RegisterType<IFeatureLogService, FeatureLogService>();

                
                using (var host = new UnityServiceHost(container, typeof (FeatureLogService)))
                {
                    try
                    {
                        host.Open();
                        Console.WriteLine("Service is listening ... ");
                        Console.ReadKey();   
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
