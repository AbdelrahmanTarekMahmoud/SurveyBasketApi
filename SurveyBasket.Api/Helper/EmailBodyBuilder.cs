﻿//for html placholders
using Microsoft.AspNetCore.OutputCaching;

namespace SurveyBasket.Api.Helper
{
    public static class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template , Dictionary<string, string> placeHolders)
        {
            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
            var streamReader = new StreamReader(templatePath) ;
            var body = streamReader.ReadToEnd() ;
            streamReader.Close() ;
            foreach( var item in placeHolders )
            {
                body = body.Replace(item.Key, item.Value);
            }
            return body;
        }
    }
}
