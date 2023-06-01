using System.Threading.Tasks;
using System;

namespace FreeCourse.Web.Services.Interfaces
{
	public interface IClientCredentialTokenService
	{
		Task<String> GetToken();
	}
}
