﻿namespace Mongo.Services.ProductAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSUCCESS { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; }

    }
}
