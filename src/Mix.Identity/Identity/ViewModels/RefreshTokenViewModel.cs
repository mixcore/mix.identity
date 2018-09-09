using System;
using Swastika.Domain.Data.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using Swastika.IO.Domain.Core.ViewModels;
using Swastika.Identity.Data;
using Swastika.IO.Identity.Identity.Entities;

namespace Swastika.IO.Identity.ViewModels
{
    public class RefreshTokenViewModel : ViewModelBase<ApplicationDbContext, RefreshToken, RefreshTokenViewModel>
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string Email { get; set; }

        public RefreshTokenViewModel(RefreshToken model, ApplicationDbContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #region Expands

        public static async Task<RepositoryResponse<bool>> LogoutOther(string refreshTokenId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                var token = await RefreshTokenViewModel.Repository.GetSingleModelAsync(t => t.Id == refreshTokenId, context, transaction);
                if (token.IsSucceed)
                {
                    var result = await RefreshTokenViewModel.Repository.RemoveListModelAsync(t => t.Id != refreshTokenId && t.Email == token.Data.Email);
                    if (result.IsSucceed)
                    {

                        if (transaction == null)
                        {
                            transaction.Commit();
                        }

                        return new RepositoryResponse<bool>()
                        {
                            IsSucceed = true,
                            Data = true
                        };
                    }
                    else
                    {
                        if (transaction == null)
                        {
                            transaction.Rollback();
                        }

                        return new RepositoryResponse<bool>()
                        {
                            IsSucceed = false,
                            Data = false
                        };
                    }
                }
                else
                {
                    if (transaction == null)
                    {
                        transaction.Rollback();
                    }

                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                context.Dispose();
            }

        }

        #endregion

    }
}
