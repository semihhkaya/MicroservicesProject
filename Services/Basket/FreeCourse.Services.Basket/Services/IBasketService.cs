﻿using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto); //Sepette ilgili ürünyoksa oluşturacak save var ise update edicek
        Task<Response<bool>> Delete(string userId);
    }
}
