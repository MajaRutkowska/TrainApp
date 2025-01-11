using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TrainApp.Data.Models
{
    public class Material
    {
        public string MaterialId { get; set; }
        public string ExerciseId { get; set; }
        public string FileName { get; set; }
        public byte[] PdfFile { get; set; }
        public virtual Exercise Exercise { get; set; }
    }
}
