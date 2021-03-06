using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        //Very important CMT hadling model state errors 
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    

    public static int CalculateAge(this DateTime thedatetime)
    {
        var age = DateTime.Today.Year - thedatetime.Year;
        if (thedatetime.AddYears(age) > DateTime.Today)
        {
            age--;
        }
        return age;
    }}
}