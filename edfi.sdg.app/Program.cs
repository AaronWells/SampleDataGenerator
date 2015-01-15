using EdFi.SampleDataGenerator;
using EdFi.SampleDataGenerator.Utility;

namespace edfi.sdg.app
{
    using Topshelf;

    public static class Program
    {
        static int Main()
        {
            var serviceParams = new ServiceParams { Bootstrap = false };
            return (int)HostFactory.Run(x =>
                {
                    x.AddCommandLineDefinition("bootstrap",
                       b =>
                       {
                           bool tmp;
                           if (bool.TryParse(b, out tmp)) serviceParams.Bootstrap = tmp;
                       });
                    x.Service<Service>(s =>
                            {
                                s.ConstructUsing(name => new Service(serviceParams, new ConsoleLogger()));
                                s.WhenStarted(tc => tc.Start());
                                s.WhenStopped(tc => tc.Stop());
                            });

                    x.SetServiceName("edfi.sdg.app");
                    x.SetDisplayName("Ed-Fi Sample Data Generator");
                    x.SetDescription("Ed-Fi Sample Data Generator");

                    x.DependsOnMsmq();
                    x.DependsOnMsSql();

                    x.RunAsLocalSystem();
                });
        }
    }
}
