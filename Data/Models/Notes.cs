using iText.StyledXmlParser.Node;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class Notes
    {
        public string Id { get; set; } 
        public string PlayerId { get; set; }
        public string CoachId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
