using App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Services
{
    public interface IAppUserManagerService
    {
        Task<AppUserModel> GetUserByEmail(string email);
        Task<AppUserModel> GetAntherUserByEmail(string email, string id);
        AppUserModel GetUserByMobile(string mobile);
        List<AppUserModel> LoadAllUsers();
        Task<List<AppUserModel>> LoadAllClients();
        Task<ResponseModel<BooleanResultDTO>> AddUser(AppUserModel model);
        Task<ResponseModel<BooleanResultDTO>> UpdateUser(AppUserModel model);
        Task<ResponseModel<BooleanResultDTO>> DeleteUser(string id);
        ResponseModel<BooleanResultDTO> IsExistUser(AppUserModel model);
        Task<ResponseModel<BooleanResultDTO>> ActivateUser(string email, string code);
        ResponseModel<AppUserModel> GetUserBySocialLoginId(string socialLoginId);
        Task<AppUserModel> GetUserById(string id);
        Task<ResponseModel<BooleanResultDTO>> ResetPassword(string email, string code, string newPassword);
    }
}
