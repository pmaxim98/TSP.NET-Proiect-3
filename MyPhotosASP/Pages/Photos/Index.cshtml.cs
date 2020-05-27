using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceReferenceMyPhotos;

namespace MyPhotosASP.Pages.Photos
{
    public class IndexModel : PageModel
    {
        private MyPhotosServiceClient service = new MyPhotosServiceClient();

        private readonly IWebHostEnvironment hostEnvironment;

        public List<MultimediaDTO> Multimedias { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public enum SearchFilter
        {
            Name,
            Description,
            Tags,
            Weather,
            DateAdded,
            DateModified
        }

        [BindProperty(SupportsGet = true)]
        public SearchFilter Filter { get; set; }

        public IndexModel(IWebHostEnvironment environment)
        {
            hostEnvironment = environment;

            Multimedias = new List<MultimediaDTO>();
        }

        public async Task OnGetAsync()
        {
            List<MultimediaDTO> everyMultimedia = await service.GetEveryMultimediaAsync();

            if (!string.IsNullOrEmpty(SearchString))
                everyMultimedia = FilterMultimedia(everyMultimedia);

            foreach (MultimediaDTO multimedia in everyMultimedia)
            {
                var uploads = Path.Combine(hostEnvironment.WebRootPath, "uploads");
                var filePath = Path.ChangeExtension(Path.Combine(uploads, multimedia.Id.ToString()), Path.GetExtension(multimedia.Path));

                if (!System.IO.File.Exists(filePath))
                    System.IO.File.Copy(multimedia.Path, filePath);

                Multimedias.Add(multimedia);
            }
        }

        public List<MultimediaDTO> FilterMultimedia(List<MultimediaDTO> multimediaList)
        {
            if (Filter == SearchFilter.Name)
                return multimediaList.Where(s => s.Path.Contains(SearchString)).ToList();

            if (Filter == SearchFilter.Description)
                return multimediaList.Where(s => s.Description.Contains(SearchString)).ToList();

            if (Filter == SearchFilter.Tags)
                return multimediaList.Where(s => s.AdditionalLabels.Contains(SearchString)).ToList();

            if (Filter == SearchFilter.Weather)
                return multimediaList.Where(s => s.Weather.ToString().Contains(SearchString)).ToList();

            if (Filter == SearchFilter.DateAdded)
                return multimediaList.Where(s => s.DateAdded.ToString().Contains(SearchString)).ToList();

            if (Filter == SearchFilter.DateModified)
                return multimediaList.Where(s => s.DateModified.ToString().Contains(SearchString)).ToList();

            return multimediaList;
        }

        public string GetUploadPath(MultimediaDTO multimedia)
        {
            return "uploads/" + multimedia.Id + Path.GetExtension(multimedia.Path);
        }

        public string GetNameFromPath(MultimediaDTO multimedia)
        {
            return Path.GetFileName(multimedia.Path);
        }
    }
}