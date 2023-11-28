using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Model.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Code has to be maximum 50 charecters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Code has to be maximum 1000 charecters")]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }
     
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
