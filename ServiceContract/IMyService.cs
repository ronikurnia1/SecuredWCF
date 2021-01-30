using System.ServiceModel;


namespace ServiceContract
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string CreateData(string id);

        [OperationContract]
        string ReadData(string id);

        [OperationContract]
        string UpdateData(string id);

        [OperationContract]
        string DeleteData(string id);
    }
}
