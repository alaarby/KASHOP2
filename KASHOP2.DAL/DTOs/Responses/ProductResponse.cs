using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public string CreatedBy { get; set; }
        public string MainImage { get; set; }
        public List<CategoryTranslationResponse> Ttranslations { get; set; }
        public List<string> SubImages { get; set; }
    }
}
