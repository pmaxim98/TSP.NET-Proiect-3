using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceMyPhotos;

namespace MyPhotosASP.Pages.Photos
{
    public class DetailsModel : PageModel
    {
        private MyPhotosServiceClient service = new MyPhotosServiceClient();

        public MultimediaDTO Multimedia { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Multimedia = await service.GetMultimediaByIdAsync(id.Value);

            return Page();
        }

        public string GetNameFromPath()
        {
            return Path.GetFileName(Multimedia.Path);
        }

        public string GetUploadPath()
        {
            return "/uploads/" + Multimedia.Id + Path.GetExtension(Multimedia.Path);
        }
    }
}