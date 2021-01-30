using DummyService.Extensions;
using ServiceContract;
using System.ServiceModel;

namespace DummyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MyService : IMyService
    {
        public static void Configure(ServiceConfiguration serviceConfiguration)
        {
            serviceConfiguration.ApplyCustomAuthentication(typeof(IMyService), Config.ServiceEndpoint);
        }

        public string CreateData(string id)
        {
            return $"Data {id} created successfully.";
        }

        public string DeleteData(string id)
        {
            return $"Data {id} deleted successfully.";
        }

        public string ReadData(string id)
        {
            return $"Data {id} read successfully.";
        }

        public string UpdateData(string id)
        {
            return $"Data {id} updated successfully.";
        }
    }
}
