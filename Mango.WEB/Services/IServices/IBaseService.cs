﻿using Mango.WEB.Models;
using Mango.WEB.Models.DTO;

namespace Mango.WEB.Services.IServices
{
    public interface IBaseService:IDisposable
    {
        ResponseDTO responseModel { get; set; }
        Task<T/* return type generic oluyor bu sekilde*/> SendAsync/*Method name*/<T/*Method tipi*/>(ApiRequest apiRequest);

    }
}
