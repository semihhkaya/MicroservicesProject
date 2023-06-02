using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        #region #region cancel token
        //Region canceltoken
        //Canceltoken alma amacı: diyelim ki buraya bir foto geldi örn 20 saniye sürsün kayıt
        //olması. Bu endpointi çağıran client işlemi sonlandırırsa burdaki foto kaydetme olayı da
        //devam etmesin. Örn tarayıcı kapattık kayıt işlemi varken cancel token işleme girecek. kayıt işlemi devam etmicek
        //asenkron bir işlemi ancak hata fırlatarak sonlandırabliriz canceltoken onu yapıyor
        #endregion

        [HttpPost] 
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo!=null && photo.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken); //tarayıcı vs kapanırsa istek cancel token sayesinde yarıda kesilir.
                

                var returnPath =photo.FileName;

                PhotoDto photoDto = new() { Url= returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
                

            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }

        public IActionResult PhotoDelete(string photoUrl)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found", 404));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
