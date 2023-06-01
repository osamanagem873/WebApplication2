using Newtonsoft.Json;
using System.Runtime.Serialization;
using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<Items> Items { get; set; }

    }
}
