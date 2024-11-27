using HieLie.Domain.Entities;


namespace HieLie.Domain.Core.Specifications
{
    public static class UserSpecification
    {
        public static BaseSpecification<User> GetUserByEmail(string email)
        {
            return new BaseSpecification<User>(u => u.Email == email);
        }

        public static BaseSpecification<User> GetUserByRefreshToken(string refreshToken)
        {
            var specification = new BaseSpecification<User>(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            specification.AddInclude(u => u.RefreshTokens);

            return specification;
        }

        public static BaseSpecification<RefreshToken> GetRefreshToken(string refreshToken)
        {
            return new BaseSpecification<RefreshToken>(rt => rt.Token == refreshToken);
        }

        public static BaseSpecification<User> GetUserWithRefreshTokens(Guid userId)
        {
            var specification = new BaseSpecification<User>(u => u.Id == userId);
            specification.AddInclude(u => u.RefreshTokens);

            return specification;
        }
    }
}
