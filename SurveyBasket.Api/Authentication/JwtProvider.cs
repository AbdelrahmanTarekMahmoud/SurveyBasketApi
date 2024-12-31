


namespace SurveyBasket.Api.Authentication
{
    public class JwtProvider(IOptions<JwtOptions> JwtOptions) : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions = JwtOptions.Value;

        public (string token, int expiresIn) GenerateToken(ApplicationUser user,
            IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub , user.Id),
                new(JwtRegisteredClaimNames.Email , user.Email!),
                new(JwtRegisteredClaimNames.GivenName , user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName , user.LastName),
                new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new(nameof(roles) , JsonSerializer.Serialize(roles) , JsonClaimValueTypes.JsonArray),
                new(nameof(permissions) , JsonSerializer.Serialize(permissions) , JsonClaimValueTypes.JsonArray)
            ];

            //key to coding and decoding
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);



            //token structure
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                signingCredentials: signingCredentials
            );
            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _jwtOptions.ExpiryMinutes * 60);
        }

    }
}