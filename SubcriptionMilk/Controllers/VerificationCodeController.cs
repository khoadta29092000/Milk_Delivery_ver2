using BusinessObject.Models;
using DataAccess.Repository;
using EASendMail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubcriptionMilk.DTO;
using System.Dynamic;

namespace SubcriptionMilk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationCodeController : ControllerBase
    {
        private readonly IRepositoryAccount repositoryAccount;
        private readonly IRepositoryVerificationCode repositoryVerificationCode;
        private readonly IConfiguration configuration;
        public VerificationCodeController(IRepositoryAccount _repositoryAccount, IRepositoryVerificationCode _repositoryVerificationCode, IConfiguration configuration)
        {
            repositoryAccount = _repositoryAccount;
            this.configuration = configuration;
            repositoryVerificationCode = _repositoryVerificationCode;
        }
        private static string GenerateVerificationCode()
        {
            // Tạo mã xác thực ngẫu nhiên (ví dụ: 6 chữ số)
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code.ToString();
        }
        [HttpPost("VerificationCode")]
        public async Task<ActionResult> PostVerification(VerificationCodeDTO code)
        {
            try
            {
                TblVerificationCode verificationCode = await repositoryVerificationCode.GetVerificationCodeById(code.AccountID, code.Code);
                if(verificationCode != null)
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeDifference = currentTime - verificationCode.ExpirationTime;
                    if (timeDifference.TotalMinutes > 5)
                    {
                        return Ok(new { StatusCode = 409, Message = "Code has expired time" });
                    }
                    else
                    {
                        await repositoryAccount.VerificationAccount(code.AccountID, true);
                        await repositoryVerificationCode.DeleteVerificationCode(verificationCode.Id);
                        return Ok(new { StatusCode = 200, Message = "Verification Successfully" });
                    }
                 
                }
                else
                {
                    return Ok(new { StatusCode = 409, Message = "Code wrong" });
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost("ResendCode")]
        public async Task<ActionResult> PostResendCode(ResentCodeDTO code)
        {
            try
            {
                var acc = await repositoryAccount.GetProfile(code.AccountId);
                string verificationCode = GenerateVerificationCode();
                string subject = "Verification Code";
                string body = @"<!DOCTYPE html>
                              <html lang=""en"">
                              <head>
                              <meta charset=""utf-8"" />
                              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                              </head>
                              <body style="" text-align: center; padding: 40px 0; background: #EBF0F5;  width: 100%; height: 100%;"">
                              <div style="" background: white;   padding-left: 300px;padding-right: 300px; padding-top: 50;padding-bottom: 50px;border-radius: 4px;
                              box-shadow: 0 2px 3px #C8D0D8;display: inline-block;margin: 0 auto;"" class=""card"">
                              <h1 style="" color: #008CBA;font-size: 40px;"">Milk</h1>
                              <hr />
                              <img style="""" src=""https://firebasestorage.googleapis.com/v0/b/carmanaager-upload-file.appspot.com/o/images%2Flogo-color%20(1).pngeddab547-393c-4a52-8228-389b00aeacad?alt=media&token=a9fe5d57-b871-46a0-a9da-508902f09832&fbclid=IwAR0naf-IAgqC0ireg_vTPIvu9q0dK_n0gqKdNHhWFhvOyvhjWph-boPTWYk"" />
                              <h1 style="" color: #59c91c; font-family: "" Nunito Sans"", ""Helvetica Neue"" , sans-serif;font-weight: 900;font-size: 40px;margin-bottom: 10px;"">
                              Verification Code
                              </h1>
                              <p style="" padding-top: 5px;color: #404F5E;font-family: "" Nunito Sans"", ""Helvetica Neue"" , sans-serif;font-size: 20px;margin: 0;"">"
                              + verificationCode +
                              " </p></body></html>";
                var newVerificationCode = new TblVerificationCode
                {
                    AccountId = code.AccountId,
                    Code = verificationCode,
                    ExpirationTime = DateTime.Now,
                };
                await repositoryVerificationCode.AddVerificationCode(newVerificationCode);
                SmtpMail oMail = new SmtpMail("TryIt");
                oMail.From = "system.milk.delivery@gmail.com";
                oMail.To = acc.Email;
                oMail.Subject = subject;
                oMail.HtmlBody = body;
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                oServer.User = "system.milk.delivery@gmail.com";
                oServer.Password = "ukbhmjdaaacdyyxh";

                // Set 465 port
                oServer.Port = 465;

                // detect SSL/TLS automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto; ;
                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);
                return Ok(new { StatusCode = 200, Message = "Resend Code Successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
    }
}
