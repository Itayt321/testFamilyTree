using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace testFamilyTree.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyTreeController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public FamilyTreeController(IConfiguration configuration)
        {
            _configuration = configuration;       
        }

        [HttpPost,Authorize]
        public IActionResult PostFamilyTree(List<FamilyMemberParent> FamilyMemberParents)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Infinite)
                .CreateLogger();

            try
            {
                Log.Information("Application started");

                List<FamilyMemberParent> peopleList = FamilyMemberParents.OrderBy(p => p.Parent).ToList();

                int nullsCount = peopleList.Count(item => item.Parent == null);

                var duplicates = peopleList.GroupBy(x => x.Id)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key);

                var idSet = new HashSet<int>(peopleList.Select(obj => obj.Id));
                bool allParentsExist = true;
                foreach (var obj in peopleList)
                {
                    if (obj.Parent.HasValue && !idSet.Contains(obj.Parent.Value))
                    {
                        allParentsExist = false;
                        break;
                    }
                }


                if (nullsCount > 1)
                    throw new ArgumentNullException($"There are {nullsCount} nulls in the list, only 1 object can contain null");
            
                if (nullsCount < 1)
                    throw new ArgumentNullException($"At least 1 object has to contain null in his parentId property");
              
                if (duplicates.Any())
                    throw new DuplicateWaitObjectException($"There are {duplicates} objects with the same Id");
                
                if (!allParentsExist)
                    throw new FormatException("Parent Id not found");

                FamilyMemberParent familyMember = new FamilyMemberParent();
                var formatedTree = familyMember.familyMembersFunc(peopleList);
                Log.Information("Application finished");
                return Ok(formatedTree);
            }

            catch (ArgumentNullException ex)
            {
                Log.Error(ex.ParamName);
                return BadRequest(ex.ParamName);
            }
            catch (DuplicateWaitObjectException ex)
            {
                Log.Error(ex.ParamName);
                return Conflict(ex.ParamName);
            }
            catch (FormatException ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }        
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }


        //BONUS:
        [HttpPost("GetToken"), AllowAnonymous]
        public string GetToken(string password = "123")
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, password)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
