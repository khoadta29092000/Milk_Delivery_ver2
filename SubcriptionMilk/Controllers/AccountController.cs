using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SubcriptionMilk.DTO;
using System.Security.Cryptography;
using EASendMail;
using System.Xml;

namespace SubcriptionMilk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepositoryAccount repositoryAccount;
        private readonly IRepositoryVerificationCode repositoryVerificationCode;
        private readonly IConfiguration configuration;
        public AccountController(IRepositoryAccount _repositoryAccount, IRepositoryVerificationCode _repositoryVerificationCode, IConfiguration configuration)
        {
            repositoryAccount = _repositoryAccount;
            this.configuration = configuration;
            repositoryVerificationCode = _repositoryVerificationCode;
        }




        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        private static string GenerateVerificationCode()
        {
            // Tạo mã xác thực ngẫu nhiên (ví dụ: 6 chữ số)
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code.ToString();
        }
        public static string GenerateHashedPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] hashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, passwordBytes.Length, saltBytes.Length);

                hashedPasswordBytes = sha256.ComputeHash(passwordWithSaltBytes);
            }

            return Convert.ToBase64String(hashedPasswordBytes);
        }
        [HttpGet]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAll(string? search, int RoleId, int page, int pageSize)
        {

            try
            {
                var AccountList = await repositoryAccount.SearchByEmail(search, RoleId, page, pageSize);
                var Count = AccountList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = AccountList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Getaccount(string id)
        {
            try
            {
                var Result = await repositoryAccount.GetProfile(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult> GetLogin(LoginDTO acc)
        {
            try
            {

                TblAccount customer = await repositoryAccount.LoginMember(acc.Email, acc.Password);
                if (customer != null)
                {
                    if (customer.IsVerified == true)
                    {
                        if (customer.IsDeleted == false)
                        {

                            return Ok(new { StatusCode = 200, Message = "Login succedfully", data = GenerateToken(customer) });
                        }
                        else
                        {
                            return Ok(new { StatusCode = 409, Message = "Account Not Active" });
                        }
                    }
                    else
                    {
                        return Ok(new { StatusCode = 405, Message = "Your account has not been verified" });
                    }

                }
                else
                {
                    return Ok(new { StatusCode = 409, Message = "Email or Password is valid" });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        private string GenerateToken(TblAccount acc)
        {
            var secretKey = configuration.GetSection("AppSettings").GetSection("SecretKey").Value;

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Email, acc.Email),
                    new Claim(ClaimTypes.Role, acc.RoleId.ToString()),
                    new Claim("Id", acc.Id.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(AccountDTO acc)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string formatDate = currentDate.ToString("ddMMyyyy");
                var saltPassword = GenerateSalt();
                var hashPassword = GenerateHashedPassword(acc.Password, saltPassword);
                var AccountList = await repositoryAccount.GetMembers();
                int count = AccountList.Count(item => item.Id.StartsWith("ACC" + currentDate.ToString("ddMMyyyy")));
                var id = "ACC" + formatDate + (count + 1);
                var newAcc = new TblAccount
                {
                    IsDeleted = acc.IsDeleted,
                    Address = acc.Address,
                    ImageCard = acc.ImageCard,
                    CreateDate = acc.CreateDate,
                    Email = acc.Email,
                    FullName = acc.FullName,
                    Gender = acc.Gender,
                    RoleId = acc.RoleId,
                    PhoneNumber = acc.PhoneNumber,
                    Id = id,
                    HashedPassword = hashPassword,
                    SaltPassword = saltPassword,
                    IsVerified = false,
                };
                await repositoryAccount.AddMember(newAcc);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO acc)
        {
            try
            {
                if (acc.ConfirmPassword != acc.Password)
                {
                    return StatusCode(409, new { StatusCode = 409, Message = "Confirm Password not correct password" });
                }
                DateTime currentDateSQL = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
                DateTime currentDate = DateTime.Now;
                string formatDate = currentDate.ToString("ddMMyyyy");
                var saltPassword = GenerateSalt();
                var hashPassword = GenerateHashedPassword(acc.Password, saltPassword);
                var AccountList = await repositoryAccount.GetMembers();
                int count = AccountList.Count(item => item.Id.StartsWith("ACC" + currentDate.ToString("ddMMyyyy")));
                var id = "ACC" + formatDate + (count + 1);
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
                var newAcc = new TblAccount
                {
                    IsDeleted = false,
                    Address = null,
                    ImageCard = "https://bloganchoi.com/wp-content/uploads/2022/02/avatar-trang-y-nghia.jpeg",
                    CreateDate = currentDateSQL,
                    Email = acc.Email,
                    FullName = acc.FullName,
                    Gender = acc.Gender,
                    HashedPassword = hashPassword,
                    SaltPassword = saltPassword,
                    Id = id,
                    PhoneNumber = acc.PhoneNumber,
                    RoleId = 2,
                    IsVerified = false
                };
                var newVerificationCode = new TblVerificationCode
                {
                    AccountId = id,
                    Code = verificationCode,
                    ExpirationTime = DateTime.Now,
                };
                await repositoryAccount.AddMember(newAcc);
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


                return Ok(new { StatusCode = 200, Message = "Register successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost("Login_Google")]
        public async Task<ActionResult> GetLoginGoogle(Token token)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token.token);
                string email = jsonToken.Claims.First(claim => claim.Type == "email").Value;
                string avatar = jsonToken.Claims.First(claim => claim.Type == "picture").Value;
                string name = jsonToken.Claims.First(claim => claim.Type == "name").Value;
                var AccountList = await repositoryAccount.GetMembers();
                var isExists = AccountList.SingleOrDefault(x => x.Email == email);
                string formatDate = currentDate.ToString("ddMMyyyy");
                int count = AccountList.Count(item => item.Id.StartsWith("ACC" + currentDate.ToString("ddMMyyyy")));
                var id = "ACC" + formatDate + (count + 1);
                if (isExists == null)
                {
                    var newAcc = new TblAccount
                    {
                        IsDeleted = false,
                        ImageCard = avatar,
                        Email = email,
                        FullName = name,
                        RoleId = 2,
                        IsVerified = true,
                        CreateDate = currentDate,
                        SaltPassword = GenerateSalt(),
                    };
                    await repositoryAccount.AddMember(newAcc);
                    var member = AccountList.SingleOrDefault(x => x.Email == newAcc.Email);
                    return Ok(new { StatusCode = 200, Message = "Login SuccessFully", data = GenerateToken(newAcc) });
                }
                else
                {
                    return Ok(new { StatusCode = 200, Message = "Login SuccessFully", data = GenerateToken(isExists) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> update(string id, TblAccount acc)
        {
            if (id != acc.Id)
            {
                return BadRequest();
            }
            try
            {
                var accId = await repositoryAccount.GetProfile(id);
                var Acc = new TblAccount
                {
                    IsDeleted = false,
                    Address = null,
                    ImageCard = acc.ImageCard,
                    CreateDate = acc.CreateDate,
                    Email = acc.Email,
                    FullName = acc.FullName,
                    HashedPassword = accId.HashedPassword,
                    SaltPassword = accId.SaltPassword,
                    Gender = acc.Gender,
                    Id = acc.Id,
                    PhoneNumber = acc.PhoneNumber,
                    RoleId = acc.RoleId,
                    IsVerified = true,
                };
                await repositoryAccount.UpdateMember(Acc);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangPassword(ChangePasswordDTO acc)
        {
            try
            {
                string userId = User.FindFirst("Id")?.Value;
                TblAccount account = await repositoryAccount.GetProfile(userId);
                if (account.HashedPassword == null)
                {
                    await repositoryAccount.ChangePassword(userId, GenerateHashedPassword(acc.NewPassword, account.SaltPassword));
                    return Ok(new { StatusCode = 200, Message = "ChangePassword successful" });
                }
                if (GenerateHashedPassword(acc.OldPassword, account.SaltPassword) != account.HashedPassword)
                {
                    return Ok(new { StatusCode = 400, Message = "Old Password not correct" });
                }
                else
                {
                    await repositoryAccount.ChangePassword(userId, GenerateHashedPassword(acc.NewPassword, account.SaltPassword));
                    return Ok(new { StatusCode = 200, Message = "ChangePassword successful" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("UpdateActive")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateActive(string id)
        {
            try
            {
                TblAccount acc = await repositoryAccount.GetProfile(id);
                if (acc == null)
                {
                    return Ok(new { StatusCode = 400, Message = "Id not Exists" });
                }
                else
                {
                    await repositoryAccount.UpdateActive(id, acc.IsDeleted);
                    return Ok(new { StatusCode = 200, Message = "Update Active successful" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await repositoryAccount.DeleteMember(id);
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
