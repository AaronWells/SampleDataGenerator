namespace edfi.sdg.app
{
    using Topshelf;

    class Program
    {
        static int Main(string[] args)
        {
            return (int) HostFactory.Run(x =>
                    {
                        x.Service<Service>(s =>
                                {
                                    s.ConstructUsing(name => new Service());
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
