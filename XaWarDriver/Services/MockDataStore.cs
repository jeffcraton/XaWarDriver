using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaWarDriver.Models;

namespace XaWarDriver.Services
{
    public class MockDataStore : IDataStore<Networkreadings>
    {
        readonly List<Networkreadings> items;

        public MockDataStore()
        {
            items = new List<Networkreadings>()
            {
                new Networkreadings { Id = Guid.NewGuid().ToString(), ssid = "12345", networkname="Homenet" },
                new Networkreadings { Id = Guid.NewGuid().ToString(), ssid = "22245", networkname="att56uh" },
                new Networkreadings { Id = Guid.NewGuid().ToString(), ssid = "87655", networkname="veriz66" },
                new Networkreadings { Id = Guid.NewGuid().ToString(), ssid = "112299", networkname="Homenet99" },
                new Networkreadings { Id = Guid.NewGuid().ToString(), ssid = "99017", networkname="thorslaur" },
                new Networkreadings { Id = Guid.NewGuid().ToString(), ssid = "34341", networkname="loki" }
            };
        }

        public async Task<bool> AddItemAsync(Networkreadings item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Networkreadings item)
        {
            var oldItem = items.Where((Networkreadings arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Networkreadings arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Networkreadings> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Networkreadings>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}