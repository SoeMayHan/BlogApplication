using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogApplication.Models;
using BlogApplication.Data;
using BlogApplication.Common;

namespace BlogApplication.IAM
{
    public class JWTAuthenication
    {

        private const string privateKey = "abcedsdkfnsjknfsbfhbifuweoho"; 
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        SigningCredentials creds;
        JwtSecurityTokenHandler handler;
        TokenValidationParameters validationParameters;

        public JWTAuthenication()
        {
            creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
            handler = new JwtSecurityTokenHandler();
            validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = key,
                ValidAudience = "smh",
                ValidIssuer = "smh"
            };
        }

        public string CreateAccessToken(string Username, string Userid, string roleCd)
        {

            ClaimsIdentity claims = new ClaimsIdentity(new List<Claim>() {
                new Claim(ClaimTypes.Name,Username),
                new Claim(ClaimTypes.NameIdentifier,Userid),
                new Claim(ClaimTypes.Role,roleCd)
            }, "custom");
            var token = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
            {
                Subject = claims,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds,
                Audience = "smh",
                Issuer = "smh"
            });
            return handler.WriteToken(token);
        }
        public JwtSecurityToken ValidateToken(string token)
        {
            SecurityToken validatedToken;
            try
            {
                handler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return null;
            }
            return validatedToken as JwtSecurityToken;

        }

        public User GetUserFromAccessToken(JwtSecurityToken token, bool refreshUser)
        {

            string name = token.Claims.FirstOrDefault((c) => c.Type == "unique_name").Value;
            string Userid = token.Claims.FirstOrDefault((c) => c.Type == "nameid").Value;
            string roleCd = token.Claims.FirstOrDefault((c) => c.Type == "role").Value;
            User User = new User();
            if (refreshUser)
            {
                using (ApplicationDbContext ctx = new ApplicationDbContext())
                {
                    User = ctx.User.Find(new string[] { Userid });
                }
            }
            else
            {
                User.UserId = Userid;
                User.UserName = name;
                User.UserRole = roleCd;
            }
            return User;
        }

        public string CreateRefreshToken(string UserId)
        {
            //hash the Username 
            string hashedUserId = Base64UrlEncoder.Encode(UserId);
            ClaimsIdentity claims = new ClaimsIdentity(new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier,hashedUserId)
            }, "custom");

            var token = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
            {
                Subject = claims,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = creds
            });
            return handler.WriteToken(token);
        }

        public JWTAuthenticationToken GetNewAccessTokenFromRefreshToken(string refreshToken, ApplicationDbContext ctx)
        {
            JwtSecurityToken token = ValidateToken(refreshToken);
            if (token == null)
                throw new BlogException("tokenInvalid", new string[] { token.ToString() });
            string hashedUserId = token.Claims.First((c) => c.Type == ClaimTypes.NameIdentifier).Value;
            string UserId = Base64UrlEncoder.Decode(hashedUserId);

            var query = (from a in ctx.User where a.UserId == UserId select a);
            if (query.Count() == 0)
            {
                throw new BlogException("userNotFound", new string[] { UserId });
            }
            else
            {
                User u = query.First();
                string newToken = CreateAccessToken(u.UserName, UserId, u.UserRole);
                string newRefreshToken = CreateRefreshToken(UserId);
                JWTAuthenticationToken newAuthToken = new JWTAuthenticationToken();
                newAuthToken.Token = newToken;
                newAuthToken.RefreshToken = newRefreshToken;
                newAuthToken.User = u;
                return newAuthToken;
            }
                
        }
    }

    
}