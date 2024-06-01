﻿using WebApplication2.Data;

namespace WebApplication2.Services
{
    public interface IEmailService
    {
       

        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}