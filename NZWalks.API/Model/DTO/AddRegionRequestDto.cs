using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Model.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code has to be minimum 3 charecters")]
        [MaxLength(3, ErrorMessage ="Code has to be maximum 3 charecters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Code has to be maximum 50 charecters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
