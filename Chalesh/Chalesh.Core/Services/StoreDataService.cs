using Chalesh.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Services
{
    public class StoreDataService : IStoreData
    {
        private static MainServiceDataModelOut? ModelOut;
        private static ConcurrentQueue<Service2DetailModel>? Service2Detail;
        private static ConsumerModel? ConsumerModels;
        public void StoreConsumerModel(ConsumerModel? consumerModel)
        {
            ConsumerModels.Message  = consumerModel.Message;
            ConsumerModels.Sender  = consumerModel.Sender;
            ConsumerModels.Id  = consumerModel.Id;
        }

        public void StoreMainServiceDataOut(MainServiceDataModelOut? modelOut)
        {
            ModelOut.IsEnabled = modelOut.IsEnabled;
            ModelOut.ExpirationTime = modelOut.ExpirationTime;
            ModelOut.NumberOfActiveClients = modelOut.NumberOfActiveClients;
        }

        public MainServiceDataModelOut GetMainServiceDataOut()
        {
            return ModelOut!;
        }

        public void StoreService2DetailOut(Service2DetailModel? modelOut)
        {
            Service2Detail.Enqueue(modelOut);
        }

        public ConsumerModel GetConsumerModel()
        {
            return ConsumerModels!;
        }
    }
}
