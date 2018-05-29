using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Entities
{
    public class Message
    {
        [Key]
        public string Id { get; set; }
        public long Date { get; set; }
        public string Text { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
    }
}
