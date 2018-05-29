using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Areas.Api.Models
{
    public class ContactViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
