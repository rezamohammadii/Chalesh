using Chalesh.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Services
{
    public interface IStoreData
    {
        void StoreMainServiceDataOut(MainServiceDataModelOut? modelOut);
        MainServiceDataModelOut GetMainServiceDataOut();
        void StoreService2DetailOut(Service2DetailModel modelOut);
        void StoreConsumerModel(ConsumerModel? consumerModel);
        ConsumerModel GetConsumerModel();
    }
}
