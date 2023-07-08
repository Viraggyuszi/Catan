using Catan.Shared.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
	public class ActorIdentity
	{
		private static Dictionary<string, string> ActorCache = new Dictionary<string, string>();
		public static bool CheckActorIdentity(Actor actor)
		{
			if (actor is null || actor.Name is null || actor.Token is null)
			{
				return false;
			}
			string value = "";
			if (ActorCache.TryGetValue(actor.Name, out value) && value is not null && value == actor.Token)
			{
				return true;
			}
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadToken(actor.Token) as JwtSecurityToken;
			if (jwtToken is not null)
			{
				var usernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "username");
				if (usernameClaim is not null)
				{
					if (usernameClaim.Value.Equals(actor.Name))
					{
						ActorCache.Add(actor.Name, actor.Token);
						return true;
					}
				}
			}
			return false;
		}
	}
}
