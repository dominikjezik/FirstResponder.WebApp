using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.Web.Validation;

namespace FirstResponder.Web.ViewModels;

public class AedFormViewModel
{
    public AedFormDTO AedFormDTO { get; set; } = new AedFormDTO();
    
    [Display(Name = "Banner image")]
    [AllowedMimeTypes("image/jpeg", "image/png")]
    public IFormFile[]? AedPhotoFormFiles { get; set; }
    
    // Edit Aed fields
    public string[]? AedPhotosToDelete { get; set; }
    
    public string GetAedPhotosJS()
    {
        string output = "[";

        foreach (var photo in AedFormDTO.Photos)
        {
            output += "{ id: '" + photo.Id + "', photoName: '" + photo.PhotoName + "' },";

        }
        
        output = output.Remove(output.Length - 1);
        output += "]";

        return output;
    }
    
}