using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signInInput); //IdentityModel kütüphaneisnden yararlanıcaz

        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken(); //kullanıcı logout olduğunda onun için tanımlı token kaldırılacak.
    }
}
