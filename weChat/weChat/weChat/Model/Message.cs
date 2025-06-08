using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weChat.Model
{
    [Table("messages")]
    public class Message : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("senderid")]
        public Guid? SenderId { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("users")]  // must match the join alias in Select()
        public User users { get; set; }

    }
}
